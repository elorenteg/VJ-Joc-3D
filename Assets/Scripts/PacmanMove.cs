using UnityEngine;
using System.Collections;

public class PacmanMove : MonoBehaviour
{
    public float moveSpeed;
    public float turnSpeed;

    private int state;
    private int timeState;
    private int MAX_TIME_STATE;

    public Texture tex;

    // Use this for initialization
    void Start () {
        state = 0;
        timeState = 0;
        MAX_TIME_STATE = 70;
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            GetComponent<Animation>().Play("EatLeft", PlayMode.StopAll);
            transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            GetComponent<Animation>().Play("EatRight", PlayMode.StopAll);
            transform.Translate(-Vector3.left * moveSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
        {
            GetComponent<Animation>().Play("EatUp", PlayMode.StopAll);
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
        {
            GetComponent<Animation>().Play("EatDown", PlayMode.StopAll);
            transform.Translate(-Vector3.forward * moveSpeed * Time.deltaTime);
        }

        //UpdateEyes();
    }

    void UpdateEyes()
    {
        if (timeState == MAX_TIME_STATE)
        {
            timeState = 0;
            state = (state++) % 2;

            //Renderer renderer = GetComponent<Renderer>();
            //renderer.material.shader = Shader.Find("pacman_eyes");
        }
        ++timeState;
    }
}
