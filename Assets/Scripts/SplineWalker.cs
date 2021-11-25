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

    public float duration;

    private float progress;

    public bool lookForward;

    private void Start()
    {
        Collider col = this.GetComponent<Collider>();
    }

    private void Update()
    {
        if(GameManager.Instance.IsPlaying == true)
        {
            progress += Time.deltaTime / duration;
            if (progress >= 1f)
            {
                print("count in walker " + gameObject.name + " : " + GameManager.Instance.m_Walker.Contains(gameObject));

                GameManager.Instance.m_Walker.Remove(gameObject);
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