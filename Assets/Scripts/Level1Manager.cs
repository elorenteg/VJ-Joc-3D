using UnityEngine;
using System.Collections;
using System.IO;
using System.Text;
using System.Collections.Generic;

public class Level1Manager : MonoBehaviour
{
    public Texture wallTexture;
    public GameObject cube;
    public GameObject floor;

    public static string appPath = "..\\VJ-Joc-3D";

    private static string fileName = appPath + ".\\Assets\\Maps\\level_1.txt";

    private List<List<int>> Map = new List<List<int>>();

    private static int CELL_EMPTY = 0;
    private static char WALL_V = 'V';
    private static int WALL_V_C = 1;
    private static char WALL_H = 'H';
    private static int WALL_H_C = 2;

    private static int numberOfExternalWalls = 4;

    private float[] externalWallsPositions = {  50.0f, 0.0f,  0.0f,
                                               -50.0f, 0.0f,  0.0f,
                                                0.0f,  0.0f, 50.0f,
                                                0.0f,  0.0f,-50.0f};

    private float[] externalWallsRotations = {  0.0f, 90.0f,  0.0f,
                                                0.0f,-90.0f,  0.0f,
                                                0.0f,  0.0f,  0.0f,
                                                0.0f,  0.0f,  0.0f};

    private float[] externalWallsScale = { 102.0f, 10.0f, 2.0f };

    private static int numberOfInternalWalls = 4;

    private float[] internalWallsPositions = {  25.0f, 0.0f, 25.0f,
                                               -25.0f, 0.0f, 25.0f,
                                               -25.0f, 0.0f,-25.0f,
                                                25.0f, 0.0f,-25.0f};

    private float[] internalWallsRotations = {  0.0f, 0.0f, 0.0f,
                                                0.0f, 0.0f, 0.0f,
                                                0.0f, 0.0f, 0.0f,
                                                0.0f, 0.0f, 0.0f};

    private float[] internalWallsScaleLittle = { 10.0f, 10.0f, 2.0f };
    private float[] internalWallsScaleLarge = { 10.0f, 10.0f, 2.0f };

