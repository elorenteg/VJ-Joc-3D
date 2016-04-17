using UnityEngine;
using System.Collections;

public class Level1Manager : MonoBehaviour
{
    private static int numberOfExternalWalls = 4;

    private float[] externalWallsPositions = {  50.0f, 0.0f,  0.0f,
                                               -50.0f, 0.0f,  0.0f,
                                                0.0f,  0.0f, 50.0f,
                                                0.0f,  0.0f,-50.0f};

    private float[] externalWallsRotations = {  0.0f, 90.0f,  0.0f,
                                                0.0f,-90.0f,  0.0f,
                                                0.0f,  0.0f,  0.0f,
                                                0.0f,  0.0f,  0.0f};

    private float[] externalWallsScale = {102.0f, 10.0f, 2.0f};

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
        PlaceWalls();
    }

    void Update()
    {

    }

    void PlaceWalls()
    {
        // Placing external walls
        for (int i = 0; i < numberOfExternalWalls; ++i)
        {
            GameObject newObject = Instantiate(cube, getExternalWallPosition(i * 3), getExternalWallRotation(i * 3)) as GameObject;
            newObject.transform.localScale = getExternalWallScale();
        }

        // Placing internal walls
        for (int i = 0; i < numberOfInternalWalls; ++i)
        {
            GameObject newObject = Instantiate(cube, getInternalWallPosition(i * 3), getInternalWallRotation(i * 3)) as GameObject;
            newObject.transform.localScale = getInternalWallScale();
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
