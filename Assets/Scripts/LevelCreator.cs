using UnityEngine;
using System.Collections;
using System.IO;
using System.Text;
using System.Collections.Generic;

public class LevelCreator : MonoBehaviour
{
    public GameObject cameraObject;
    public GameObject LightObject;
    public GameObject plane;
    public GameObject hole;
    public GameObject wall;
    public Texture texWallLatSmall;
    public Texture texWallLatBig1;
    public Texture texWallLatBig2;
    public Texture texWallLatBig3;
    public Texture texWallUp1;
    public Texture texWallUp2;
    public GameObject floor;
    public Texture floorTexture;
    public GameObject pacman;
    public GameObject ghost;
    public Texture blueGhostTexture;
    public Texture orangeGhostTexture;
    public Texture pinkGhostTexture;
    public Texture redGhostTexture;
    public GameObject coin;
    public GameObject bonus;
    public GameObject cherry;

    private static string levelPath = "..\\VJ-Joc-3D\\Assets\\Maps\\";

    public static int TILE_SIZE = 2;
    private static int[][] Map;
    public static int MAP_WIDTH;
    public static int MAP_HEIGHT;
    private const int PLANE_HEIGHT = 140;
    private const int PLANE_SEP = 30;

    public static int CELL_EMPTY = 0;
    private static char WALL_V = 'V';
    public static int WALL_V_C = 1;
    private static char WALL_H = 'H';
    public static int WALL_H_C = 2;
    private static char WALL_V_RES = 'v';
    private static char WALL_H_RES = 'h';
    public static int WALL_RES = 3;
    private static char GHOST_B = 'B';
    public static int GHOST_B_C = 4;
    private static char GHOST_O = 'O';
    public static int GHOST_O_C = 5;
    private static char GHOST_P = 'P';
    public static int GHOST_P_C = 6;
    private static char GHOST_R = 'R';
    public static int GHOST_R_C = 7;
    private static char PACMAN = '+';
    public static int PACMAN_C = 10;

    private static char COIN = '.';
    public static int COIN_C = 20;
    private static char BONUS = '*';
    public static int BONUS_C = 21;

    private static char BASE = '#';
    private static char DOOR = 'X';
    private static char DOOR_RES = 'x';
    public static int BASE_C = 30;
    private int doorTx, doorTz, doorTx2, doorTz2;

    private float GHOST_Y_POS = 6.0f;
    private float PACMAN_Y_POS = 18.0f;
    private float COIN_Y_POS = 10.0f;
    private float BONUS_Y_POS = 13.0f;
    private float CHERRY_Y_POS = 3.5f;

    private float FLOOR_HEIGHT = 1.0f;
    private const float WALL_HEIGHT = 7.5f;
    private Vector3 WALL_SCALE = new Vector3(4 * TILE_SIZE, WALL_HEIGHT, 2 * TILE_SIZE);
    private int WALL_V_ANGLE = -90;

    private Vector3 GHOST_SCALE = new Vector3(5.0f, 5.0f, 5.0f);
    private Vector2 GHOST_TEXTURE_SCALE = new Vector2(0.5f, 1.0f);

    private Vector3 PACMAN_SCALE = new Vector3(6.0f, 6.0f, 6.0f);
    private Vector2 PACMAN_TEXTURE_SCALE = new Vector2(0.5f, 1.0f);
    private static float PACMAN_OFFSET_X = 0.5f;
    private static float PACMAN_OFFSET_Z = -7.5f;

    private Vector3 COIN_SCALE = new Vector3(4.0f, 4.0f, 4.0f);
    private Vector2 COIN_TEXTURE_SCALE = new Vector2(0.5f, 1.0f);
    private float COIN_OFFSET = 0.55f;

    private Vector3 BONUS_SCALE = new Vector3(8.0f, 8.0f, 8.0f);
    private Vector2 BONUS_TEXTURE_SCALE = new Vector2(0.5f, 1.0f);

    private Vector3 CHERRY_SCALE = new Vector3(0.23f, 0.23f, 0.23f);

    private int PACMAN_SPEED_MOVE = 28;
    private int PACMAN_SPEED_TURN = 300;

    private List<GameObject> walls;
    private List<bool> animUpWall;
    private List<bool> animFrontWall;
    private List<bool> animBackWall;

