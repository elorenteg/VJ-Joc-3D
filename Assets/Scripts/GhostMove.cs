using UnityEngine;
using UnityEngine.Rendering;
using System.Collections;

public class GhostMove : MonoBehaviour
{
    protected static float GHOST_SPEED = 20;
    protected static float GHOST_ROTATE_SPEED = 2.5f;

    private static int MAX_TIME_STATE = 15;

    private float GHOST_Y_POS = 18.5f;

    private int state;
    private int timeState;
    private bool isDead;

    protected int tile_x, tile_z;
    protected int new_tx, new_tz;

    private GhostAnimate animationScript;

    // Use this for initialization
    public void Start ()
    {
        isDead = false;
        state = 0;
        timeState = 0;
        
        animationScript = GetComponent<GhostAnimate>();
        //animationScript.SetTextures(animationScript.stateMove(), state);
    }

    // Update is called once per frame
    public void Update()
    {
        if (timeState == MAX_TIME_STATE)
        {
            timeState = 0;
            state = state + 1;
            if (state == 2) state = 0;

            if (!isDead) animationScript.SetTextures(animationScript.stateMove(), state);
            else animationScript.SetTextures(animationScript.stateDead(), state);
        }

        ++timeState;

        Vector3 pos = transform.position;
        pos.y = GHOST_Y_POS;
        transform.position = pos;

        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationX
                                      | RigidbodyConstraints.FreezeRotationY
                                      | RigidbodyConstraints.FreezeRotationZ;
    }

    public void SetDead()
    {
        isDead = true;
        animationScript.SetTextures(animationScript.stateDead(), state);

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
        tile_x = tx;
        tile_z = tz;
    }

    protected int GetTileX()
    {
        return tile_x;
    }

    protected int GetTileZ()
    {
        return tile_z;
    }

    protected Vector3 GetPosition(int tx, int tz)
    {
        return new Vector3((tx + Globals.GHOST_OFFSET_X) * Globals.TILE_SIZE, transform.position.y, 
            (tz + Globals.GHOST_OFFSET_Z) * Globals.TILE_SIZE);
    }

    public static void positionToTiles(Vector3 pos, out int tx, out int tz)
    {
        tx = (int) (pos.x - Globals.GHOST_OFFSET_X * Globals.TILE_SIZE) / Globals.TILE_SIZE;
        tz = (int) (pos.z - Globals.GHOST_OFFSET_Z * Globals.TILE_SIZE) / Globals.TILE_SIZE;
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
                if (Map[i][j] > 0 && Map[i][j] <= 3)
                {
                    Debug.Log("Value -- " + "( " + i + "," + j + ") " + Map[i][j]);
                    return false;
                }
            }
        }
        return true;
    }
}
