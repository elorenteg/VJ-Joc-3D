using UnityEngine;
using UnityEngine.Rendering;
using System.Collections;

public class GhostMove : MonoBehaviour
{
    protected static float GHOST_SPEED = 1.0f;
    protected static float GHOST_ROTATE_SPEED = 3.5f;
    
    protected static float UP_ANGLE = 90.0f;
    protected static float DOWN_ANGLE = 270.0f;
    protected static float LEFT_ANGLE = 0.0f;
    protected static float RIGHT_ANGLE = 180.0f;

    public static int MAX_FRAMES_STATE = 15;
    private int textureState;
    private int frameState;

    private bool isDead;
    private bool canBeKilled;

    private int[] currentPath;
    private int currentDir;
    private bool currentDirCalculated;

    protected int ghostState;
    protected const int WANDERING_BASE = 0;
    protected const int LEAVING_BASE = 1;
    protected const int CHASING_PACMAN = 2;
    protected const int EVADING_PACMAN = 3;
    protected const int RETURNING_BASE = 4;

    protected int tileX, tileZ;
    protected int newTileX, newTileZ;
    protected float startTime, duration;
    protected bool isMoving;
    protected Vector3 newPosition;

    private GhostAnimate animationScript;

    protected LevelManager levelManager;

    private bool first = true;

    // Use this for initialization
    public void Start()
    {
        //isDead = false;
        //canBeKilled = false;
        textureState = 0;
        frameState = 0;

        ghostState = LEAVING_BASE;
        currentDirCalculated = false;

        animationScript = GetComponent<GhostAnimate>();
        animationScript.SetTextures(animationScript.stateMove(), textureState);

        GameObject gameManager = GameObject.Find("GameManager");
        levelManager = gameManager.GetComponent<LevelManager>();
    }

    // Update is called once per frame
    public void Update()
    {
        if (frameState == MAX_FRAMES_STATE)
        {
            frameState = 0;
            textureState = (textureState + 1) % 2;

            if (isDead) animationScript.SetTextures(animationScript.stateDead(), textureState);
            else if (canBeKilled) animationScript.SetTextures(animationScript.stateKilleable(), textureState);
            else animationScript.SetTextures(animationScript.stateMove(), textureState); 
        }

        ++frameState;
    }

    public void SetKilleable(bool killeable)
    {
        canBeKilled = killeable;

        if (canBeKilled)
            animationScript.SetTextures(animationScript.stateKilleable(), textureState);

        // Mover alejandose
        // canBeKilled = false;
    }

    public void SetDead(bool dead)
    {
        isDead = dead;

        if (isDead)
            animationScript.SetTextures(animationScript.stateDead(), textureState);

        // Mover a base
        // isDead = false;
    }

    protected float getBaseGhostSpeed()
    {
        return GHOST_SPEED;
    }

    protected float getBaseGhostRotateSpeed()
    {
        return GHOST_ROTATE_SPEED;
    }

    public void SetInitTiles(int tx, int tz)
    {
        tileX = tx;
        tileZ = tz;
    }

    protected Vector3 GetPosition(int tx, int tz)
    {
        return new Vector3(tx * LevelCreator.TILE_SIZE, transform.position.y, tz * LevelCreator.TILE_SIZE);
    }

    public static bool isValid(int[][] Map, int tx, int tz)
    {
        for (int i = tx - 1; i <= tx + 1; ++i)
        {
            for (int j = tz - 1; j <= tz + 1; ++j)
            {
                if (!LevelCreator.isValidTile(i, j))
                {
                    //Debug.Log("Size");
                    return false;
                }
                else if (LevelCreator.isWall(i, j))
                {
                    //Debug.Log("Is a Wall");
                    //Debug.Log("IS A WALL -- " + "(" + i + "," + j + ") " + Map[j][i]);
                    return false;
                }
            }
        }
        return true;
    }

    protected bool doRotation(int dir)
    {
        float angleRotation = 180.0f;
        float comparisonAngle = 180.0f;
        float eulerAngle = transform.rotation.eulerAngles.y;
        switch (dir)
        {
            case Globals.UP:
                angleRotation = UP_ANGLE;
                comparisonAngle = angleRotation;
                break;
            case Globals.RIGHT:
                angleRotation = RIGHT_ANGLE;
                comparisonAngle = angleRotation;
                break;
            case Globals.DOWN:
                angleRotation = DOWN_ANGLE;
                comparisonAngle = angleRotation;
                break;
            case Globals.LEFT:
                angleRotation = LEFT_ANGLE;
                if (eulerAngle <= 180.0f) comparisonAngle = 0.0f;
                else comparisonAngle = 360.0f;
                break;
        }

        float diff = eulerAngle - comparisonAngle;
        if (Mathf.Abs(diff) <= 2.0f)
        {
            //Debug.Log("Fixing angle");

            Vector3 eulerAngles = transform.eulerAngles;
            eulerAngles.y = angleRotation;
            transform.eulerAngles = eulerAngles;

            return true;
        }
        else
        {
            //Debug.Log("Rotating");

            Quaternion newRotation = Quaternion.AngleAxis(angleRotation, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, Time.deltaTime * GHOST_ROTATE_SPEED);

            return false;
        }
    }

    public void onMove(int[][] Map)
    {
        if (ghostState == WANDERING_BASE)
        {
            wanderingBase(Map);
        }
        else if (ghostState == LEAVING_BASE)
        {
            if (!currentDirCalculated)
            {
                newTileX = tileX;
                newTileZ = tileZ + 6;
                currentPath = BFS.calculatePath(Map, tileX, tileZ, newTileX, newTileZ);
                currentDirCalculated = true;

                isMoving = false;
                SetDirection(currentPath[currentDir]);

                Debug.Log(tileX + " " + tileZ);
            }

            leavingBase();
        }
        else if (ghostState == CHASING_PACMAN)
        {
            chasingPacman(Map);
        }
        else if (ghostState == EVADING_PACMAN)
        {

        }
        else if (ghostState == RETURNING_BASE) { }
        else Debug.Log("State error");
    }

    public virtual void wanderingBase(int[][] Map)
    {
        //Debug.Log("Wandering father");
    }

    public virtual void leavingBase()
    {
        if (isMoving && transform.position == newPosition)
        {
            isMoving = false;
            tileX = newTileX;
            tileZ = newTileZ;
            ++currentDir;

            Debug.Log(tileX + " " + tileZ);

            if (currentDir == currentPath.Length) ghostState = WANDERING_BASE;
            else SetDirection(currentPath[currentDir]);
        }

        if (isMoving)
        {
            float distCovered = (Time.time - startTime) * GHOST_SPEED;
            float fracJourney = distCovered / duration;
            transform.position = Vector3.MoveTowards(transform.position, newPosition, fracJourney);
        }
        else
        {
            bool ghostCanMove = doRotation(currentPath[currentDir]);

            if (ghostCanMove)
            {
                newPosition = LevelCreator.TileToPosition(newTileX, newTileZ, transform.position.y);
                startTime = Time.time;
                duration = Vector3.Distance(transform.position, newPosition);
                isMoving = true;
            }
        }
    }

    public virtual void chasingPacman(int[][] Map)
    {
        Debug.Log("Chasing father");
    }

    public void SetDirection(int dir)
    {
        newTileX = tileX;
        newTileZ = tileZ;

        if (dir == Globals.UP) newTileZ = newTileZ + 1;
        else if (dir == Globals.RIGHT) newTileX = newTileX + 1;
        else if (dir == Globals.DOWN) newTileZ = newTileZ - 1;
        else if (dir == Globals.LEFT) newTileX = newTileX - 1;
    }
}