    private bool playersCreated;

    private int timeState;
    private int MAX_TIME_STATE;

    void Start()
    {
        timeState = 0;
        MAX_TIME_STATE = 20;
        playersCreated = false;
    }

    void Update()
    {
        if (timeState == MAX_TIME_STATE)
        {
            timeState = 0;

            for (int i = 0; i < walls.Count; ++i)
            {
                walls[i].GetComponent<WallAnimate>().AnimateTexture();
            }
        }

        ++timeState;
    }

    public void loadLevel(int level)
    {
        DeleteAll();

        string fileLocation = levelPath + "level_" + level + ".txt";
        readMap(fileLocation);
        placeFloor();
        placePlanes();
        placeObjects();
    }

    public void DeleteAll()
    {
        walls = new List<GameObject>();
        animUpWall = new List<bool>();
        animFrontWall = new List<bool>();
        animBackWall = new List<bool>();

        string[] destroyTags = { Globals.TAG_COIN, Globals.TAG_BONUS, Globals.TAG_WALL, Globals.TAG_FLOOR };
        //Globals.TAG_PACMAN, Globals.TAG_GHOST_BLUE, Globals.TAG_GHOST_ORANGE, Globals.TAG_GHOST_PINK, Globals.TAG_GHOST_RED};

        for (int i = 0; i < destroyTags.Length; ++i)
        {
            GameObject[] gameObjects = GameObject.FindGameObjectsWithTag(destroyTags[i]);
            for (int j = 0; j < gameObjects.Length; j++)
            {
                Destroy(gameObjects[j]);
            }
        }
    }

    private bool readMap(string fileName)
    {
        try
        {
            string line;
            StreamReader theReader = new StreamReader(fileName, Encoding.Default);
            using (theReader)
            {
                MAP_HEIGHT = File.ReadAllLines(fileName).Length;

                Map = new int[MAP_HEIGHT][];

                int i = MAP_HEIGHT - 1;
                do
                {
                    line = theReader.ReadLine();

                    if (line != null)
                    {
                        MAP_WIDTH = line.Length;

                        int[] MapLine = new int[MAP_WIDTH];
                        for (int j = 0; j < MAP_WIDTH; ++j)
                        {
                            if (line[j] == WALL_V)
                            {
                                MapLine[j] = WALL_V_C;
                            }
                            else if (line[j] == WALL_H)
                            {
                                MapLine[j] = WALL_H_C;
                            }
                            else if (line[j] == WALL_V_RES || line[j] == WALL_H_RES)
                            {
                                MapLine[j] = WALL_RES;
                            }
                            else if (line[j] == GHOST_B)
                            {
                                MapLine[j] = GHOST_B_C;
                            }
                            else if (line[j] == GHOST_O)
                            {
                                MapLine[j] = GHOST_O_C;
                            }
                            else if (line[j] == GHOST_P)
                            {
                                MapLine[j] = GHOST_P_C;
                            }
                            else if (line[j] == GHOST_R)
                            {
                                MapLine[j] = GHOST_R_C;
                            }
                            else if (line[j] == PACMAN)
                            {
                                MapLine[j] = PACMAN_C;
                            }
                            else if (line[j] == COIN)
                            {
                                MapLine[j] = COIN_C;
                            }
                            else if (line[j] == BONUS)
                            {
                                MapLine[j] = BONUS_C;
                            }
                            else if (line[j] == BASE)
                            {
                                MapLine[j] = BASE_C;
                            }
                            else if (line[j] == DOOR)
                            {
                                MapLine[j] = CELL_EMPTY;
                                doorTx = j;
                                doorTz = i;
                            }
                            else if (line[j] == DOOR_RES)
                            {
                                MapLine[j] = CELL_EMPTY;
                                doorTx2 = j;
                                doorTz2 = i;
                            }
                            else
                            {
                                MapLine[j] = CELL_EMPTY;
                            }
                        }
                        Map[i] = MapLine;
                        --i;
                    }
                } while (line != null);

                theReader.Close();

                return true;
            }
        }
        catch (System.Exception e)
        {
            Debug.Log(e.Message);
            return false;
        }
    }

