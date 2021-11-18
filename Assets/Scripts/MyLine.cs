using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyLine : MonoBehaviour
{

    public GameObject m_Start, m_End;
    private Vector3 p0, p1;

    // Start is called before the first frame update
    void Start()
    {
        p0 = m_Start.transform.position;
        p1 = m_End.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
