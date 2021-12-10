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
	float angleBetween = 0.0f;
	Vector3 target;
	bool debug_printed = false;

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
				GameManager.Instance.m_Walker.Remove(gameObject);
				Destroy(gameObject);
			}
			else
			{
				countdown = countdown - Time.deltaTime;
				transform.Translate(0, 0, m_Speed * Time.deltaTime);
				//m_Rigidbody.velocity = transform.forward * m_Speed;
				//transform.position = transform.position + Vector3.Lerp(transform.position,transform.forward,m_Speed);
				//Vector3 targetDir = target - transform.position;
				//angleBetween = Vector3.Angle(transform.forward, targetDir);
				//if (debug_printed == false) { Debug.Log(target); Debug.Log(angleBetween); debug_printed = true; }
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
        if(collision.gameObject.tag == "Walker")
        {
			this.gameObject.GetComponent<SplineWalker>().enabled = true;
			this.gameObject.GetComponent<SplineWalker>().progress = collision.gameObject.GetComponent<SplineWalker>().progress + 0.001f;
			this.gameObject.GetComponent<MoveForward>().enabled = false;
		}
    }
}