    public int[][] GetMap()
    {
        return Map;
    }

    public static bool isValidTile(int tx, int tz)
    {
        return (tx >= 0 && tz >= 0 && tx < MAP_WIDTH && tz < MAP_HEIGHT);
    }

    public static bool isWall(int tx, int tz)
    {
        return (Map[tz][tx] == WALL_H_C || Map[tz][tx] == WALL_V_C || Map[tz][tx] == WALL_RES);
    }

    public static bool isBase(int tx, int tz)
    {
        return (Map[tz][tx] == BASE_C || Map[tz][tx] == GHOST_B_C || Map[tz][tx] == GHOST_O_C || Map[tz][tx] == GHOST_P_C || Map[tz][tx] == GHOST_R_C);
    }

    public static bool isValidAndPlaceableTile(int tx, int tz)
    {
        return isValidTile(tx, tx) && !isWall(tx, tz) && !isBase(tx, tz);
    }

    public static void positionToTile(Vector3 pos, out int tx, out int tz)
    {
        tx = (int)(pos.x - TILE_SIZE / 2) / TILE_SIZE;
        tz = (int)(pos.z - TILE_SIZE / 2) / TILE_SIZE;
    }

    public static void pacmanPositionToTile(Vector3 pos, out int tx, out int tz)
    {
        tx = (int)pos.x / TILE_SIZE;
        tz = (int)pos.z / TILE_SIZE;
    }

    public static Vector3 TileToPosition(int tx, int tz, float y)
    {
        Vector3 pos;
        pos.y = y;
        pos.x = tx * TILE_SIZE;
        pos.z = tz * TILE_SIZE;

        pos.x += TILE_SIZE / 2;
        pos.z += TILE_SIZE / 2;

        return pos;
    }

    private void placeFloor()
    {
        Vector3 floorPosition = new Vector3(MAP_HEIGHT * TILE_SIZE / 2, 0.0f, MAP_WIDTH * TILE_SIZE / 2);
        Vector3 floorRotation = new Vector3(0.0f, 0.0f, 0.0f);
        Vector3 floorScale = new Vector3(MAP_WIDTH * TILE_SIZE, FLOOR_HEIGHT, MAP_HEIGHT * TILE_SIZE);

        floorPosition.x -= WALL_SCALE.x;
        floorPosition.z += WALL_SCALE.x;

        GameObject newFloor = Instantiate(floor, floorPosition, Quaternion.Euler(floorRotation)) as GameObject;
        newFloor.transform.localScale = floorScale;

        Renderer renderer = newFloor.GetComponent<Renderer>();
        renderer.material.mainTexture = floorTexture;
        renderer.material.mainTextureScale = new Vector2(MAP_WIDTH / 8, MAP_HEIGHT / 8);

        newFloor.SetActive(true);
    }

    void placePlanes()
    {

        Vector4 xSca = new Vector4(MAP_WIDTH * TILE_SIZE, MAP_HEIGHT * TILE_SIZE, MAP_WIDTH * TILE_SIZE, MAP_HEIGHT * TILE_SIZE);

        Vector4 xPos = new Vector4(MAP_WIDTH * TILE_SIZE / 2, MAP_WIDTH * TILE_SIZE + PLANE_SEP, MAP_WIDTH * TILE_SIZE / 2, 0 - PLANE_SEP);
        Vector4 zPos = new Vector4(0 - PLANE_SEP, MAP_HEIGHT * TILE_SIZE / 2, MAP_HEIGHT * TILE_SIZE + PLANE_SEP, MAP_HEIGHT * TILE_SIZE / 2);

        Vector4 xRot = new Vector4(90, 90, 90, 90);
        Vector4 yRot = new Vector4(0, 270, 180, 90);

        for (int i = 0; i < 4; ++i)
        {
            Vector3 planePosition = new Vector3(xPos[i], 0, zPos[i]);
            Vector3 planeRotation = new Vector3(xRot[i], yRot[i], 0.0f);
            Vector3 planeScale = new Vector3(xSca[i] + PLANE_SEP * TILE_SIZE, 1, PLANE_HEIGHT);

            GameObject newPlane = Instantiate(plane, planePosition, Quaternion.Euler(planeRotation)) as GameObject;
            newPlane.transform.localScale = planeScale;

            Renderer renderer = newPlane.GetComponent<Renderer>();
            renderer.material.mainTextureScale = new Vector2(xSca[i] / TILE_SIZE / 32, 1.0f);

            newPlane.SetActive(true);
        }

        Vector3 holePosition = new Vector3(MAP_HEIGHT * TILE_SIZE / 2, -PLANE_HEIGHT / 2, MAP_WIDTH * TILE_SIZE / 2);
        Vector3 holeRotation = new Vector3(0, 0, 0);
        Vector3 holeScale = new Vector3(MAP_WIDTH * TILE_SIZE + PLANE_SEP * 2, 0, MAP_HEIGHT * TILE_SIZE + PLANE_SEP * 2);

        holePosition.x -= WALL_SCALE.x;
        holePosition.z += WALL_SCALE.x;

        GameObject newHole = Instantiate(hole, holePosition, Quaternion.Euler(holeRotation)) as GameObject;
        newHole.transform.localScale = holeScale;

        newHole.SetActive(true);
    }

