using UnityEngine;
using System.Collections;

public class PacmanMove : MonoBehaviour
{
    protected static float PACMAN_SPEED = 45.0f;
    protected static float PACMAN_ROTATE_SPEED = 350.0f;

    private const float ERROR = 1.5f;

    public static int MAX_FRAMES_STATE = 15;
    private int textureState;
    private int frameState;

    private int tileX;
    private int tileZ;

    private SkinnedMeshRenderer skinnedMeshRenderer;
    private PacmanAnimate animationScript;

    private LevelManager levelManager;

    public float pullRadius = 5;
    public float pullForce = 4;

    private float currentSpeed;
    private float BATTERY_SPEED_INCREASE = 20.0f;

    // Use this for initialization
    void Start()
    {
        skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        animationScript = GetComponent<PacmanAnimate>();
        //animationScript.SetTextures(state);

        initPacman();

        GameObject gameManager = GameObject.Find("GameManager");
        levelManager = gameManager.GetComponent<LevelManager>();
    }

    public void restartPacman(Vector3 pos)
    {
        transform.position = pos;

        initPacman();
    }

    public void initPacman()
    {
        textureState = 0;
        frameState = 0;

        currentSpeed = PACMAN_SPEED;

        animationScript = GetComponent<PacmanAnimate>();
        animationScript.Start();
        animationScript.SetTextures(animationScript.stateMove());
    }

    public void SetVisible(bool visible)
    {
        GetComponent<SphereCollider>().enabled = visible;
        GetComponentInChildren<SkinnedMeshRenderer>().GetComponent<Renderer>().enabled = visible;

        animationScript.StopSound();
    }

    public void SetInitTiles(int tx, int tz)
    {
        tileX = tx;
        tileZ = tz;
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

                    transform.Translate(Vector3.left * currentSpeed * Time.deltaTime);
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

            currentSpeed = Mathf.Max(currentSpeed - 0.5f, PACMAN_SPEED);
        }

        ++frameState;
    }

    public bool doRotation(int dir)
    {
        bool rotateLeft = true;

        bool fixAngle = false;
        float fixedAngle = 0.0f;
        float incAngle = PACMAN_ROTATE_SPEED * Time.deltaTime;

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

    public void fixEulerAngle(float fixedAngle)
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
            GhostMove ghostMove = collider.gameObject.GetComponent<GhostMove>();
            if (levelManager.isBonusPacmanKillsGhost() && !ghostMove.ghostIsDead())
            {
                levelManager.ghostEaten(collider.gameObject.tag, ghostMove.boundsPosition());
            }
            else
            {
                if (!ghostMove.ghostIsDead())
                {
                    animationScript.StopSound();
                    animationScript.PlaySound(animationScript.stateDead());
                    animationScript.Animate(animationScript.stateDead());

                    levelManager.decreaseLifes();
                }
            }
        }
        if (collider.gameObject.tag == Globals.TAG_COIN)
        {
            collider.enabled = false;

            ObjectAnimate attractScript = collider.gameObject.GetComponent<ObjectAnimate>();
            attractScript.SetStateAttraction(skinnedMeshRenderer.bounds.center, 10.0f);
            attractScript.PlaySound();
            levelManager.coinEaten();
        }
        else if (collider.gameObject.tag == Globals.TAG_BONUS)
        {
            ObjectAnimate attractScript = collider.gameObject.GetComponent<ObjectAnimate>();
            attractScript.SetStateAttraction(skinnedMeshRenderer.bounds.center, 10.0f);
            attractScript.PlaySound();

            //Destroy(collider.gameObject);
            levelManager.bonusEaten();
        }
        else if (collider.gameObject.tag == Globals.TAG_CHERRY)
        {
            ObjectAnimate attractScript = collider.gameObject.GetComponent<ObjectAnimate>();
            attractScript.SetStateAttraction(skinnedMeshRenderer.bounds.center, 10.0f);

            //Destroy(collider.gameObject);
            levelManager.cherryEaten(collider.transform.position);
        }
        else if (collider.gameObject.tag == Globals.TAG_BATTERY)
        {
            Debug.Log("BATTERY");
            ObjectAnimate attractScript = collider.gameObject.GetComponent<ObjectAnimate>();
            attractScript.SetStateAttraction(skinnedMeshRenderer.bounds.center, 10.0f);
            attractScript.PlaySound();

            currentSpeed = currentSpeed + BATTERY_SPEED_INCREASE;
        }
    }

    public void ActualTile(out int tx, out int tz)
    {
        LevelCreator.pacmanPositionToTile(skinnedMeshRenderer.bounds.center, out tx, out tz);
    }

    public int ActualSection()
    {
        int tx, tz;
        ActualTile(out tx, out tz);

        return LevelCreator.SectionTile(tx, tz);
    }
}

