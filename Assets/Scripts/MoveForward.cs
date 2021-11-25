using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveForward : MonoBehaviour
{
    
    Rigidbody m_Rigidbody;
    float m_Speed;
    public float angleBetween = 0.0f;
    Vector3 target;
    float countdown = 10;
    bool debug_printed = false;

    // Start is called before the first frame update
    void Start()
    {
        //Fetch the Rigidbody component you attach from your GameObject
        target = Camera.main.ScreenToWorldPoint(new Vector3(
                                                            Input.mousePosition.x,
                                                            Input.mousePosition.y,
                                                            0.5f)); //gameObject.transform.position.y
        m_Rigidbody = GetComponent<Rigidbody>();
        //Set the speed of the GameObject
        m_Speed = 10.0f;
    }

    void FixedUpdate()
    {
        //Move the Rigidbody forwards constantly at speed you define (the blue arrow axis in Scene view)
        if(countdown <= 0)
        {
            GameManager.Instance.m_Walker.Remove(gameObject);
            Destroy(gameObject);
        }
        else
        {
            countdown = countdown - Time.deltaTime;
            Vector3 targetDir = target - transform.position;
            angleBetween = Vector3.Angle(transform.forward, targetDir);
            if (debug_printed == false) { Debug.Log(target); Debug.Log(angleBetween); debug_printed = true; }
            /*gameObject.transform.Translate(new Vector3(
                                                            Input.mousePosition.x,
                                                            Input.mousePosition.z,
                                                            gameObject.transform.position.y));*/
            transform.position += new Vector3(Mathf.Cos(angleBetween), 0, Mathf.Sin(angleBetween)) * 3f * Time.deltaTime;
            //m_Rigidbody.velocity = transform.forward * m_Speed;
        }
        
    }
}