    private void placeObjects()
    {
        for (int tz = 0; tz < MAP_HEIGHT; ++tz)
        {
            for (int tx = 0; tx < MAP_WIDTH; ++tx)
            {
                int cell = Map[tz][tx];
                if (cell != CELL_EMPTY)
                {
                    GameObject element;
                    Vector3 cellPosition;
                    Vector3 cellScale;
                    Texture texture;
                    Vector2 textureScale;

                    if (cell == WALL_V_C)
                    {
                        element = wall;
                        cellPosition = new Vector3(tx * TILE_SIZE, WALL_HEIGHT / 2 + FLOOR_HEIGHT / 2, tz * TILE_SIZE);
                        cellScale = WALL_SCALE;
                        texture = null;
                        textureScale = new Vector2(0.5f, 1.0f);

                        cellPosition.x += WALL_SCALE.z / 2;
                        cellPosition.z += WALL_SCALE.x / 2;
                    }
                    else if (cell == WALL_H_C)
                    {
                        element = wall;
                        cellPosition = new Vector3(tx * TILE_SIZE, WALL_HEIGHT / 2 + FLOOR_HEIGHT / 2, tz * TILE_SIZE);
                        cellScale = WALL_SCALE;
                        texture = null;
                        textureScale = new Vector2(0.5f, 1.0f);

                        cellPosition.x += WALL_SCALE.x / 2;
                        cellPosition.z += WALL_SCALE.z / 2;
                    }
                    else if (cell == GHOST_B_C)
                    {
                        element = ghost;
                        cellPosition = new Vector3(tx * TILE_SIZE, GHOST_Y_POS, tz * TILE_SIZE);
                        cellScale = new Vector3(GHOST_SCALE.x, GHOST_SCALE.y, GHOST_SCALE.z);
                        texture = blueGhostTexture;
                        textureScale = new Vector2(GHOST_TEXTURE_SCALE.x, GHOST_TEXTURE_SCALE.y);

                        cellPosition.x += TILE_SIZE / 2;
                        cellPosition.z += TILE_SIZE / 2;
                    }
                    else if (cell == GHOST_O_C)
                    {
                        element = ghost;
                        cellPosition = new Vector3(tx * TILE_SIZE, GHOST_Y_POS, tz * TILE_SIZE);
                        cellScale = GHOST_SCALE;
                        texture = orangeGhostTexture;
                        textureScale = new Vector2(GHOST_TEXTURE_SCALE.x, GHOST_TEXTURE_SCALE.y);

                        cellPosition.x += TILE_SIZE / 2;
                        cellPosition.z += TILE_SIZE / 2;
                    }
                    else if (cell == GHOST_P_C)
                    {
                        element = ghost;
                        cellPosition = new Vector3(tx * TILE_SIZE, GHOST_Y_POS, tz * TILE_SIZE);
                        cellScale = GHOST_SCALE;
                        texture = pinkGhostTexture;
                        textureScale = new Vector2(GHOST_TEXTURE_SCALE.x, GHOST_TEXTURE_SCALE.y);

                        cellPosition.x += TILE_SIZE / 2;
                        cellPosition.z += TILE_SIZE / 2;
                    }
                    else if (cell == GHOST_R_C)
                    {
                        element = ghost;
                        cellPosition = new Vector3(tx * TILE_SIZE, GHOST_Y_POS, tz * TILE_SIZE);
                        cellScale = GHOST_SCALE;
                        texture = redGhostTexture;
                        textureScale = new Vector2(GHOST_TEXTURE_SCALE.x, GHOST_TEXTURE_SCALE.y);

                        cellPosition.x += TILE_SIZE / 2;
                        cellPosition.z += TILE_SIZE / 2;
                    }
                    else if (cell == PACMAN_C)
                    {
                        element = pacman;
                        cellPosition = new Vector3(tx * TILE_SIZE, PACMAN_Y_POS, tz * TILE_SIZE);
                        cellScale = PACMAN_SCALE;
                        texture = null;
                        textureScale = PACMAN_TEXTURE_SCALE;

                        cellPosition.x += PACMAN_OFFSET_X * TILE_SIZE;
                        cellPosition.z += PACMAN_OFFSET_Z * TILE_SIZE;

                        // El mapa es de numTiles pares y asi colocamos al Pacman entre medio de dos tiles
                        //if (tx < MAP_WIDTH / 2) cellPosition.x += TILE_SIZE / 2;
                        //else cellPosition.x -= TILE_SIZE / 2;
                    }
                    else if (cell == COIN_C)
                    {
                        element = coin;
                        cellPosition = new Vector3(tx * TILE_SIZE, COIN_Y_POS, tz * TILE_SIZE);
                        cellScale = COIN_SCALE;
                        texture = null;
                        textureScale = COIN_TEXTURE_SCALE;

                        cellPosition.x += COIN_OFFSET * TILE_SIZE;
                        cellPosition.z += COIN_OFFSET * TILE_SIZE;
                    }
                    else if (cell == BONUS_C)
                    {
                        element = bonus;
                        cellPosition = new Vector3(tx * TILE_SIZE, BONUS_Y_POS, tz * TILE_SIZE);
                        cellScale = BONUS_SCALE;
                        texture = null;
                        textureScale = BONUS_TEXTURE_SCALE;

                        cellPosition.x -= 0.2f * TILE_SIZE;
                        cellPosition.z += 0.5f * TILE_SIZE;

                        cellPosition.y -= 4 * TILE_SIZE;
                    }
                    else if (cell == WALL_RES || cell == BASE_C) continue;
                    else
                    {
                        Debug.LogError("Creating a non empty cell");
                        element = null;
                        cellPosition = Vector3.zero;
                        cellScale = Vector3.zero;
                        texture = null;
                        textureScale = Vector2.zero;
                    }

                    if ((cell == PACMAN_C || cell == GHOST_B_C || cell == GHOST_O_C || cell == GHOST_P_C || cell == GHOST_R_C))
                    {
                        if (playersCreated) RestartPlayer(cell, cellPosition, tx, tz);
                        else
                        {
                            GameObject newObject;
                            newObject = Instantiate(element, cellPosition, element.transform.rotation) as GameObject;
                            newObject.transform.parent = transform;
                            newObject.transform.localScale = cellScale;

                            newObject.SetActive(true);

                            if (cell == GHOST_B_C || cell == GHOST_O_C || cell == GHOST_P_C || cell == GHOST_R_C)
                            {
                                GhostAnimate animationScript = newObject.GetComponent<GhostAnimate>();
                                animationScript.SetBodyTexture(texture);
                                animationScript.Start();

                                if (cell == GHOST_B_C)
                                {
                                    newObject.AddComponent<GhostBlueMove>();
                                    newObject.tag = Globals.TAG_GHOST_BLUE;
                                }
                                else if (cell == GHOST_O_C)
                                {
                                    newObject.AddComponent<GhostOrangeMove>();
                                    newObject.tag = Globals.TAG_GHOST_ORANGE;
                                }
                                else if (cell == GHOST_P_C)
                                {
                                    newObject.AddComponent<GhostPinkMove>();
                                    newObject.tag = Globals.TAG_GHOST_PINK;
                                }
                                else if (cell == GHOST_R_C)
                                {
                                    newObject.AddComponent<GhostRedMove>();
                                    newObject.tag = Globals.TAG_GHOST_RED;
                                }
                            }

                            RestartPlayer(cell, cellPosition, tx, tz);
                        }
                    }
                    else if (cell == WALL_H_C || cell == WALL_V_C)
                    {
                        GameObject newObject;
                        newObject = Instantiate(element, cellPosition, element.transform.rotation) as GameObject;
                        newObject.transform.parent = transform;
                        newObject.transform.localScale = cellScale;

                        newObject.SetActive(true);

                        if (cell == WALL_V_C)
                        {
                            Vector3 center = newObject.transform.position;
                            //center -= new Vector3(WALL_SCALE.x / (5f * TILE_SIZE), 0, WALL_SCALE.z / 2);
                            newObject.transform.RotateAround(center, transform.up, WALL_V_ANGLE);
                        }

                        walls.Add(newObject);

                        float rand = Random.value;
                        Texture texLat1, texLat2, texUp;
                        bool animLat1, animLat2, animUp;
                        if (rand <= 0.33f)
                        {
                            texLat1 = texWallLatBig1;
                            texLat2 = texWallLatBig3;
                            animLat1 = true;
                            animLat2 = false;

                        }
                        else if (rand <= 0.66f)
                        {
                            texLat1 = texWallLatBig2;
                            texLat2 = texWallLatBig3;
                            animLat1 = false;
                            animLat2 = false;
                        }
                        else
                        {
                            texLat1 = texWallLatBig3;
                            texLat2 = texWallLatBig1;
                            animLat1 = false;
                            animLat2 = true;
                        }

                        if (rand <= 0.5f)
                        {
                            texUp = texWallUp1;
                            animUp = true;
                        }
                        else
                        {
                            texUp = texWallUp2;
                            animUp = false;
                        }

                        WallAnimate animationScript = newObject.GetComponent<WallAnimate>();
                        animationScript.SetTextures(texUp, texLat1, texLat2, texWallLatSmall);
                        animationScript.SetAnimatedWalls(animUp, animLat1, animLat2);
                        animationScript.AnimateTexture();
                    }
                    else
                    {
                        GameObject newObject;
                        newObject = Instantiate(element, cellPosition, element.transform.rotation) as GameObject;
                        newObject.transform.parent = transform;
                        newObject.transform.localScale = cellScale;

                        newObject.SetActive(true);
                    }
                }
            }
        }
        playersCreated = true;
        SetGhostTarget();
    }

