using UnityEngine;

public class SplineWalker : MonoBehaviour
{

    public BezierSpline spline;

    public bool isCollision = false;
    public float duration;

    private float progress;

    public bool lookForward;


    void OnCollisionEnter(Collision collision)
    {
        /*
        switch(collision.gameObject.tag)
        {
            case "cube":
                isCollision = true;
                Debug.Log("collsion cube avec"+ collision.gameObject.tag);
                break;
            case "cube1":
                isCollision = true;
                Debug.Log("collsion cube avec" + collision.gameObject.tag);
                break;
            case "cube2":
                isCollision = true;
                Debug.Log("collsion cube avec" + collision.gameObject.tag);
                break;
        }
       */
        if (collision.gameObject.name == "Cube")
        {
            isCollision = true;

            Debug.Log("collsion cube avec" + collision.gameObject.tag);
        }
        
    }

    
    private void Update()
    {
        if (isCollision == false)
        {
            progress += Time.deltaTime / duration;
                    if (progress > 1f)
                    {
                        progress = 1f;
                    }
                    Vector3 position = spline.GetPoint(progress);
                    transform.localPosition = position;
                    if (lookForward)
                    {
                        transform.LookAt(position + spline.GetDirection(progress));
                    }
        }
        isCollision = false;
        
    }
}