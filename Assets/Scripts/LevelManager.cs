using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SDD.Events;

public class LevelManager : MonoBehaviour, IEventHandler
{
    [SerializeField] Transform m_PlayerSpawnPosition;

    public void SubscribeEvents()
    {
        EventManager.Instance.AddListener<GamePlayEvent>(GamePlay);
    }

    public void UnsubscribeEvents()
    {
        EventManager.Instance.RemoveListener<GamePlayEvent>(GamePlay);
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
        Init();
    }
    #endregion

    void DestroyAllBWalker()
    {
        // destruction des balles
        GameObject[] Walker = GameObject.FindGameObjectsWithTag("Walker");
        for (int i = 0; i < Walker.Length; i++)
        {
            Destroy(Walker[i].gameObject);
        }
    }
    void Init()
    {
        DestroyAllBWalker();
        EventManager.Instance.Raise(new LevelHasBeenInitializedEvent() { ePlayerSpawnPos = m_PlayerSpawnPosition.position });
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