    private void SetGhostTarget()
    {
        GameObject pacmanObj = GameObject.FindGameObjectsWithTag(Globals.TAG_PACMAN)[0];

        string[] ghostTags = { Globals.TAG_GHOST_BLUE, Globals.TAG_GHOST_ORANGE, Globals.TAG_GHOST_PINK,
            Globals.TAG_GHOST_RED };

        GameObject ghostObj;

        ghostObj = GameObject.FindGameObjectsWithTag(Globals.TAG_GHOST_BLUE)[0];
        GhostBlueMove blueScript = ghostObj.GetComponent<GhostBlueMove>();
        blueScript.SetPacmanObj(pacmanObj);

        ghostObj = GameObject.FindGameObjectsWithTag(Globals.TAG_GHOST_ORANGE)[0];
        GhostOrangeMove orangeScript = ghostObj.GetComponent<GhostOrangeMove>();
        orangeScript.SetPacmanObj(pacmanObj);

        ghostObj = GameObject.FindGameObjectsWithTag(Globals.TAG_GHOST_PINK)[0];
        GhostPinkMove pinkScript = ghostObj.GetComponent<GhostPinkMove>();
        pinkScript.SetPacmanObj(pacmanObj);

        ghostObj = GameObject.FindGameObjectsWithTag(Globals.TAG_GHOST_RED)[0];
        GhostRedMove redScript = ghostObj.GetComponent<GhostRedMove>();
        redScript.SetPacmanObj(pacmanObj);
    }

