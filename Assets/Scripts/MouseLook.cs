using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SDD.Events;

public class MouseLook : MonoBehaviour
{

	public GameObject prefab;
	public Material[] material_color = new Material[3];
	int color_index, next_color_index;
	string color, next_color;


	public void SubscribeEvents()
	{
		EventManager.Instance.AddListener<GamePlayEvent>(GamePlay);
		EventManager.Instance.AddListener<BallChangedEvent>(NextBallChanged);
		EventManager.Instance.AddListener<LevelHasBeenInitializedEvent>(LevelHasBeenInitialized);
	}

	public void UnsubscribeEvents()
	{
		EventManager.Instance.RemoveListener<GamePlayEvent>(GamePlay);
		EventManager.Instance.RemoveListener<BallChangedEvent>(NextBallChanged);
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

	void NextBallChanged(BallChangedEvent e)
    {

    }
	#endregion

	// Start is called before the first frame update
	void Start()
	{
		color_index = (int)Random.Range(0, 3);
		color = "Red"; 
		if (color_index == 1) color = "Green";
		if (color_index == 2) color = "Blue";
		EventManager.Instance.Raise(new BallChangedEvent() { eColor = color });
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
				next_color_index = (int)Random.Range(0, 3);
				next_color = "Red";
				int index = GameManager.Instance.launch_count++;
				GameObject clone = Instantiate(prefab, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z), Quaternion.Euler(new Vector3(0, transform.rotation.eulerAngles.y, 0)));
				Debug.Log("roation y de Frog : " + transform.rotation.eulerAngles.y);
				clone.name = "Walker" + index;
				if (next_color_index == 1) next_color = "Green";
				if (next_color_index == 2) next_color = "Blue";
				Debug.Log("color : " + color);
				clone.GetComponent<Renderer>().material = material_color[color_index];
				clone.GetComponent<SplineWalker>().color = color;
				clone.GetComponent<SplineWalker>().index = index;
				clone.GetComponent<SplineWalker>().enabled = false;
				clone.GetComponent<MoveForward>().color = color;
				clone.GetComponent<MoveForward>().index = index;
				clone.GetComponent<MoveForward>().enabled = true;
				//Destroy(clone.GetComponent<SplineWalker>());
				//clone.AddComponent<MoveForward>();
				color = next_color;
				color_index = next_color_index;
				EventManager.Instance.Raise(new BallChangedEvent() { eColor = color });

				GameManager.Instance.launched_Walker.Add(new Balls(color, index, clone));
				Debug.Log("Color : " + GameManager.Instance.launched_Walker[index].color + " for index : " + color_index);
				clone.SetActive(true);
			}
			catch
			{

			}
			
		}
	}
}
