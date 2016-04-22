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

    public static string appPath = "..\\VJ-Joc-3D";

    private static string fileName = appPath + ".\\Assets\\Maps\\level_1.txt";

    private int[][] Map;
    private int MAP_WIDTH;
    private int MAP_HEIGHT;

    private float WALL_HEIGHT = 7.5f;

    private static int CELL_EMPTY = 0;
    private static char WALL_V = 'V';
    private static int WALL_V_C = 1;
    private static char WALL_H = 'H';
    private static int WALL_H_C = 2;

    private const int TILE_SIZE = 2;

    void Start()
    {
        readMap();
        placeFloor();
        placeWalls();
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
    }

    void placeWalls()
    {
        for (int i = 0; i < MAP_HEIGHT; ++i)
        {
            for (int j = 0; j < MAP_WIDTH; ++j)
            {
                int cell = Map[i][j];
                if (cell != CELL_EMPTY)
                {
                    Vector3 cellPosition;
                    Vector3 cellScale;
                    Texture texture;
                    Vector2 textureScale;
                    Quaternion cellQuaternion;

                    if (cell == WALL_V_C)
                    {
                        cellPosition = new Vector3(j * TILE_SIZE, WALL_HEIGHT/2, i * TILE_SIZE);
                        cellScale = new Vector3(2.0f, WALL_HEIGHT, 2.5f);
                        texture = wallTexture;
                        textureScale = new Vector2(0.5f, 1.0f);

                        cellQuaternion = Quaternion.AngleAxis(90.0f, Vector3.up);
                    }
                    else if (cell == WALL_H_C)
                    {
                        cellPosition = new Vector3(j * TILE_SIZE, WALL_HEIGHT/2, i * TILE_SIZE);
                        cellScale = new Vector3(2.0f, WALL_HEIGHT, 2.5f);
                        texture = wallTexture;
                        textureScale = new Vector2(0.5f, 1.0f);

                        cellQuaternion = Quaternion.AngleAxis(0.0f, Vector3.up);
                    }
                    else
                    {
                        Debug.LogError("Creating a non empty cell");
                        cellPosition = new Vector3(0, 0, 0);
                        cellScale = new Vector3(0.0f, 0.0f, 0.0f);
                        texture = wallTexture;
                        textureScale = new Vector2(0.0f, 0.0f);

                        cellQuaternion = Quaternion.AngleAxis(0.0f, Vector3.up);
                    }

                    GameObject newObject = Instantiate(cube, cellPosition, cube.transform.rotation) as GameObject;
                    newObject.transform.parent = transform;
                    newObject.transform.localScale = cellScale;

                    //Renderer rend = newObject.GetComponent<Renderer>();
                    //rend.material.mainTexture = texture;
                    //rend.material.mainTextureScale = textureScale;
                }
            }
        }
    }
}
