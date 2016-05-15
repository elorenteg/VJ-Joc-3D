using UnityEngine;
using System.Collections;

public class PacmanMove : MonoBehaviour
{
    public float moveSpeed;
    public float turnSpeed;

    private const float ERROR = 1.5f;

    public static int MAX_FRAMES_STATE = 15;
    private int textureState;
    private int frameState;

    private SkinnedMeshRenderer skinnedMeshRenderer;
    private PacmanAnimate animationScript;

    private LevelManager levelManager;

    private int TILE_SIZE = 2;
    private float PACMAN_OFFSET_X = 0.38f;
    private float PACMAN_OFFSET_Z = -6.2f;
    private float COIN_OFFSET = 0.55f;

    public float pullRadius = 5;
    public float pullForce = 4;

    // Use this for initialization
    void Start()
    {
        textureState = 0;
        frameState = 0;

        skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        animationScript = GetComponent<PacmanAnimate>();
        //animationScript.SetTextures(state);

        GameObject gameManager = GameObject.Find("GameManager");
        levelManager = gameManager.GetComponent<LevelManager>();
    }

    // Update is called once per frame
    void Update()
    {
        bool pacmanCanMove = false;

        if (!levelManager.getGamePaused())
        {
            pacmanCanMove = rotate();
            if (pacmanCanMove)
            {
                animationScript.PlaySound(animationScript.stateMove());
                animationScript.Animate(animationScript.stateMove());

                transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
            }
        }

        if (frameState == MAX_FRAMES_STATE)
        {
            frameState = 0;
            textureState = (textureState + 1) % 2;

            animationScript.SetTextures(textureState);
        }

        ++frameState;
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

        bool canMove = false;
        bool rotate = false;
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            if (prevAngle >= 180.0f) rotateLeft = false;

            if ((rotateLeft && leftAngle < 360.0f && leftAngle > 180.0f) || (!rotateLeft && rightAngle > 0.0f && rightAngle < 180.0f))
            {
                fixAngle = true;
                fixedAngle = 0.0f;
            }

            if (prevAngle <= 0.0f + ERROR)
            {
                canMove = true;
                rotate = true;
                fixAngle = true;
                fixedAngle = 0.0f;
            }
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
        GetComponent<Rigidbody>().freezeRotation = true;
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

        if (collision.gameObject.tag == "ghost" ||
            collision.gameObject.tag == LevelCreator.TAG_GHOST_BLUE ||
            collision.gameObject.tag == LevelCreator.TAG_GHOST_ORANGE ||
            collision.gameObject.tag == LevelCreator.TAG_GHOST_PINK ||
            collision.gameObject.tag == LevelCreator.TAG_GHOST_RED)
        {
            Debug.Log("PacMan has collisioned with " + collision.gameObject.tag);

            if (levelManager.isBonusPacmanKillsGhost())
            {
                levelManager.ghostEaten(collision.gameObject.tag);
            }
            else
            {
                // Pacman should be killed
            }
        }
    }

    void OnTriggerEnter(Collider collider) {
        if (collider.gameObject.tag == "coin")
        {
            Debug.Log("PacMan has eaten a COIN");
            collider.enabled = false;

            ObjectAttraction attractScript = collider.gameObject.GetComponent<ObjectAttraction>();
            attractScript.SetStateAttraction(skinnedMeshRenderer.bounds.center, 10.0f);
            levelManager.coinEaten();
        }
        else if (collider.gameObject.tag == "bonus")
        {
            Debug.Log("PacMan has eaten a BONUS");

            ObjectAttraction attractScript = collider.gameObject.GetComponent<ObjectAttraction>();
            //attractScript.SetStateAttraction(skinnedMeshRenderer.bounds.center, 10.0f);

            Destroy(collider.gameObject);
            levelManager.bonusEaten();
        }
    }
}

