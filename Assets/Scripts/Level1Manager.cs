using UnityEngine;
using System.Collections;
using System.IO;
using System.Text;
using System.Collections.Generic;

public class Level1Manager : MonoBehaviour
{
    public GameObject cube;
    public GameObject floor;
    public Texture wallTexture;
    public GameObject pacman;
    public GameObject ghost;
    public GameObject coin;

    public static string appPath = "..\\VJ-Joc-3D";

    private static string fileName = appPath + ".\\Assets\\Maps\\level_1.txt";

    private const int TILE_SIZE = 2;
    private int[][] Map;
    private int MAP_WIDTH;
    private int MAP_HEIGHT;

    private static int CELL_EMPTY = 0;
    private static char WALL_V = 'V';
    private static int WALL_V_C = 1;
    private static char WALL_H = 'H';
    private static int WALL_H_C = 2;
    private static char GHOST_B = 'B';
    private static int GHOST_B_C = 3;
    private static char GHOST_O = 'O';
    private static int GHOST_O_C = 4;
    private static char GHOST_P = 'P';
    private static int GHOST_P_C = 5;
    private static char GHOST_R = 'R';
    private static int GHOST_R_C = 6;
    private static char PACMAN = '+';
    private static int PACMAN_C = 10;

    private static char COIN = '.';
    private static int COIN_C = 20;
    private static char BONUS = '*';
    private static int BONUS_C = 21;

    private float WALL_HEIGHT = 7.5f;
    private float GHOST_Y_POS = 18.5f;
    private float PACMAN_Y_POS = 18.0f;
    private float COIN_Y_POS = 14.0f;
    private float BONUS_Y_POS = 18.0f;

    private Vector3 GHOST_SCALE = new Vector3(6.0f, 6.0f, 6.0f);
    private Vector2 GHOST_TEXTURE_SCALE = new Vector2(0.5f, 1.0f);

    private Vector3 PACMAN_SCALE = new Vector3(6.0f, 6.0f, 6.0f);
    private Vector2 PACMAN_TEXTURE_SCALE = new Vector2(0.5f, 1.0f);

    private Vector3 COIN_SCALE = new Vector3(6.0f, 6.0f, 6.0f);
    private Vector2 COIN_TEXTURE_SCALE = new Vector2(0.5f, 1.0f);

    private Vector3 BONUS_SCALE = new Vector3(6.0f, 6.0f, 6.0f);
    private Vector2 BONUS_TEXTURE_SCALE = new Vector2(0.5f, 1.0f);