    void Start()
    {
        readMap();
        placeFloor();
        placeWalls();
        //PlaceWallsOld();
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
                // While there's lines left in the text file, do this:
                do
                {
                    line = theReader.ReadLine();

                    if (line != null)
                    {
                        List<int> MapLine = new List<int>();
                        for (int i = 0; i < line.Length; ++i)
                        {
                            if (line[i] == WALL_V)
                            {
                                MapLine.Add(WALL_V_C);
                            }
                            else if (line[i] == WALL_H)
                            {
                                MapLine.Add(WALL_H_C);
                            }
                            else
                            {
                                MapLine.Add(CELL_EMPTY);
                            }
                        }
                        Map.Add(MapLine);
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
        float mapHeight = Map.Count;
        float mapWidth = Map[0].Count;
        Vector3 floorPosition = new Vector3(mapHeight, 0.0f, mapWidth);
        Vector3 floorRotation = new Vector3(0.0f, 0.0f, 0.0f);
        Vector3 floorScale = new Vector3(mapHeight*2, 1.0f, mapWidth*2);

        GameObject newFloor = Instantiate(floor, floorPosition, Quaternion.Euler(floorRotation)) as GameObject;
        newFloor.transform.localScale = floorScale;
    }

    void placeWalls()
    {
        for (int i = 0; i < Map.Count; ++i)
        {
            for (int j = 0; j < Map[i].Count; ++j)
            {
                int cell = Map[i][j];
                if (cell != CELL_EMPTY)
                {
                    Vector3 cellPosition;
                    Vector3 cellRotation;
                    Vector3 cellScale;
                    Texture texture;
                    Vector2 textureScale;

                    if (cell == WALL_V_C)
                    {
                        cellPosition = new Vector3(i * 2, 0, j * 2);
                        cellRotation = new Vector3(0.0f, 0.0f, 0.0f);
                        cellScale = new Vector3(2.0f, 15.0f, 2.5f);
                        texture = wallTexture;
                        textureScale = new Vector2(0.5f, 1.0f);
                    }
                    else if (cell == WALL_H_C)
                    {
                        cellPosition = new Vector3(i * 2, 0, j * 2);
                        cellRotation = new Vector3(0.0f, 90.0f, 0.0f);
                        cellScale = new Vector3(2.0f, 15.0f, 2.5f);
                        texture = wallTexture;
                        textureScale = new Vector2(0.5f, 1.0f);
                    }
                    else
                    {
                        Debug.LogError("Creating a non empty cell");
                        cellPosition = new Vector3(0, 0, 0);
                        cellRotation = new Vector3(0.0f, 0.0f, 0.0f);
                        cellScale = new Vector3(0.0f, 0.0f, 0.0f);
                        texture = wallTexture;
                        textureScale = new Vector2(0.0f, 0.0f);
                    }

                    GameObject newObject = Instantiate(cube, cellPosition, Quaternion.Euler(cellRotation)) as GameObject;
                    newObject.transform.localScale = cellScale;

                    Renderer rend = newObject.GetComponent<Renderer>();
                    rend.material.mainTexture = texture;
                    rend.material.mainTextureScale = textureScale;
                }
            }
        }
    }

    void PlaceWallsOld()
    {
        // Placing external walls
        for (int i = 0; i < numberOfExternalWalls; ++i)
        {
            GameObject newObject = Instantiate(cube, getExternalWallPosition(i * 3), getExternalWallRotation(i * 3)) as GameObject;
            newObject.transform.localScale = getExternalWallScale();

            Renderer rend = newObject.GetComponent<Renderer>();
            rend.material.mainTexture = wallTexture;
            rend.material.mainTextureScale = new Vector2(3.0f, 1.0f);
        }

        // Placing internal walls
        for (int i = 0; i < numberOfInternalWalls; ++i)
        {
            GameObject newObject = Instantiate(cube, getInternalWallPosition(i * 3), getInternalWallRotation(i * 3)) as GameObject;
            newObject.transform.localScale = getInternalWallScale();

            Renderer rend = newObject.GetComponent<Renderer>();
            rend.material.mainTexture = wallTexture;
            rend.material.mainTextureScale = new Vector2(0.4f, 1.0f);
        }
    }

    Vector3 getExternalWallPosition(int pos)
    {
        float x = 0, y = 0, z = 0;

        x = externalWallsPositions[pos];
        y = externalWallsPositions[pos + 1];
        z = externalWallsPositions[pos + 2];

        return new Vector3(x, y, z);
    }

    Quaternion getExternalWallRotation(int pos)
    {
        float x = 0, y = 0, z = 0;

        x = externalWallsRotations[pos];
        y = externalWallsRotations[pos + 1];
        z = externalWallsRotations[pos + 2];

        return Quaternion.Euler(new Vector3(x, y, z));
    }

    Vector3 getExternalWallScale()
    {
        float x = 0, y = 0, z = 0;

        x = externalWallsScale[0];
        y = externalWallsScale[1];
        z = externalWallsScale[2];

        return new Vector3(x, y, z);
    }

    Vector3 getInternalWallPosition(int pos)
    {
        float x = 0, y = 0, z = 0;

        x = internalWallsPositions[pos];
        y = internalWallsPositions[pos + 1];
        z = internalWallsPositions[pos + 2];

        return new Vector3(x, y, z);
    }

    Quaternion getInternalWallRotation(int pos)
    {
        float x = 0, y = 0, z = 0;

        x = internalWallsRotations[pos];
        y = internalWallsRotations[pos + 1];
        z = internalWallsRotations[pos + 2];

        return Quaternion.Euler(new Vector3(x, y, z));
    }

    Vector3 getInternalWallScale()
    {
        float x = 0, y = 0, z = 0;

        x = internalWallsScaleLittle[0];
        y = internalWallsScaleLittle[1];
        z = internalWallsScaleLittle[2];

        return new Vector3(x, y, z);
    }
}
