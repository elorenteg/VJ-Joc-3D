using UnityEngine;
using UnityEngine.Rendering;
using System.Collections;

public class GhostMove : MonoBehaviour
{
    protected static float GHOST_SPEED = 14.0f;
    protected static float GHOST_ROTATE_SPEED = 8.0f;

    protected static float UP_ANGLE = 90.0f;
    protected static float DOWN_ANGLE = 270.0f;
    protected static float LEFT_ANGLE = 0.0f;
    protected static float RIGHT_ANGLE = 180.0f;

    public static int MAX_FRAMES_STATE = 15;
    private int textureState;
    private int frameState;

    private bool isDead;
    private bool canBeKilled;

    protected int[] currentPath;
    private int currentDir;
    private bool currentDirCalculated;
    private int doorTx, doorTz;
    private int initTx, initTz;

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
    protected GameObject pacmanObj;

    private float GHOST_AUDIO_VOLUME = 0.5f;

    // Use this for initialization
    public void Start()
    {
        initGhost();

        GameObject gameManager = GameObject.Find("GameManager");
        levelManager = gameManager.GetComponent<LevelManager>();
    }

    public void restartGhost(Vector3 pos)
    {
        transform.position = pos;

        initGhost();
        initSpecificGhost();
    }

    public void initGhost()
    {
        textureState = 0;
        frameState = 0;

        isDead = false;
        canBeKilled = false;

        ghostState = LEAVING_BASE;
        currentDirCalculated = false;

        animationScript = GetComponent<GhostAnimate>();
        animationScript.SetTextures(animationScript.stateMove(), textureState);
    }

    protected virtual void initSpecificGhost() { }

    public void SetPacmanObj(GameObject pacman)
    {
        pacmanObj = pacman;
    }

    public void SetVisible(bool visible)
    {
        GetComponent<SphereCollider>().enabled = visible;
        GetComponentInChildren<SkinnedMeshRenderer>().GetComponent<Renderer>().enabled = visible;

        animationScript.StopSound();
    }

    // Update is called once per frame
    public void Update()
    {
        if (frameState == MAX_FRAMES_STATE)
        {
            frameState = 0;
            if (!canBeKilled || (canBeKilled && levelManager.lastTimeBonus())) {
                textureState = (textureState + 1) % 2;
            }

            UpdateTextures();
        }

        ++frameState;
    }

    public void UpdateTextures()
    {
        if (isDead) animationScript.SetTextures(animationScript.stateDead(), textureState);
        else if (canBeKilled) animationScript.SetTextures(animationScript.stateKilleable(), textureState);
        else animationScript.SetTextures(animationScript.stateMove(), textureState);
    }

    public void SetKilleable(bool killeable)
    {
        canBeKilled = killeable;

        if (canBeKilled)
        {
            textureState = 0;
            animationScript.SetTextures(animationScript.stateKilleable(), textureState);
        }

        updateState();
    }

    public void SetDead(bool dead)
    {
        isDead = dead;

        if (isDead)
        {
            textureState = 0;
            animationScript.SetTextures(animationScript.stateDead(), textureState);
        }

        updateState();
    }

    public bool ghostIsDead()
    {
        return isDead;
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

        initTx = tx;
        initTz = tz;
    }

    public void SetDoorTiles(int tx, int tz)
    {
        doorTx = tx;
        doorTz = tz;
    }

