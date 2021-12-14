using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SDD.Events;

public class MoveForward : MonoBehaviour
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


	Rigidbody m_Rigidbody;
	public float m_Speed = 2f;
	public float countdown = 10;
	public string color;
	public int index;
	float angleBetween = 0.0f;
	Vector3 target;

	// Start is called before the first frame update
	void Start()
	{
		//Fetch the Rigidbody component you attach from your GameObject
		/*target = Camera.main.ScreenToWorldPoint(new Vector3(
															Input.mousePosition.x,
															Input.mousePosition.y,
															0.5f));*/ //gameObject.transform.position.y
		m_Rigidbody = GetComponent<Rigidbody>();
		//Set the speed of the GameObject
	}

	void FixedUpdate()
	{
		if (GameManager.Instance.IsPlaying)
		{
			if (countdown <= 0)
			{
				GameManager.Instance.m_Walker.Remove(new Balls(color, index, gameObject));
				Destroy(gameObject);
			}
			else
			{
				countdown = countdown - Time.deltaTime;
				transform.Translate(0, 0, m_Speed * 4 * Time.deltaTime);
				//m_Rigidbody.velocity = transform.forward * m_Speed;
				//transform.position = transform.position + Vector3.Lerp(transform.position,transform.forward,m_Speed);
				//Vector3 targetDir = target - transform.position;
				//angleBetween = Vector3.Angle(transform.forward, targetDir);
				/*gameObject.transform.Translate(new Vector3(
																Input.mousePosition.x,
																Input.mousePosition.z,
																gameObject.transform.position.y));*/
				//transform.position += new Vector3(Mathf.Cos(angleBetween), 0, Mathf.Sin(angleBetween)) * 3f * Time.deltaTime;
				//m_Rigidbody.velocity = transform.forward * m_Speed;
			}
		}		
	}

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Walker" && this.isActiveAndEnabled)
        {
			//int col_index = collision.gameObject.GetComponent<SplineWalker>().index;
			int col_index = 0;
			for(int i = 1; i < (GameManager.Instance.m_Walker.Count - 1); i++)
            {
				if(GameManager.Instance.m_Walker[i].go.name == collision.gameObject.name)
				{
					col_index = i;
					break;
				}
				
            }
			//int col_index = GameManager.Instance.m_Walker.IndexOf(new Balls( collision.gameObject.GetComponent<SplineWalker>().color, collision.gameObject.GetComponent<SplineWalker>().index, collision.gameObject));
			Debug.Log("Collision color : " + collision.gameObject.GetComponent<SplineWalker>().color + " index : " + collision.gameObject.GetComponent<SplineWalker>().index);
			Debug.Log("index of collision go : " + col_index);
			float between_value = 0.001f;
			float collision_progess = collision.gameObject.GetComponent<SplineWalker>().progress;
			if ((1 - GameManager.Instance.m_Walker[col_index - 1].go.GetComponent<SplineWalker>().progress) > (1 - GameManager.Instance.m_Walker[col_index + 1].go.GetComponent<SplineWalker>().progress))
			{
				between_value = -0.001f;
				Debug.Log("Inserted behind collision");
			}
            else
            {
				Debug.Log("Inserted ahead of collision");
            }
			if (between_value < 0f)
			{
				GameManager.Instance.m_Walker.Insert(col_index - 1, new Balls(color, index, this.gameObject));
				for (int i = 0; i < GameManager.Instance.launched_Walker.Count; i++)
				{
					if (GameManager.Instance.launched_Walker[i].go.name == this.gameObject.name)
					{
						GameManager.Instance.launched_Walker.RemoveAt(i);
						break;
					}
				}
				
			}
			if (between_value > 0f)
			{
				GameManager.Instance.m_Walker.Insert(col_index + 1, new Balls(color, index, this.gameObject));
				for (int i = 0; i < GameManager.Instance.launched_Walker.Count; i++)
				{
					if (GameManager.Instance.launched_Walker[i].go.name == this.gameObject.name)
					{
						GameManager.Instance.launched_Walker.RemoveAt(i);
						break;
					}
				}
			}
			this.gameObject.GetComponent<SplineWalker>().progress = collision_progess + between_value;
			//Debug.Log("collision progress : " + collision_progess + ", progress of go collided : " + this.gameObject.GetComponent<SplineWalker>().progress);
			this.gameObject.GetComponent<SplineWalker>().enabled = true;
			this.gameObject.GetComponent<MoveForward>().enabled = false;
		}
    }
}
