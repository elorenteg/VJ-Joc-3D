using UnityEngine;
using System.Collections;
using System.IO;
using System.Text;
using System.Collections.Generic;

public class LevelCreator : MonoBehaviour
{
    public GameObject cameraObject;
    public GameObject wall;
    public Material wallMaterial1;
    public Material wallMaterial2;
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
    public string actualLevel;

    public static string appPath = "..\\VJ-Joc-3D";
    private string fileName = appPath + ".\\Assets\\Maps\\";

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

    private float GHOST_Y_POS = 18.5f;
    private float PACMAN_Y_POS = 18.0f;
    private float COIN_Y_POS = 10.0f;
    private float BONUS_Y_POS = 13.0f;
    
    private const float WALL_HEIGHT = 7.5f;
    private Vector3 WALL_H_SCALE = new Vector3(4*TILE_SIZE, WALL_HEIGHT, 2*TILE_SIZE);
    private Vector3 WALL_V_SCALE = new Vector3(2*TILE_SIZE, WALL_HEIGHT, 4*TILE_SIZE);

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

    void Start()
    {
        fileName += "level_" + actualLevel + ".txt";

        readMap();
        placeFloor();
        placeObjects();
    }

    void Update()
    {
        
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
        Vector3 floorPosition = new Vector3(MAP_HEIGHT - WALL_H_SCALE.x, 0.0f, MAP_WIDTH + WALL_V_SCALE.z);
        Vector3 floorRotation = new Vector3(0.0f, 0.0f, 0.0f);
        Vector3 floorScale = new Vector3(MAP_WIDTH * TILE_SIZE, 1.0f, MAP_HEIGHT * TILE_SIZE);

        GameObject newFloor = Instantiate(floor, floorPosition, Quaternion.Euler(floorRotation)) as GameObject;
        newFloor.transform.localScale = floorScale;

        Renderer renderer = newFloor.GetComponent<Renderer>();
        renderer.material.mainTexture = floorTexture;
        renderer.material.mainTextureScale = new Vector2(MAP_WIDTH / 8, MAP_HEIGHT / 8);

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
                        element = wall;
                        cellPosition = new Vector3(j * TILE_SIZE + WALL_V_SCALE.x/2, WALL_HEIGHT / 2, i * TILE_SIZE + WALL_V_SCALE.z / 2);
                        cellScale = WALL_V_SCALE;
                        texture = null;
                        textureScale = new Vector2(0.5f, 1.0f);

                        cellQuaternion = Quaternion.AngleAxis(90.0f, Vector3.up);
                    }
                    else if (cell == WALL_H_C)
                    {
                        element = wall;
                        cellPosition = new Vector3(j * TILE_SIZE + WALL_H_SCALE.x / 2, WALL_HEIGHT / 2, i * TILE_SIZE + WALL_H_SCALE.z / 2);
                        cellScale = WALL_H_SCALE;
                        texture = null;
                        textureScale = new Vector2(0.5f, 1.0f);

                        cellQuaternion = Quaternion.AngleAxis(0.0f, Vector3.up);
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
                        if (j < MAP_WIDTH/2) cellPosition.x += TILE_SIZE / 2;
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
                    }
                    else if (cell == PACMAN_C)
                    {
                        FollowPacman cameraScript = cameraObject.GetComponent<Camera>().GetComponent<FollowPacman>();
                        cameraScript.SetPacman(newObject);
                        cameraScript.SetInitPosition(MAP_HEIGHT * TILE_SIZE, MAP_WIDTH * TILE_SIZE);
                    }
                    else if (cell == WALL_H_C || cell == WALL_V_C)
                    {
                        GameObject front = newObject.transform.FindChild("Front").gameObject;
                        GameObject back = newObject.transform.FindChild("Back").gameObject;

                        int rand = Random.Range(1, 2);
                        if (rand == 1)
                        {
                            Renderer renderer1 = front.GetComponent<Renderer>();
                            renderer1.material = wallMaterial1;
                            Renderer renderer2 = back.GetComponent<Renderer>();
                            renderer2.material = wallMaterial2;
                        }
                        else
                        {
                            Renderer renderer1 = back.GetComponent<Renderer>();
                            renderer1.material = wallMaterial1;
                            Renderer renderer2 = front.GetComponent<Renderer>();
                            renderer2.material = wallMaterial2;
                        }
                    }
                }
            }
        }
    }
}