    public static bool isValid(int[][] Map, int tx, int tz, bool baseIsValid)
    {
        for (int i = tx - 1; i <= tx + 1; ++i)
        {
            for (int j = tz - 1; j <= tz + 1; ++j)
            {
                if (!LevelCreator.isValidTile(i, j)) return false;
                else if (LevelCreator.isWall(i, j)) return false;
                else if (!baseIsValid && LevelCreator.isBase(i, j)) return false;
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
            fixEulerAngle(angleRotation);

            return true;
        }
        else
        {
            Quaternion newRotation = Quaternion.AngleAxis(angleRotation, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, Time.deltaTime * GHOST_ROTATE_SPEED);

            return false;
        }
    }

    public void fixEulerAngle(float fixedAngle)
    {
        Vector3 euler = transform.eulerAngles;
        euler.y = fixedAngle;
        transform.eulerAngles = euler;
    }

    public void onMove(int[][] Map)
    {
        updateState();

        if (!currentDirCalculated) initMove(Map);

        followPath();
    }

    // Completely chasing Pacman
    public virtual void chasingPacman(int[][] Map)
    {
        bool baseIsValid = false;
        int pactx, pactz;
        PacmanMove moveScript = pacmanObj.GetComponent<PacmanMove>();
        moveScript.ActualTile(out pactx, out pactz);

        // Nos quedamos con un camino de 5 tiles para ir actualizando el camino hasta el pacman cada 5
        int[] allPath = BFS.calculatePath(Map, tileX, tileZ, pactx, pactz, baseIsValid);
        int size = Mathf.Min(5, allPath.Length);
        currentPath = new int[size];
        for (int i = 0; i < size; ++i)
        {
            currentPath[i] = allPath[i];
        }
    }

    public void followPath()
    {
        if (isMoving && transform.position == newPosition)
        {
            isMoving = false;
            tileX = newTileX;
            tileZ = newTileZ;
            ++currentDir;

            if (currentDir == currentPath.Length)
            {
                nextState();
                currentDirCalculated = false;
                return;
            }
            else SetDirection(currentPath[currentDir]);
        }

        if (isMoving)
        {
            animationScript.PlaySound(animationScript.stateMove(), GHOST_AUDIO_VOLUME);

            //float distCovered = (Time.time - startTime) * GHOST_SPEED;
            //float fracJourney = distCovered / duration;
            //transform.position = Vector3.Lerp(transform.position, newPosition, fracJourney);
            //transform.position = Vector3.MoveTowards(transform.position, newPosition, fracJourney);

            float step = GHOST_SPEED * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, newPosition, step);
        }
        else
        {
            animationScript.StopSound();
            bool ghostCanMove = doRotation(currentPath[currentDir]);

            if (ghostCanMove)
            {
                newPosition = LevelCreator.TileToPosition(newTileX, newTileZ, transform.position.y);
                //startTime = Time.time;
                //duration = Vector3.Distance(transform.position, newPosition) * LevelCreator.TILE_SIZE;
                isMoving = true;
            }
        }
    }

    public void SetDirection(int dir)
    {
        newTileX = tileX;
        newTileZ = tileZ;

        //int numSameDir = extendDir();

        if (dir == Globals.UP) newTileZ = newTileZ + 1;
        else if (dir == Globals.RIGHT) newTileX = newTileX + 1;
        else if (dir == Globals.DOWN) newTileZ = newTileZ - 1;
        else if (dir == Globals.LEFT) newTileX = newTileX - 1;
    }

    private int extendDir()
    {
        int dir = currentPath[currentDir];
        int numSameDir = 0;
        while (currentDir < currentPath.Length && dir == currentPath[currentDir])
        {
            ++numSameDir;
            ++currentDir;
        }
        --currentDir;

        return numSameDir;
    }

    private bool isWanderingBase()
    {
        return ghostState == WANDERING_BASE;
    }

    private bool isLeavingBase()
    {
        return ghostState == LEAVING_BASE;
    }

    private bool isChasingPacman()
    {
        return ghostState == CHASING_PACMAN;
    }

    private bool isEvadingPacman()
    {
        return ghostState == EVADING_PACMAN;
    }

    private bool isReturningBase()
    {
        return ghostState == RETURNING_BASE;
    }

    private void updateState()
    {
        if (canBeKilled)
        {
            if (isChasingPacman())
            {
                ghostState = EVADING_PACMAN;
                currentDirCalculated = false;
            }
        }
        else
        {
            if (isEvadingPacman())
            {
                ghostState = CHASING_PACMAN;
                currentDirCalculated = false;
            }
        }

        if (isDead)
        {
            if (isEvadingPacman())
            {
                ghostState = RETURNING_BASE;
                currentDirCalculated = false;
            }
        }
    }

    private void nextState()
    {
        switch (ghostState)
        {
            case WANDERING_BASE:
                ghostState = LEAVING_BASE;
                break;
            case LEAVING_BASE:
                ghostState = CHASING_PACMAN;
                break;
            case RETURNING_BASE:
                //ghostState = WANDERING_BASE;
                ghostState = LEAVING_BASE;
                SetDead(false);
                SetKilleable(false);
                break;
        }
    }

