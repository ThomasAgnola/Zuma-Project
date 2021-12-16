using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SDD.Events;

public class SpawnPos : MonoBehaviour
{

    public bool isFree = true;

    // Start is called before the first frame update
    void Start()
    {
        isFree = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionExit(Collision collision)
    {
        isFree = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        isFree = false;
    }

    private void OnCollisionStay(Collision collision)
    {
        isFree = false;
    }
}