    void Start()
    {
        readMap();
        placeFloor();
        placeObjects();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) == true)
        {
            Application.Quit();
        }
    }

    bool readMap()
    {
        try
        {
            string line;
            StreamReader theReader = new StreamReader(fileName, Encoding.Default);
            using (theReader)
            {
                MAP_HEIGHT = File.ReadAllLines(fileName).Length;

                Map = new int[MAP_HEIGHT][];

                // While there's lines left in the text file, do this:
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

    void placeFloor()
    {
        Vector3 floorPosition = new Vector3(MAP_HEIGHT, 0.0f, MAP_WIDTH);
        Vector3 floorRotation = new Vector3(0.0f, 0.0f, 0.0f);
        Vector3 floorScale = new Vector3(MAP_HEIGHT * 2, 1.0f, MAP_WIDTH * 2);

        GameObject newFloor = Instantiate(floor, floorPosition, Quaternion.Euler(floorRotation)) as GameObject;
        newFloor.transform.localScale = floorScale;

        newFloor.SetActive(true);
    }

    void placeObjects()
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
                        element = cube;
                        cellPosition = new Vector3(j * TILE_SIZE, WALL_HEIGHT / 2, i * TILE_SIZE);
                        cellScale = new Vector3(2.0f, WALL_HEIGHT, 2.5f);
                        texture = wallTexture;
                        textureScale = new Vector2(0.5f, 1.0f);

                        cellQuaternion = Quaternion.AngleAxis(90.0f, Vector3.up);
                    }
                    else if (cell == WALL_H_C)
                    {
                        element = cube;
                        cellPosition = new Vector3(j * TILE_SIZE, WALL_HEIGHT / 2, i * TILE_SIZE);
                        cellScale = new Vector3(2.0f, WALL_HEIGHT, 2.5f);
                        texture = wallTexture;
                        textureScale = new Vector2(0.5f, 1.0f);

                        cellQuaternion = Quaternion.AngleAxis(0.0f, Vector3.up);
                    }
                    else if (cell == GHOST_B_C)
                    {
                        element = ghost;
                        cellPosition = new Vector3(j * TILE_SIZE, GHOST_Y_POS, i * TILE_SIZE);
                        cellScale = new Vector3(GHOST_SCALE.x, GHOST_SCALE.y, GHOST_SCALE.z);
                        texture = null;
                        textureScale = new Vector2(GHOST_TEXTURE_SCALE.x, GHOST_TEXTURE_SCALE.y);

                        cellQuaternion = Quaternion.AngleAxis(0.0f, Vector3.up);

                        cellPosition.x += 11 * TILE_SIZE;
                        cellPosition.z += 13.5f * TILE_SIZE;
                    }
                    else if (cell == GHOST_O_C)
                    {
                        element = ghost;
                        cellPosition = new Vector3(j * TILE_SIZE, GHOST_Y_POS, i * TILE_SIZE);
                        cellScale = new Vector3(GHOST_SCALE.x, GHOST_SCALE.y, GHOST_SCALE.z);
                        texture = null;
                        textureScale = new Vector2(GHOST_TEXTURE_SCALE.x, GHOST_TEXTURE_SCALE.y);

                        cellQuaternion = Quaternion.AngleAxis(0.0f, Vector3.up);

                        cellPosition.x += 11 * TILE_SIZE;
                        cellPosition.z += 13.5f * TILE_SIZE;
                    }
                    else if (cell == GHOST_P_C)
                    {
                        element = ghost;
                        cellPosition = new Vector3(j * TILE_SIZE, GHOST_Y_POS, i * TILE_SIZE);
                        cellScale = new Vector3(GHOST_SCALE.x, GHOST_SCALE.y, GHOST_SCALE.z);
                        texture = null;
                        textureScale = new Vector2(GHOST_TEXTURE_SCALE.x, GHOST_TEXTURE_SCALE.y);

                        cellQuaternion = Quaternion.AngleAxis(0.0f, Vector3.up);

                        cellPosition.x += 11 * TILE_SIZE;
                        cellPosition.z += 13.5f * TILE_SIZE;
                    }
                    else if (cell == GHOST_R_C)
                    {
                        element = ghost;
                        cellPosition = new Vector3(j * TILE_SIZE, GHOST_Y_POS, i * TILE_SIZE);
                        cellScale = new Vector3(GHOST_SCALE.x, GHOST_SCALE.y, GHOST_SCALE.z);
                        texture = null;
                        textureScale = new Vector2(GHOST_TEXTURE_SCALE.x, GHOST_TEXTURE_SCALE.y);

                        cellQuaternion = Quaternion.AngleAxis(0.0f, Vector3.up);

                        cellPosition.x += 11 * TILE_SIZE;
                        cellPosition.z += 13.5f * TILE_SIZE;
                    }
                    else if (cell == PACMAN_C)
                    {
                        element = pacman;
                        cellPosition = new Vector3(j * TILE_SIZE, PACMAN_Y_POS, i * TILE_SIZE);
                        cellScale = new Vector3(PACMAN_SCALE.x, PACMAN_SCALE.y, PACMAN_SCALE.z);
                        texture = null;
                        textureScale = new Vector2(PACMAN_TEXTURE_SCALE.x, PACMAN_TEXTURE_SCALE.y);

                        cellQuaternion = Quaternion.AngleAxis(0.0f, Vector3.up);

                        //cellPosition.x += 11 * TILE_SIZE;
                        cellPosition.z -= 7.0f * TILE_SIZE;
                    }
                    else if (cell == COIN_C)
                    {
                        element = coin;
                        cellPosition = new Vector3(j * TILE_SIZE, COIN_Y_POS, i * TILE_SIZE);
                        cellScale = new Vector3(COIN_SCALE.x, COIN_SCALE.y, COIN_SCALE.z);
                        texture = null;
                        textureScale = new Vector2(COIN_TEXTURE_SCALE.x, COIN_TEXTURE_SCALE.y);

                        cellQuaternion = Quaternion.AngleAxis(0.0f, Vector3.up);
                    }
                    else if (cell == BONUS_C)
                    {
                        element = null;
                        cellPosition = new Vector3(j * TILE_SIZE, BONUS_Y_POS, i * TILE_SIZE);
                        cellScale = new Vector3(BONUS_SCALE.x, BONUS_SCALE.y, BONUS_SCALE.z);
                        texture = null;
                        textureScale = new Vector2(BONUS_TEXTURE_SCALE.x, BONUS_TEXTURE_SCALE.y);

                        cellQuaternion = Quaternion.AngleAxis(0.0f, Vector3.up);
                    }
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

                    string texPath = "Textures/ghost_blue";
                    int angle = -90;
                    if (cell == GHOST_O_C) { texPath = "Textures/ghost_orange"; angle = 90; }
                    else if (cell == GHOST_P_C) texPath = "Textures/ghost_pink";
                    else if (cell == GHOST_R_C) { texPath = "Textures/ghost_red"; angle = 90; }
                    if (cell == GHOST_B_C || cell == GHOST_O_C || cell == GHOST_P_C || cell == GHOST_R_C) {
                        GhostAnimate animationScript = newObject.GetComponent<GhostAnimate>();
                        animationScript.SetBodyTexture(texPath);
                        animationScript.Start();
                    }

                    if (cell == PACMAN_C || cell == GHOST_B_C || cell == GHOST_O_C || cell == GHOST_P_C || cell == GHOST_R_C)
                    {
                        SkinnedMeshRenderer skinnedMeshRenderer = newObject.GetComponentInChildren<SkinnedMeshRenderer>();
                        newObject.transform.RotateAround(skinnedMeshRenderer.bounds.center, new Vector3(0, 1, 0), angle);
                    }

                    //Renderer rend = newObject.GetComponent<Renderer>();
                    //rend.material.mainTexture = texture;
                    //rend.material.mainTextureScale = textureScale;
                }
            }
        }
    }
}
