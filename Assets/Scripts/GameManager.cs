using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SDD.Events;

public enum GAMESTATE { menu, play, pause, victory, gameover, highscore, credit }

public class GameManager : MonoBehaviour, IEventHandler
{
    private static GameManager m_Instance;

    public static GameManager Instance { get { return m_Instance; } }

    public AudioSource gameOverSound;
    public AudioSource victorySound;
    public BezierSpline spline;
    public int count = 0;
    public int launch_count = 0;
    public List<Balls> m_Walker = new List<Balls>();
    public List<Balls> launched_Walker = new List<Balls>();
    public GameObject Spawn;
    private GAMESTATE m_State;

    public bool IsPlaying { get { return m_State == GAMESTATE.play; } }
    public bool IsPaused { get { return m_State == GAMESTATE.pause; } }
    private bool IsMenu { get { return m_State == GAMESTATE.menu; } }
    private bool IsHighScore { get { return m_State == GAMESTATE.highscore; } }
    private bool IsCredit { get { return m_State == GAMESTATE.credit; } }
    private int highScore = 0;


    int m_Score;
    float deltatime;

    [SerializeField] float m_CountDownStartValue;
    float m_CountDown;

    [SerializeField] GameObject[] SplineWalker;

    public void SubscribeEvents()
    {
        EventManager.Instance.AddListener<PlayButtonClickedEvent>(PlayButtonClicked);
        EventManager.Instance.AddListener<AsteroidExplosionEvent>(AsteroidExplosion);
        EventManager.Instance.AddListener<MainMenuButtonClickedEvent>(MainMenuButtonClicked);
        EventManager.Instance.AddListener<HighScoreButtonClickedEvent>(HighScoreButtonClicked);
        EventManager.Instance.AddListener<EscapeButtonClickedEvent>(EscapeButtonClicked);
        EventManager.Instance.AddListener<CreditButtonClickedEvent>(CreditButtonClicked);
        EventManager.Instance.AddListener<GameOverEvent>(GameOver);

    }

    public void UnsubscribeEvents()
    {
        EventManager.Instance.RemoveListener<PlayButtonClickedEvent>(PlayButtonClicked);
        EventManager.Instance.RemoveListener<AsteroidExplosionEvent>(AsteroidExplosion);
        EventManager.Instance.RemoveListener<MainMenuButtonClickedEvent>(MainMenuButtonClicked);
        EventManager.Instance.RemoveListener<HighScoreButtonClickedEvent>(HighScoreButtonClicked);
        EventManager.Instance.RemoveListener<EscapeButtonClickedEvent>(EscapeButtonClicked);
        EventManager.Instance.RemoveListener<CreditButtonClickedEvent>(CreditButtonClicked);
        EventManager.Instance.RemoveListener<GameOverEvent>(GameOver);
    }

    private void OnEnable()
    {
        SubscribeEvents();
    }

    private void OnDisable()
    {
        UnsubscribeEvents();
    }

    void SetStatistics(int score, float countdown)
    {
        m_Score = score;
        m_CountDown = countdown;
        EventManager.Instance.Raise(new GameStatisticsChangedEvent() { eScore = score, eCountDownValue = Mathf.Max(countdown, 0) });
    }

    void InitGame()
    {
        SetStatistics(0, m_CountDownStartValue);
    }

    void Victory()
    {
        m_State = GAMESTATE.victory;
        victorySound.Play();
        EventManager.Instance.Raise(new GameVictoryEvent());
    }
    void GameOver(GameOverEvent e)
    {
        m_State = GAMESTATE.gameover;
        gameOverSound.Play();
    }

    void IncrementScore(int scoreIncrement)
    {
        m_Score += scoreIncrement;
        SetStatistics(m_Score, m_CountDown);

        if (m_Score > highScore)
        {
            PlayerPrefs.SetInt("highScore", m_Score);
        }
    }

    #region Events callbacks
    void PlayButtonClicked(PlayButtonClickedEvent e)
    {
        m_State = GAMESTATE.play;
        InitGame();
        EventManager.Instance.Raise(new GamePlayEvent());
    }
    void MainMenuButtonClicked(MainMenuButtonClickedEvent e)
    {
        m_State = GAMESTATE.menu;
        EventManager.Instance.Raise(new GameMenuEvent());
    }

    void HighScoreButtonClicked(HighScoreButtonClickedEvent e)
    {
        m_State = GAMESTATE.highscore;
        EventManager.Instance.Raise(new GameHighScoreEvent());
    }

