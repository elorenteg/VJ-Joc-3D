using UnityEngine;
using System.Collections;

public class PacmanMove : MonoBehaviour
{
    public float moveSpeed;
    public float turnSpeed;

    private int state;
    private int timeState;
    private int MAX_TIME_STATE;

    private SkinnedMeshRenderer skinnedMeshRenderer;
    private int BODY = 1;
    private int EYES = 0;

    public Texture eyesNormalTex;

    // Use this for initialization
    void Start ()
    {
        state = 0;
        timeState = 0;
        MAX_TIME_STATE = 15;

        skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        UpdateTextures();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(Mathf.Abs(transform.rotation.eulerAngles.y - 180));
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            GetComponent<Animation>().Play("Move", PlayMode.StopAll);
            while (Mathf.Abs(transform.rotation.eulerAngles.y - 180) > 3.0f)
            {
                float prevAngle = transform.rotation.eulerAngles.y;
                if (prevAngle > 0.0f && prevAngle < 180.0f)
                    transform.RotateAround(skinnedMeshRenderer.bounds.center, new Vector3(0, -1, 0), turnSpeed * Time.deltaTime);
                else
                    transform.RotateAround(skinnedMeshRenderer.bounds.center, new Vector3(0, 1, 0), turnSpeed * Time.deltaTime);
                float newAngle = transform.rotation.eulerAngles.y;
            }
            transform.Translate(-Vector3.left * moveSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.D))
        {
            GetComponent<Animation>().Play("Move", PlayMode.StopAll);
            while (Mathf.Abs(transform.rotation.eulerAngles.y - 180) > 3.0f)
            {
                float prevAngle = transform.rotation.eulerAngles.y;
                if (prevAngle < 180.0f)
                    transform.RotateAround(skinnedMeshRenderer.bounds.center, new Vector3(0, 1, 0), turnSpeed * Time.deltaTime);
                else
                    transform.RotateAround(skinnedMeshRenderer.bounds.center, new Vector3(0, -1, 0), turnSpeed * Time.deltaTime);
                float newAngle = transform.rotation.eulerAngles.y;
            }
            transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
        {
            //GetComponent<Animation>().Play("EatUp", PlayMode.StopAll);
        }
        if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
        {
            //GetComponent<Animation>().Play("EatDown", PlayMode.StopAll);
        }

        if (timeState == MAX_TIME_STATE)
        {
            timeState = 0;
            state = state + 1;
            if (state == 2) state = 0;
        }

        ++timeState;

        UpdateCoordinates();
    }

    void UpdateTextures()
    {
        skinnedMeshRenderer.materials[EYES].mainTexture = eyesNormalTex;

        UpdateCoordinates();
    }

    void UpdateCoordinates()
    {
        if (state == 0)
        {
            Vector2 offset = new Vector2(0.0f, 0.0f);
            skinnedMeshRenderer.materials[EYES].mainTextureOffset = offset;
        }
        else
        {
            Vector2 offset = new Vector2(0.5f, 0.0f);
            skinnedMeshRenderer.materials[EYES].mainTextureOffset = offset;
        }
    }
}
