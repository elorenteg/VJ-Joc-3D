using UnityEngine;
using System.Collections;

public class PacmanMove : MonoBehaviour
{
    public float moveSpeed;
    public float turnSpeed;

    private AudioSource audioSource;
    public AudioClip moveSound;

    private const float ERROR = 1.5f;

    private int state;
    private int timeState;
    private int MAX_TIME_STATE;

    private SkinnedMeshRenderer skinnedMeshRenderer;
    private const int BODY = 1;
    private const int EYES = 0;

    public Texture eyesNormalTex;

    // Use this for initialization
    void Start ()
    {
        state = 0;
        timeState = 0;
        MAX_TIME_STATE = 15;

        audioSource = GetComponent<AudioSource>();
        skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        UpdateTextures();
    }

    // Update is called once per frame
    void Update()
    {
        bool move = false;
        float fixedAngle = 0.0f;

        float prevAngle = transform.rotation.eulerAngles.y;
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            if ((prevAngle > 0.0f && prevAngle < 180.0f && Mathf.Abs(prevAngle - 0) > ERROR) ||
                (prevAngle > 180.0f && prevAngle < 360.0f && Mathf.Abs(prevAngle - 360) > ERROR))
            {
                
                if (prevAngle < 180.0f)
                    transform.RotateAround(skinnedMeshRenderer.bounds.center, new Vector3(0, -1, 0), turnSpeed * Time.deltaTime);
                else
                    transform.RotateAround(skinnedMeshRenderer.bounds.center, new Vector3(0, 1, 0), turnSpeed * Time.deltaTime);
            }
            else {
                move = true;
                fixedAngle = 0.0f;
            }
        }
        else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            if (Mathf.Abs(prevAngle - 180) > ERROR)
            {
                if (prevAngle < 180.0f)
                    transform.RotateAround(skinnedMeshRenderer.bounds.center, new Vector3(0, 1, 0), turnSpeed * Time.deltaTime);
                else
                    transform.RotateAround(skinnedMeshRenderer.bounds.center, new Vector3(0, -1, 0), turnSpeed * Time.deltaTime);
            }
            else
            {
                move = true;
                fixedAngle = 180.0f;
            }
        }
        else if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
        {
            if (Mathf.Abs(prevAngle - 90) > ERROR)
            {
                if (prevAngle > 90.0f && prevAngle < 270.0f)
                    transform.RotateAround(skinnedMeshRenderer.bounds.center, new Vector3(0, -1, 0), turnSpeed * Time.deltaTime);
                else
                    transform.RotateAround(skinnedMeshRenderer.bounds.center, new Vector3(0, 1, 0), turnSpeed * Time.deltaTime);
            }
            else
            {
                move = true;
                fixedAngle = 90.0f;
            }
        }
        else if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
        {
            if (Mathf.Abs(prevAngle - 270) > ERROR)
            {
                if (prevAngle > 90.0f && prevAngle < 270.0f)
                    transform.RotateAround(skinnedMeshRenderer.bounds.center, new Vector3(0, 1, 0), turnSpeed * Time.deltaTime);
                else
                    transform.RotateAround(skinnedMeshRenderer.bounds.center, new Vector3(0, -1, 0), turnSpeed * Time.deltaTime);
            }
            else
            {
                move = true;
                fixedAngle = 270.0f;
            }
        }

        if (move)
        {
            if (!audioSource.isPlaying)
            {
                audioSource.volume = 0.5f;
                audioSource.clip = moveSound;
                audioSource.Play();
            }
            //AudioSource.PlayClipAtPoint(moveSound, transform.position);
            GetComponent<Animation>().Play("Move", PlayMode.StopAll);

            Vector3 euler = transform.eulerAngles;
            euler.y = fixedAngle;
            transform.eulerAngles = euler;
            transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
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
