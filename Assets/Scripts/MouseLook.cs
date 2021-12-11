using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SDD.Events;

public class MouseLook : MonoBehaviour
{

	public GameObject prefab;
  
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

	// Start is called before the first frame update
	void Start()
	{
		
	}

	// Update is called once per frame
	void Update()
	{
		Vector3 mouse = Input.mousePosition;
		Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(new Vector3(
															mouse.x,
															mouse.y,
															gameObject.transform.position.y));
		Vector3 forward = mouseWorld - gameObject.transform.position;
		gameObject.transform.rotation = Quaternion.LookRotation(forward, Vector3.up);
		if (Input.GetMouseButtonDown(0) && GameManager.Instance.IsPlaying)
		{
			try
			{
				int color_index = (int)Random.Range(1, 3);
				string color = "red";
				int index = GameManager.Instance.count++;
				GameObject clone = Instantiate(prefab, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z), Quaternion.Euler(new Vector3(0, transform.rotation.eulerAngles.y, 0)));
				Debug.Log("roation y de Frog : " + transform.rotation.eulerAngles.y);
				clone.name = "Walker" + index;
				if (color_index == 2) color = "green";
				if (color_index == 3) color = "blue";
				clone.GetComponent<SplineWalker>().color = color;
				clone.GetComponent<SplineWalker>().index = index;
				clone.GetComponent<SplineWalker>().enabled = false;
				clone.GetComponent<MoveForward>().color = color;
				clone.GetComponent<MoveForward>().index = index;
				clone.GetComponent<MoveForward>().enabled = true;
				//Destroy(clone.GetComponent<SplineWalker>());
				//clone.AddComponent<MoveForward>();
				
				GameManager.Instance.m_Walker.Add(new Balls(color, index, clone));
				Debug.Log("Color : " + GameManager.Instance.m_Walker[index].color);
				clone.SetActive(true);
			}
			catch
			{

			}
			
		}
	}
}