    private void leavingBase(int[][] Map)
    {
        bool baseIsValid = true;

        currentPath = BFS.calculatePath(Map, tileX, tileZ, doorTx, doorTz, baseIsValid);
    }

    private void evadingPacman(int[][] Map)
    {
        bool baseIsValid = false;
        int sectToMove = sectionToMove();

        int secTx, secTz;
        do
        {
            LevelCreator.TileInSection(tileX, tileZ, sectToMove, out secTx, out secTz);
        }
        while (!isValid(Map, secTx, secTz, baseIsValid));

        // Nos quedamos con un camino de 5 tiles para ir actualizando el camino hasta el pacman cada 5
        int[] allPath = BFS.calculatePath(Map, tileX, tileZ, secTx, secTz, baseIsValid);
        int size = Mathf.Min(5, allPath.Length);
        currentPath = new int[size];
        for (int i = 0; i < size; ++i)
        {
            currentPath[i] = allPath[i];
        }
    }

    private void returningBase(int[][] Map)
    {
        bool baseIsValid = true;

        currentPath = BFS.calculatePath(Map, tileX, tileZ, initTx, initTz, baseIsValid);
    }

    public void initMove(int[][] Map)
    {
        switch (ghostState)
        {
            case WANDERING_BASE:
                break;
            case LEAVING_BASE:
                leavingBase(Map);
                break;
            case CHASING_PACMAN:
                chasingPacman(Map);
                break;
            case EVADING_PACMAN:
                evadingPacman(Map);
                break;
            case RETURNING_BASE:
                returningBase(Map);
                break;
        }

        currentDirCalculated = true;
        currentDir = 0;
        isMoving = false;
        SetDirection(currentPath[currentDir]);
    }

    protected int sectionToMove()
    {
        int pactx, pactz;
        PacmanMove moveScript = pacmanObj.GetComponent<PacmanMove>();
        moveScript.ActualTile(out pactx, out pactz);

        int pacSection = moveScript.ActualSection();
        int ghostSection = LevelCreator.SectionTile(tileX, tileZ);

        int newSection;
        if (isEvadingPacman())
        {
            if (LevelCreator.AreDiagonalSections(pacSection, ghostSection)) newSection = ghostSection;
            else if (LevelCreator.AreContiguousSections(pacSection, ghostSection))
                newSection = LevelCreator.OppositeSection(pacSection);
            else newSection = LevelCreator.ContiguousSection(pacSection, tileX, tileZ);
        }
        else
        {
            newSection = LevelCreator.SECTION_BOTTOM_LEFT;
        }

        return newSection;
    }

    protected bool CanSeePacman(int[][] Map)
    {
        int pactx, pactz;
        PacmanMove moveScript = pacmanObj.GetComponent<PacmanMove>();
        moveScript.ActualTile(out pactx, out pactz);

        if ((tileX - 1 <= pactx && pactx <= tileX + 1) || (tileZ - 1 <= pactz && pactz <= tileZ + 1))
        {
            for (int tz = tileZ - 1; tz <= tileZ + 1; ++tz)
            {
                // LEFT
                for (int tx = tileX; tx >= pactx; --tx)
                {
                    if (LevelCreator.isValidTile(tx, tz))
                    {
                        if (LevelCreator.isWall(tx, tz)) break;
                        else if (pactx == tx && pactz == tz) return true;
                    }
                }
                // RIGHT
                for (int tx = tileX; tx <= pactx; ++tx)
                {
                    if (LevelCreator.isValidTile(tx, tz))
                    {
                        if (LevelCreator.isWall(tx, tz)) break;
                        else if (pactx == tx && pactz == tz) return true;
                    }
                }
            }
            
            for (int tx = tileX - 1; tx <= tileX + 1; ++tx)
            {
                // DOWN
                for (int tz = tileZ; tz >= pactz; --tz)
                {
                    if (LevelCreator.isValidTile(tx, tz))
                    {
                        if (LevelCreator.isWall(tx, tz)) break;
                        else if (pactx == tx && pactz == tz) return true;
                    }
                }
                // UP
                for (int tz = tileZ; tz <= pactz; ++tz)
                {
                    if (LevelCreator.isValidTile(tx, tz))
                    {
                        if (LevelCreator.isWall(tx, tz)) break;
                        else if (pactx == tx && pactz == tz) return true;
                    }
                }
            }
        }

        return false;
    }
}