    public void RestartPlayer(int cell, Vector3 cellPosition, int tx, int tz)
    {
        GameObject player;
        if (cell == PACMAN_C)
        {
            player = GameObject.FindGameObjectsWithTag(Globals.TAG_PACMAN)[0];

            CameraMove cameraScript = cameraObject.GetComponent<Camera>().GetComponent<CameraMove>();
            cameraScript.SetPacman(player);
            cameraScript.SetInitPosition(MAP_HEIGHT * TILE_SIZE, MAP_WIDTH * TILE_SIZE);

            LightObject.GetComponent<Light>().transform.position = new Vector3(MAP_WIDTH * TILE_SIZE / 2, 30, MAP_HEIGHT * TILE_SIZE / 2);
            PacmanMove pacmanmove = player.AddComponent<PacmanMove>();
            pacmanmove.moveSpeed = PACMAN_SPEED_MOVE;
            pacmanmove.turnSpeed = PACMAN_SPEED_TURN;
        }
        else if (cell == GHOST_B_C)
        {
            player = GameObject.FindGameObjectsWithTag(Globals.TAG_GHOST_BLUE)[0];
            GhostBlueMove moveScript = player.GetComponent<GhostBlueMove>();
            moveScript.restartGhost(cellPosition);
            moveScript.SetInitTiles(tx, tz);
            moveScript.SetDoorTiles(doorTx, doorTz);
        }
        else if (cell == GHOST_O_C)
        {
            player = GameObject.FindGameObjectsWithTag(Globals.TAG_GHOST_ORANGE)[0];
            GhostOrangeMove moveScript = player.GetComponent<GhostOrangeMove>();
            moveScript.restartGhost(cellPosition);
            moveScript.SetInitTiles(tx, tz);
            moveScript.SetDoorTiles(doorTx2, doorTz2);
        }
        else if (cell == GHOST_P_C)
        {
            player = GameObject.FindGameObjectsWithTag(Globals.TAG_GHOST_PINK)[0];
            GhostPinkMove moveScript = player.GetComponent<GhostPinkMove>();
            moveScript.restartGhost(cellPosition);
            moveScript.SetInitTiles(tx, tz);
            moveScript.SetDoorTiles(doorTx, doorTz);
        }
        else
        {
            player = GameObject.FindGameObjectsWithTag(Globals.TAG_GHOST_RED)[0];
            GhostRedMove moveScript = player.GetComponent<GhostRedMove>();
            moveScript.restartGhost(cellPosition);
            moveScript.SetInitTiles(tx, tz);
            moveScript.SetDoorTiles(doorTx2, doorTz2);
        }

        int angle = 180;
        if (cell == GHOST_O_C || cell == GHOST_R_C) angle = 0;
        else if (cell == PACMAN_C) angle = -90;

        SkinnedMeshRenderer skinnedMeshRenderer = player.GetComponentInChildren<SkinnedMeshRenderer>();
        player.transform.RotateAround(skinnedMeshRenderer.bounds.center, new Vector3(0, 1, 0), angle);
    }

