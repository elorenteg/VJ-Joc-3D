using UnityEngine;
using System.Collections;

public class ObjectAttraction : MonoBehaviour {

    private bool stateAttract = false;
    private Vector3 postPos;

    private float startTime, duration;
    public float moveSpeed = 5;
    
    public void Start()
    {

    }

    public void SetStateAttraction(Vector3 fiPos)
    {
        stateAttract = true;
        postPos = fiPos;
        
        duration = (postPos - transform.position).magnitude / moveSpeed;
        startTime = Time.time;
    }
    
    void Update () {
	    if (stateAttract)
        {
            float time = Time.time - startTime; // time since start
            transform.position = Vector3.Lerp(transform.position, postPos, time / duration);

            if (time/duration > 0.9f) Destroy(this);
        }
    }
}