    void EscapeButtonClicked(EscapeButtonClickedEvent e)
    {
        if (m_State == GAMESTATE.menu)
        {
            Application.Quit();
        }
        else if (m_State == GAMESTATE.play)
        {
            m_State = GAMESTATE.pause;
            EventManager.Instance.Raise(new GamePauseEvent());
        }
        else if (m_State == GAMESTATE.pause)
        {
            m_State = GAMESTATE.play;
            EventManager.Instance.Raise(new GameResumeEvent());
        }
    }
    void CreditButtonClicked(CreditButtonClickedEvent e)
    {
        m_State = GAMESTATE.credit;
        EventManager.Instance.Raise(new GameCreditEvent());
    }
    void AsteroidExplosion(AsteroidExplosionEvent e)
    {
        if (!IsPlaying) return;
        IncrementScore(5);
    }
    #endregion


    private void Awake()
    {
        if (m_Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            m_Instance = this;
        }
    }
    // Start is called before the first frame update
    IEnumerator Start()
    {
        m_State = GAMESTATE.menu;
        EventManager.Instance.Raise(new GameMenuEvent());
        if (PlayerPrefs.HasKey("highScore")) //récupération du highscore d'une ancienne partie
        {
            highScore = PlayerPrefs.GetInt("highScore");
        }
        deltatime = 5;
        yield break;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (IsPlaying)
        {
            SetStatistics(m_Score, m_CountDown - Time.deltaTime);

            /*if (m_CountDown < 0)
            {
                Victory();
            }*/
            //Debug.Log("State SpawnPos : " + Spawn.GetComponent<SpawnPos>().isFree);
            if (Spawn.GetComponent<SpawnPos>().isFree)
            {
                int color_index = (int)Random.Range(1, 3);
                GameObject clone = Instantiate(SplineWalker[0], new Vector3(SplineWalker[0].transform.position.x, SplineWalker[0].transform.position.y, SplineWalker[0].transform.position.z), Quaternion.identity);
                clone.name = "Walker" + count++;
                string color = "Red";
                if (color_index == 2) color = "Green";
                if (color_index == 3) color = "Blue";
                clone.GetComponent<SplineWalker>().color = color;
                clone.GetComponent<SplineWalker>().index = count;
                m_Walker.Add(new Balls(color, count, clone));
                clone.SetActive(true);
                Spawn.GetComponent<SpawnPos>().isFree = false;
            }
            /*if ((deltatime - Time.fixedTime) <= 0)
            {
                int color_index = (int)Random.Range(1, 3);
                //GameObject instance = new GameObject("Walker" + m_CountDownStartValue);
                GameObject clone = Instantiate(SplineWalker[0], new Vector3(SplineWalker[0].transform.position.x, SplineWalker[0].transform.position.y, SplineWalker[0].transform.position.z), Quaternion.identity);
                clone.name = "Walker" + count++;
                string color = "Red";
                if (color_index == 2) color = "Green";
                if (color_index == 3) color = "Blue";
                m_Walker.Add(new Balls(color, count, clone));
                //print("nbr GO : " + m_Walker.Count);
                clone.SetActive(true);
                deltatime = 0.5f + Time.fixedTime;
            }*/

        }
        
        checkInputs();
    }

    void checkInputs()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.JoystickButton7))
        {
            EventManager.Instance.Raise(new EscapeButtonClickedEvent());
        }
        else if (IsMenu)
        {
            if (Input.GetKeyDown(KeyCode.JoystickButton0))
            {
                EventManager.Instance.Raise(new PlayButtonClickedEvent());
            }
            else if (Input.GetKeyDown(KeyCode.JoystickButton2))
            {
                EventManager.Instance.Raise(new HighScoreButtonClickedEvent());
            }
            else if (Input.GetKeyDown(KeyCode.JoystickButton3))
            {
                EventManager.Instance.Raise(new CreditButtonClickedEvent());
            }

        }
        else if (IsPaused) //Détection du menu pause
        {
            if (Input.GetKeyDown(KeyCode.JoystickButton1))
            {
                EventManager.Instance.Raise(new MainMenuButtonClickedEvent());
            }
            else if (Input.GetKeyDown(KeyCode.JoystickButton0))
            {
                EventManager.Instance.Raise(new ResumeButtonClickedEvent());
            }
        }
        else if (IsHighScore)
        {
            if (Input.GetKeyDown(KeyCode.JoystickButton1))
            {
                EventManager.Instance.Raise(new MainMenuButtonClickedEvent());
            }
        }
        else if (IsCredit)
        {
            if (Input.GetKeyDown(KeyCode.JoystickButton1))
            {
                EventManager.Instance.Raise(new MainMenuButtonClickedEvent());
            }
        }
    } //Gère tous les inputs lié au menu pendant le jeu

}
