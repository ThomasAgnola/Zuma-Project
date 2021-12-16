using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SDD.Events;

public class HudManager : MonoBehaviour, IEventHandler
{
    [SerializeField] GameObject m_StatisticsContainer;
    [SerializeField] Text m_Score;
    [SerializeField] Text m_Time;
    [SerializeField] Text m_NewLevel;
    [SerializeField] RawImage m_NextBall;
    private float offTime, stayTime = 3.0f;

    public void SubscribeEvents()
    {
        EventManager.Instance.AddListener<GameStatisticsChangedEvent>(GameStatisticsChanged);
        EventManager.Instance.AddListener<BallChangedEvent>(NextBallChanged);
        EventManager.Instance.AddListener<GamePlayEvent>(GamePlay);
        EventManager.Instance.AddListener<GameMenuEvent>(GameMenu);
        EventManager.Instance.AddListener<NewlevelEvent>(NewLevel);
    }

    public void UnsubscribeEvents()
    {
        EventManager.Instance.RemoveListener<GameStatisticsChangedEvent>(GameStatisticsChanged);
        EventManager.Instance.RemoveListener<BallChangedEvent>(NextBallChanged);
        EventManager.Instance.RemoveListener<GamePlayEvent>(GamePlay);
        EventManager.Instance.RemoveListener<GameMenuEvent>(GameMenu);
        EventManager.Instance.RemoveListener<NewlevelEvent>(NewLevel);
    }

    private void OnEnable()
    {
        SubscribeEvents();
    }

    private void OnDisable()
    {
        UnsubscribeEvents();
    }

    void RefreshedScoreAndCountDown(int score, float countdown)
    {
        m_Score.text = score.ToString();
        m_Time.text = countdown.ToString("N01");
    }

    void DisplayNextBall(string color)
    {
        Debug.Log("Changement de couleur pour : " + color);
        if(color == "Red")
        {
            Debug.Log("Changement en rouge");
            m_NextBall.GetComponent<RawImage>().color = new Color(190, 22, 22);
        }
        else if(color == "Green")
        {
            Debug.Log("Changement en vert");
            m_NextBall.GetComponent<RawImage>().color = new Color(20, 142, 58);
        }
        else if(color == "Blue")
        {
            Debug.Log("Changement en bleu");
            m_NextBall.GetComponent<RawImage>().color = new Color(18, 25, 179);
        }
        /*else
        {
            m_NextBall.color = new Color(255, 255, 255);
        }*/
    }

    #region Events callbacks
    void GamePlay(GamePlayEvent e)
    {
        m_StatisticsContainer.SetActive(true);
    }
    void GameMenu(GameMenuEvent e)
    {
        m_StatisticsContainer.SetActive(false);
        m_NewLevel.gameObject.SetActive(false);
    }
    void NewLevel(NewlevelEvent e)
    {
        m_NewLevel.gameObject.SetActive(true);
        offTime = Time.time + stayTime;
    }
    void GameStatisticsChanged(GameStatisticsChangedEvent e)
    {
        RefreshedScoreAndCountDown(e.eScore, e.eCountDownValue);
    }
    void NextBallChanged(BallChangedEvent e)
    {
        DisplayNextBall(e.eColor);
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(m_NewLevel.enabled && (Time.time >= offTime))
        {
            m_NewLevel.gameObject.SetActive(false);
        }
    }
}
