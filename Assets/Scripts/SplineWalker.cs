using UnityEngine;
using SDD.Events;

public class SplineWalker : MonoBehaviour
{
    public void SubscribeEvents()
    {
        EventManager.Instance.AddListener<GamePlayEvent>(GamePlay);
        EventManager.Instance.AddListener<LevelHasBeenInitializedEvent>(LevelHasBeenInitialized);
    }

    public void UnsubscribeEvents()
    {
        EventManager.Instance.RemoveListener<GamePlayEvent>(GamePlay);
        EventManager.Instance.RemoveListener<LevelHasBeenInitializedEvent>(LevelHasBeenInitialized);
    }

    private void OnEnable()
    {
        SubscribeEvents();
    }

    private void OnDisable()
    {
        UnsubscribeEvents();
    }

    #region Events callbacks
    void GamePlay(GamePlayEvent e)
    {

    }

    void LevelHasBeenInitialized(LevelHasBeenInitializedEvent e)
    {
        
    }
    #endregion

    public BezierSpline spline;

    public string color;

    public int index;

    public float duration;

    public float progress;

    public bool lookForward;

    bool ScriptFound = false;

    private void Start()
    {
        try
        {
            spline = GameObject.Find("Spline").GetComponent<BezierSpline>();
            ScriptFound = true;
        }
        catch
        {
            Debug.Log("Pas de Script utilisable !!");
        }
    }

    private void Update()
    {
        if(GameManager.Instance.IsPlaying == true && ScriptFound)
        {
            progress += Time.deltaTime / duration;
            if (progress >= 1f)
            {
                //print("count in walker " + gameObject.name + " : " + GameManager.Instance.m_Walker.Contains(gameObject));

                GameManager.Instance.m_Walker.Remove(new Balls(color, index, gameObject));
                Destroy(gameObject);
                //progress = 1f;
            }
            Vector3 position = spline.GetPoint(progress);
            transform.localPosition = position;
            if (lookForward)
            {
                transform.LookAt(position + spline.GetDirection(progress));
            }
        }
    }
}