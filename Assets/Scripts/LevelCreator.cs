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

    private static string levelPath = "..\\VJ-Joc-3D\\Assets\\Maps\\";

    private const int TILE_SIZE = 2;
    private int[][] Map;
    private int MAP_WIDTH;
    private int MAP_HEIGHT;
    private const int PLANE_HEIGHT = TILE_SIZE * 70;
    private const int PLANE_SEP = 30;

    public static string TAG_GHOST_BLUE = "ghost_blue";
    public static string TAG_GHOST_ORANGE = "ghost_orange";
    public static string TAG_GHOST_PINK = "ghost_pink";
    public static string TAG_GHOST_RED = "ghost_red";

    private static int CELL_EMPTY = 0;
    private static char WALL_V = 'V';
    private static int WALL_V_C = 1;
    private static char WALL_H = 'H';
    private static int WALL_H_C = 2;
    private static char WALL_V_RES = 'v';
    private static char WALL_H_RES = 'h';
    private static int WALL_RES = 3;
    private static char GHOST_B = 'B';
    private static int GHOST_B_C = 4;
    private static char GHOST_O = 'O';
    private static int GHOST_O_C = 5;
    private static char GHOST_P = 'P';
    private static int GHOST_P_C = 6;
    private static char GHOST_R = 'R';
    private static int GHOST_R_C = 7;
    private static char PACMAN = '+';
    private static int PACMAN_C = 10;

    private static char COIN = '.';
    private static int COIN_C = 20;
    private static char BONUS = '*';
    private static int BONUS_C = 21;

    private float GHOST_Y_POS = 18.5f;
    private float PACMAN_Y_POS = 18.0f;
    private float COIN_Y_POS = 10.0f;
    private float BONUS_Y_POS = 13.0f;

    private float FLOOR_HEIGHT = 1.0f;
    private const float WALL_HEIGHT = 7.5f;
    private Vector3 WALL_SCALE = new Vector3(4 * TILE_SIZE, WALL_HEIGHT, 2 * TILE_SIZE);
    private int WALL_V_ANGLE = -90;

    private Vector3 GHOST_SCALE = new Vector3(6.0f, 6.0f, 6.0f);
    private Vector2 GHOST_TEXTURE_SCALE = new Vector2(0.5f, 1.0f);
    private float GHOST_OFFSET_X = 11.6f;
    private float GHOST_OFFSET_Z = 13.9f;

    private Vector3 PACMAN_SCALE = new Vector3(6.0f, 6.0f, 6.0f);
    private Vector2 PACMAN_TEXTURE_SCALE = new Vector2(0.5f, 1.0f);
    private float PACMAN_OFFSET_X = 0.38f;
    private float PACMAN_OFFSET_Z = -6.2f;

    private Vector3 COIN_SCALE = new Vector3(4.0f, 4.0f, 4.0f);
    private Vector2 COIN_TEXTURE_SCALE = new Vector2(0.5f, 1.0f);
    private float COIN_OFFSET = 0.55f;

    private Vector3 BONUS_SCALE = new Vector3(8.0f, 8.0f, 8.0f);
    private Vector2 BONUS_TEXTURE_SCALE = new Vector2(0.5f, 1.0f);

    private List<GameObject> walls;
    private List<bool> animUpWall;
    private List<bool> animFrontWall;
    private List<bool> animBackWall;

    private int timeState;
    private int MAX_TIME_STATE;

    void Start()
    {
        timeState = 0;
        MAX_TIME_STATE = 20;
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

        string[] destroyTags = { "pacman", "ghost", "coin", "bonus", "wall", "floor",
            TAG_GHOST_BLUE, TAG_GHOST_ORANGE, TAG_GHOST_PINK, TAG_GHOST_RED};

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
            Vector3 planePosition = new Vector3(xPos[i], -PLANE_HEIGHT / 3, zPos[i]);
            Vector3 planeRotation = new Vector3(xRot[i], yRot[i], 0.0f);
            Vector3 planeScale = new Vector3(xSca[i] + PLANE_SEP * TILE_SIZE, 1, PLANE_HEIGHT);

            GameObject newPlane = Instantiate(plane, planePosition, Quaternion.Euler(planeRotation)) as GameObject;
            newPlane.transform.localScale = planeScale;

            Renderer renderer = newPlane.GetComponent<Renderer>();
            renderer.material.mainTextureScale = new Vector2(xSca[i] / TILE_SIZE / 32, PLANE_HEIGHT / TILE_SIZE / 28);

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
        for (int i = 0; i < MAP_HEIGHT; ++i)
        {
            for (int j = 0; j < MAP_WIDTH; ++j)
            {
                int cell = Map[i][j];
                if (cell != CELL_EMPTY)
                {
                    GameObject element;
                    Vector3 cellPosition;
                    Vector3 cellScale;
                    Texture texture;
                    Vector2 textureScale;
                    Quaternion cellQuaternion;

                    if (cell == WALL_V_C)
                    {
                        element = wall;
                        cellPosition = new Vector3(j * TILE_SIZE, WALL_HEIGHT / 2 + FLOOR_HEIGHT / 2, i * TILE_SIZE);
                        cellScale = WALL_SCALE;
                        texture = null;
                        textureScale = new Vector2(0.5f, 1.0f);

                        cellQuaternion = Quaternion.AngleAxis(90.0f, Vector3.up);

                        cellPosition.x += WALL_SCALE.z / 2;
                        cellPosition.z += WALL_SCALE.x / 2;
                    }
                    else if (cell == WALL_H_C)
                    {
                        element = wall;
                        cellPosition = new Vector3(j * TILE_SIZE, WALL_HEIGHT / 2 + FLOOR_HEIGHT / 2, i * TILE_SIZE);
                        cellScale = WALL_SCALE;
                        texture = null;
                        textureScale = new Vector2(0.5f, 1.0f);

                        cellQuaternion = Quaternion.AngleAxis(0.0f, Vector3.up);

                        cellPosition.x += WALL_SCALE.x / 2;
                        cellPosition.z += WALL_SCALE.z / 2;
                    }
                    else if (cell == GHOST_B_C)
                    {
                        element = ghost;
                        cellPosition = new Vector3(j * TILE_SIZE, GHOST_Y_POS, i * TILE_SIZE);
                        cellScale = new Vector3(GHOST_SCALE.x, GHOST_SCALE.y, GHOST_SCALE.z);
                        texture = blueGhostTexture;
                        textureScale = new Vector2(GHOST_TEXTURE_SCALE.x, GHOST_TEXTURE_SCALE.y);

                        cellQuaternion = Quaternion.AngleAxis(0.0f, Vector3.up);

                        cellPosition.x += GHOST_OFFSET_X * TILE_SIZE;
                        cellPosition.z += GHOST_OFFSET_Z * TILE_SIZE;
                    }
                    else if (cell == GHOST_O_C)
                    {
                        element = ghost;
                        cellPosition = new Vector3(j * TILE_SIZE, GHOST_Y_POS, i * TILE_SIZE);
                        cellScale = new Vector3(GHOST_SCALE.x, GHOST_SCALE.y, GHOST_SCALE.z);
                        texture = orangeGhostTexture;
                        textureScale = new Vector2(GHOST_TEXTURE_SCALE.x, GHOST_TEXTURE_SCALE.y);

                        cellQuaternion = Quaternion.AngleAxis(0.0f, Vector3.up);

                        cellPosition.x += GHOST_OFFSET_X * TILE_SIZE;
                        cellPosition.z += GHOST_OFFSET_Z * TILE_SIZE;
                    }
                    else if (cell == GHOST_P_C)
                    {
                        element = ghost;
                        cellPosition = new Vector3(j * TILE_SIZE, GHOST_Y_POS, i * TILE_SIZE);
                        cellScale = new Vector3(GHOST_SCALE.x, GHOST_SCALE.y, GHOST_SCALE.z);
                        texture = pinkGhostTexture;
                        textureScale = new Vector2(GHOST_TEXTURE_SCALE.x, GHOST_TEXTURE_SCALE.y);

                        cellQuaternion = Quaternion.AngleAxis(0.0f, Vector3.up);

                        cellPosition.x += GHOST_OFFSET_X * TILE_SIZE;
                        cellPosition.z += GHOST_OFFSET_Z * TILE_SIZE;
                    }
                    else if (cell == GHOST_R_C)
                    {
                        element = ghost;
                        cellPosition = new Vector3(j * TILE_SIZE, GHOST_Y_POS, i * TILE_SIZE);
                        cellScale = new Vector3(GHOST_SCALE.x, GHOST_SCALE.y, GHOST_SCALE.z);
                        texture = redGhostTexture;
                        textureScale = new Vector2(GHOST_TEXTURE_SCALE.x, GHOST_TEXTURE_SCALE.y);

                        cellQuaternion = Quaternion.AngleAxis(0.0f, Vector3.up);

                        cellPosition.x += GHOST_OFFSET_X * TILE_SIZE;
                        cellPosition.z += GHOST_OFFSET_Z * TILE_SIZE;
                    }
                    else if (cell == PACMAN_C)
                    {
                        element = pacman;
                        cellPosition = new Vector3(j * TILE_SIZE, PACMAN_Y_POS, i * TILE_SIZE);
                        cellScale = new Vector3(PACMAN_SCALE.x, PACMAN_SCALE.y, PACMAN_SCALE.z);
                        texture = null;
                        textureScale = new Vector2(PACMAN_TEXTURE_SCALE.x, PACMAN_TEXTURE_SCALE.y);

                        cellQuaternion = Quaternion.AngleAxis(0.0f, Vector3.up);

                        cellPosition.x += PACMAN_OFFSET_X * TILE_SIZE;
                        cellPosition.z += PACMAN_OFFSET_Z * TILE_SIZE;

                        // El mapa es de numTiles pares y asi colocamos al Pacman entre medio de dos tiles
                        if (j < MAP_WIDTH / 2) cellPosition.x += TILE_SIZE / 2;
                        else cellPosition.x -= TILE_SIZE / 2;
                    }
                    else if (cell == COIN_C)
                    {
                        element = coin;
                        cellPosition = new Vector3(j * TILE_SIZE, COIN_Y_POS, i * TILE_SIZE);
                        cellScale = new Vector3(COIN_SCALE.x, COIN_SCALE.y, COIN_SCALE.z);
                        texture = null;
                        textureScale = new Vector2(COIN_TEXTURE_SCALE.x, COIN_TEXTURE_SCALE.y);

                        cellQuaternion = Quaternion.AngleAxis(0.0f, Vector3.up);

                        cellPosition.x += COIN_OFFSET * TILE_SIZE;
                        cellPosition.z += COIN_OFFSET * TILE_SIZE;
                    }
                    else if (cell == BONUS_C)
                    {
                        element = bonus;
                        cellPosition = new Vector3(j * TILE_SIZE, BONUS_Y_POS, i * TILE_SIZE);
                        cellScale = new Vector3(BONUS_SCALE.x, BONUS_SCALE.y, BONUS_SCALE.z);
                        texture = null;
                        textureScale = new Vector2(BONUS_TEXTURE_SCALE.x, BONUS_TEXTURE_SCALE.y);

                        cellQuaternion = Quaternion.AngleAxis(0.0f, Vector3.up);

                        cellPosition.x -= 0.2f * TILE_SIZE;
                        cellPosition.z += 0.5f * TILE_SIZE;

                        cellPosition.y -= 4 * TILE_SIZE;
                    }
                    else if (cell == WALL_RES) continue;
                    else
                    {
                        Debug.LogError("Creating a non empty cell");
                        element = null;
                        cellPosition = Vector3.zero;
                        cellScale = Vector3.zero;
                        texture = null;
                        textureScale = Vector2.zero;

                        cellQuaternion = Quaternion.AngleAxis(0.0f, Vector3.zero);
                    }

                    GameObject newObject = Instantiate(element, cellPosition, element.transform.rotation) as GameObject;
                    newObject.transform.parent = transform;
                    newObject.transform.localScale = cellScale;

                    newObject.SetActive(true);

                    int angle = -90;
                    if (cell == GHOST_O_C || cell == GHOST_R_C) angle = 90;

                    if (cell == PACMAN_C || cell == GHOST_B_C || cell == GHOST_O_C || cell == GHOST_P_C || cell == GHOST_R_C)
                    {
                        SkinnedMeshRenderer skinnedMeshRenderer = newObject.GetComponentInChildren<SkinnedMeshRenderer>();
                        newObject.transform.RotateAround(skinnedMeshRenderer.bounds.center, new Vector3(0, 1, 0), angle);
                    }

                    if (cell == GHOST_B_C || cell == GHOST_O_C || cell == GHOST_P_C || cell == GHOST_R_C)
                    {
                        GhostAnimate animationScript = newObject.GetComponent<GhostAnimate>();
                        animationScript.SetBodyTexture(texture);
                        animationScript.Start();

                        if (cell == GHOST_B_C)
                        {
                            newObject.AddComponent<GhostBlueMove>();
                            newObject.tag = TAG_GHOST_BLUE;
                        }
                        else if (cell == GHOST_O_C)
                        {
                            newObject.AddComponent<GhostOrangeMove>();
                            newObject.tag = TAG_GHOST_ORANGE;
                        }
                        else if (cell == GHOST_P_C)
                        {
                            newObject.AddComponent<GhostPinkMove>();
                            newObject.tag = TAG_GHOST_PINK;
                        }
                        else if (cell == GHOST_R_C)
                        {
                            newObject.AddComponent<GhostRedMove>();
                            newObject.tag = TAG_GHOST_RED;
                        }
                    }
                    else if (cell == PACMAN_C)
                    {
                        FollowPacman cameraScript = cameraObject.GetComponent<Camera>().GetComponent<FollowPacman>();
                        cameraScript.SetPacman(newObject);
                        cameraScript.SetInitPosition(MAP_HEIGHT * TILE_SIZE, MAP_WIDTH * TILE_SIZE);

                        LightObject.GetComponent<Light>().transform.position = new Vector3(MAP_WIDTH * TILE_SIZE / 2, 30, MAP_HEIGHT * TILE_SIZE / 2);
                    }
                    else if (cell == WALL_H_C || cell == WALL_V_C)
                    {
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
                        else {
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
                        else {
                            texUp = texWallUp2;
                            animUp = false;
                        }

                        WallAnimate animationScript = newObject.GetComponent<WallAnimate>();
                        animationScript.SetTextures(texUp, texLat1, texLat2, texWallLatSmall);
                        animationScript.SetAnimatedWalls(animUp, animLat1, animLat2);
                        animationScript.AnimateTexture();
                    }
                }
            }
        }
    }
}
