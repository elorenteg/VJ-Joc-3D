using UnityEngine;
using UnityEngine.Rendering;
using System.Collections;

public class GhostMove : MonoBehaviour
{
    protected static float GHOST_SPEED = 20;
    protected static float GHOST_ROTATE_SPEED = 2.5f;

    protected static float UP_ANGLE = 90.0f;
    protected static float DOWN_ANGLE = 270.0f;
    protected static float LEFT_ANGLE = 0.0f;
    protected static float RIGHT_ANGLE = 180.0f;

    public static int MAX_FRAMES_STATE = 15;
    private int textureState;
    private int frameState;

    private bool isDead;
    private bool canBeKilled;

    protected int tileX, tileZ;
    protected int newTileX, newTileZ;

    private GhostAnimate animationScript;

    protected LevelManager levelManager;

    // Use this for initialization
    public void Start()
    {
        isDead = false;
        canBeKilled = false;
        textureState = 0;
        frameState = 0;

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
        return new Vector3(tx * Globals.TILE_SIZE, transform.position.y, tz * Globals.TILE_SIZE);
    }

    protected bool isValid(int[][] Map, int tx, int tz)
    {
        for (int i = tx - 0; i <= tx + 0; ++i)
        {
            for (int j = tz - 0; j <= tz + 0; ++j)
            {
                if (i < 0 || j < 0 || i == Globals.MAP_HEIGHT || j == Globals.MAP_WIDTH)
                {
                    Debug.Log("Size");
                    return false;
                }
                if (LevelCreator.isWall(i, j))
                {
                    Debug.Log("Value -- " + "( " + i + "," + j + ") " + Map[i][j]);
                    return false;
                }
            }
        }
        return true;
    }

    protected bool doRotation(int dir)
    {
        float angleRotation = 180.0f;
        switch (dir)
        {
            case Globals.UP:
                angleRotation = UP_ANGLE;
                break;
            case Globals.RIGHT:
                angleRotation = RIGHT_ANGLE;
                break;
            case Globals.DOWN:
                angleRotation = DOWN_ANGLE;
                break;
            case Globals.LEFT:
                angleRotation = LEFT_ANGLE;
                break;

        }

        float diff = transform.rotation.eulerAngles.y - angleRotation;
        if (Mathf.Abs(diff) <= 2.0f)
        {
            Vector3 eulerAngles = transform.eulerAngles;
            eulerAngles.y = angleRotation;
            transform.eulerAngles = eulerAngles;

            return true;
        }
        else {
            Quaternion newRotation = Quaternion.AngleAxis(angleRotation, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, Time.deltaTime * GHOST_ROTATE_SPEED);

            return false;
        }
    }
}
