using UnityEngine;
using System.Collections;
using System.IO;
using System.Text;

public class Level1Manager : MonoBehaviour
{
    public Texture wallTexture;

    public static string appPath = "..\\VJ-Joc-3D";

    private static string fileName = appPath + ".\\Assets\\Maps\\level_1.txt";

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

    public GameObject cube;

    void Start()
    {
        readMap();
        PlaceWalls();
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
        Debug.Log(Application.dataPath);
        // Handle any problems that might arise when reading the text
        try
        {
            string line;
            // Create a new StreamReader, tell it which file to read and what encoding the file was saved as
            StreamReader theReader = new StreamReader(fileName, Encoding.Default);
            // Immediately clean up the reader after this block of code is done.
            // You generally use the "using" statement for potentially memory-intensive objects
            // instead of relying on garbage collection.
            using (theReader)
            {
                // While there's lines left in the text file, do this:
                do
                {
                    line = theReader.ReadLine();

                    if (line != null)
                    {
                        // Do whatever you need to do with the text line, it's a string now
                        Debug.Log(line);
                        //DoStuff(line);
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

    void PlaceWalls()
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