    public void instantiateCherry()
    {
        GameObject element = cherry;
        Vector3 cellPosition = generateValidRandomVector3();
        Vector3 cellScale = CHERRY_SCALE;

        GameObject newObject = Instantiate(element, cellPosition, element.transform.rotation) as GameObject;
        newObject.SetActive(true);
        newObject.transform.localScale = cellScale;
        newObject.transform.parent = transform;
        newObject.tag = Globals.TAG_CHERRY;
    }

    public void destroyObject(string tag)
    {
        GameObject gameObject = GameObject.FindGameObjectWithTag(tag);
        Destroy(gameObject);
    }

    public Vector3 generateValidRandomVector3()
    {
        Vector3 position = new Vector3(0.0f, 0.0f, 0.0f);

        int tx = 0;
        int tz = 0;
        do
        {
            tx = Random.Range(0, MAP_WIDTH);
            tz = Random.Range(0, MAP_HEIGHT);
        } while (!isValidAndPlaceableTile9x9(tx - 1, tz));

        position = TileToPosition(tx, tz, CHERRY_Y_POS);

        return position;
    }

    public static bool isValidAndPlaceableTile9x9(int tx, int tz)
    {
        for (int i = tx - 2; i <= tx + 2; ++i)
        {
            for (int j = tz - 2; j <= tz + 2; ++j)
            {
                if (!isValidTile(i, j)) return false;
                else if (isWall(i, j)) return false;
                else if (isBase(i, j)) return false;
            }
        }
        return true;
    }
}
