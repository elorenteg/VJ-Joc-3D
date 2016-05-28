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
        int keyPressed = -1;
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) keyPressed = Globals.LEFT;
        else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) keyPressed = Globals.RIGHT;
        else if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)) keyPressed = Globals.UP;
        else if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S)) keyPressed = Globals.DOWN;

        if (keyPressed != -1)
        {
            if (!levelManager.getGamePaused())
            {
                bool pacmanCanMove = doRotation(keyPressed);
                if (pacmanCanMove)
                {
                    animationScript.PlaySound(animationScript.stateMove());
                    animationScript.Animate(animationScript.stateMove());

                    transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
                }
            }

            int tx, tz;
            LevelCreator.positionToTile(skinnedMeshRenderer.bounds.center, out tx, out tz);
        }

        if (frameState == MAX_FRAMES_STATE)
        {
            frameState = 0;
            textureState = (textureState + 1) % 2;

            animationScript.SetTextures(textureState);
        }

        ++frameState;
    }

    public bool doRotation(int dir)
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
        if (dir == Globals.LEFT)
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
        else if (dir == Globals.RIGHT)
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
        else if (dir == Globals.UP)
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
        else if (dir == Globals.DOWN)
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
            else {
                SkinnedMeshRenderer skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();

                if (rotateLeft)
                    transform.RotateAround(skinnedMeshRenderer.bounds.center, new Vector3(0, -1, 0), incAngle);
                else
                    transform.RotateAround(skinnedMeshRenderer.bounds.center, new Vector3(0, 1, 0), incAngle);
            }

            return fixAngle;
        }

        return canMove;
    }

    private void fixEulerAngle(float fixedAngle)
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
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == Globals.TAG_GHOST ||
            collider.gameObject.tag == Globals.TAG_GHOST_BLUE ||
            collider.gameObject.tag == Globals.TAG_GHOST_ORANGE ||
            collider.gameObject.tag == Globals.TAG_GHOST_PINK ||
            collider.gameObject.tag == Globals.TAG_GHOST_RED)
        {
            Debug.Log("PacMan has collisioned with " + collider.gameObject.tag);

            if (levelManager.isBonusPacmanKillsGhost())
            {
                levelManager.ghostEaten(collider.gameObject.tag);
            }
            else
            {
                animationScript.PlaySound(animationScript.stateDead());
                animationScript.Animate(animationScript.stateDead());
            }
        }
        if (collider.gameObject.tag == Globals.TAG_COIN)
        {
            //Debug.Log("PacMan has eaten a COIN");
            collider.enabled = false;

            ObjectAttraction attractScript = collider.gameObject.GetComponent<ObjectAttraction>();
            attractScript.SetStateAttraction(skinnedMeshRenderer.bounds.center, 10.0f);
            levelManager.coinEaten();
        }
        else if (collider.gameObject.tag == Globals.TAG_BONUS)
        {
            //Debug.Log("PacMan has eaten a BONUS");

            ObjectAttraction attractScript = collider.gameObject.GetComponent<ObjectAttraction>();
            //attractScript.SetStateAttraction(skinnedMeshRenderer.bounds.center, 10.0f);

            Destroy(collider.gameObject);
            levelManager.bonusEaten();
        }
    }
}

