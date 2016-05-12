using UnityEngine;
using System.Collections;

public class PacmanMove : MonoBehaviour
{
    public float moveSpeed;
    public float turnSpeed;

    private const float ERROR = 1.5f;

    private int state;
    private int timeState;
    private int MAX_TIME_STATE;

    private SkinnedMeshRenderer skinnedMeshRenderer;
    private PacmanAnimate animationScript;

    private LevelManager levelManager;

    private const int LEFT = 0;
    private const int RIGHT = 1;
    private const int UP = 2;
    private const int DOWN = 3;

    // Use this for initialization
    void Start()
    {
        state = 0;
        timeState = 0;
        MAX_TIME_STATE = 15;

        skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        animationScript = GetComponent<PacmanAnimate>();
        //animationScript.SetTextures(state);

        GameObject gameManager = GameObject.Find("GameManager");
        levelManager = gameManager.GetComponent<LevelManager>();
    }

    // Update is called once per frame
    void Update()
    {
        bool pacmanCanMove = rotate();
        if (pacmanCanMove)
        {
            animationScript.PlaySound(animationScript.stateMove());
            animationScript.Animate(animationScript.stateMove());
            
            transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
        }


        if (timeState == MAX_TIME_STATE)
        {
            timeState = 0;
            state = state + 1;
            if (state == 2) state = 0;

            animationScript.SetTextures(state);
        }

        ++timeState;

        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationX
                                      | RigidbodyConstraints.FreezeRotationY
                                      | RigidbodyConstraints.FreezeRotationZ;
    }

    bool rotate()
    {
        bool rotateLeft = true;

        bool fixAngle = false;
        float fixedAngle = 0.0f;
        float incAngle = turnSpeed * Time.deltaTime;
        
        float prevAngle = transform.rotation.eulerAngles.y;
        float leftAngle = (prevAngle - incAngle) % 360;
        float rightAngle = (prevAngle + incAngle) % 360;

        //leftAngle = prevAngle - incAngle;
        //rightAngle = prevAngle + incAngle;
        //if (leftAngle < 0.0f) leftAngle = 360.0f - leftAngle;
        //if (rightAngle >= 360.0f) rightAngle = rightAngle - 360.0f;

        bool canMove = false;
        bool rotate = false;
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            if (prevAngle > 180.0f) rotateLeft = false;

            if ((rotateLeft && leftAngle < 360.0f && leftAngle > 180.0f) || (!rotateLeft && rightAngle > 0.0f && rightAngle < 180.0f))
            {
                fixAngle = true;
                fixedAngle = 0.0f;
            }

            if (prevAngle == 0.0f) canMove = true;
            else rotate = true;
        }
        else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            if (prevAngle < 180.0f) rotateLeft = false;
            
            if ((rotateLeft && leftAngle < 180.0f && leftAngle > 0.0f) || (!rotateLeft && rightAngle > 180.0f && rightAngle < 360.0f))
            {
                fixAngle = true;
                fixedAngle = 180.0f;
            }

            if (prevAngle == 180.0f) canMove = true;
            else rotate = true;
        }
        else if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
        {
            if (prevAngle > 270.0f || prevAngle < 90.0f) rotateLeft = false;
            
            if ((rotateLeft && (leftAngle < 90.0f || leftAngle > 270.0f)) || (!rotateLeft && rightAngle > 90.0f && rightAngle < 270.0f))
            {
                fixAngle = true;
                fixedAngle = 90.0f;
            }

            if (prevAngle == 90.0f) canMove = true;
            else rotate = true;
        }
        else if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
        {
            if (prevAngle > 90.0f && prevAngle < 270.0f) rotateLeft = false;
            
            if ((rotateLeft && leftAngle < 270.0f && leftAngle > 90.0f) || (!rotateLeft && (rightAngle > 270.0f || rightAngle < 90.0f)))
            {
                fixAngle = true;
                fixedAngle = 270.0f;
            }

            if (prevAngle == 270.0f) canMove = true;
            else rotate = true;
        }

        if (rotate)
        {
            if (fixAngle) fixEulerAngle(fixedAngle);
            else if (rotateLeft)
                transform.RotateAround(skinnedMeshRenderer.bounds.center, new Vector3(0, -1, 0), incAngle);
            else
                transform.RotateAround(skinnedMeshRenderer.bounds.center, new Vector3(0, 1, 0), incAngle);

            return fixAngle;
        }

        return canMove;
    }

    void fixEulerAngle(float fixedAngle)
    {
        Vector3 euler = transform.eulerAngles;
        euler.y = fixedAngle;
        transform.eulerAngles = euler;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "ghost")
        {
            Debug.Log("PacMan has collisioned with a GHOST");
            GhostMove ghostScript = collision.gameObject.GetComponent<GhostMove>();
            ghostScript.SetDead();
        }

        if (collision.gameObject.tag == "coin")
        {
            Debug.Log("PacMan has eaten a COIN");
            Destroy(collision.gameObject);

            levelManager.coinEaten();
        }

        if (collision.gameObject.tag == "bonus")
        {
            Debug.Log("PacMan has eaten a BONUS");
            Destroy(collision.gameObject);
        }
    }
}
