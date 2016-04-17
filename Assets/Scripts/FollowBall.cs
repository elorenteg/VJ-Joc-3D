using UnityEngine;
using System.Collections;

public class FollowBall : MonoBehaviour
{

    public GameObject ball;

    enum CubeFace
    {
        None,
        Up,
        Down,
        East,
        West,
        North,
        South
    };
    CubeFace actualFace;

    void Start()
    {
        actualFace = CubeFace.Up;
    }

    void LateUpdate()
    {
        Vector3 ballPosition = ball.transform.position;

        Vector3 baseRotation = new Vector3(90.0f, 0.0f, 0.0f);
        Vector3 perspecRotation = new Vector3(0.0f, 0.0f, 0.0f);
        Vector3 basePosition = new Vector3(0.0f, 100.0f, 0.0f);
        Vector3 perspecPosition = new Vector3(0.0f, 0.0f, 0.0f);

        actualFace = checkActualCubeFace(ballPosition);
        switch (actualFace)
        {
            case CubeFace.Up:
                baseRotation = new Vector3(90.0f, 0.0f, 0.0f);
                perspecRotation = new Vector3(-20.0f, 0.0f, 0.0f);
                basePosition = new Vector3(0.0f, 100.0f, 0.0f);
                perspecPosition = new Vector3(0.0f, 50.0f, -40.0f);
                break;
            case CubeFace.Down:
                baseRotation = new Vector3(-90.0f, 0.0f, 0.0f);
                perspecRotation = new Vector3(-20.0f, 0.0f, 0.0f);
                basePosition = new Vector3(0.0f, -100.0f, 0.0f);
                perspecPosition = new Vector3(0.0f, -50.0f, 40.0f);
                break;
            case CubeFace.East:
                baseRotation = new Vector3(0.0f, -90.0f, -90.0f);
                perspecRotation = new Vector3(0.0f, 20.0f, 0.0f);
                basePosition = new Vector3(100.0f, 0.0f, 0.0f);
                perspecPosition = new Vector3(50.0f, 0.0f, -40.0f);
                break;
            case CubeFace.West:
                baseRotation = new Vector3(0.0f, 90.0f, 90.0f);
                perspecRotation = new Vector3(0.0f, -20.0f, 0.0f);
                basePosition = new Vector3(-100.0f, 0.0f, 0.0f);
                perspecPosition = new Vector3(-50.0f, 0.0f, -40.0f);
                break;
            case CubeFace.North:
                baseRotation = new Vector3(0.0f, 180.0f, 180.0f);
                perspecRotation = new Vector3(20.0f, 0.0f, 0.0f);
                basePosition = new Vector3(0.0f, 0.0f, 100.0f);
                perspecPosition = new Vector3(0.0f, 40.0f, 50.0f);
                break;
            case CubeFace.South:
                baseRotation = new Vector3(0.0f, 0.0f, 0.0f);
                perspecRotation = new Vector3(-20.0f, 0.0f, 0.0f);
                basePosition = new Vector3(0.0f, 0.0f, -100.0f);
                perspecPosition = new Vector3(0.0f, -40.0f, -50.0f);
                break;
            case CubeFace.None:
                baseRotation = new Vector3(0.0f, 0.0f, 0.0f);
                perspecRotation = new Vector3(0.0f, 0.0f, 0.0f);
                basePosition = new Vector3(0.0f, 0.0f, 0.0f);
                perspecPosition = new Vector3(0.0f, 0.0f, 0.0f);
                break;
        }

        if (actualFace != CubeFace.None)
        {
            //transform.position = basePosition + perspecPosition + ball.transform.position;
            transform.position = basePosition + perspecPosition;
            transform.rotation = Quaternion.Euler(baseRotation + perspecRotation);

            Debug.Log("BallPosition: " + ballPosition + "ActualFace:" + actualFace);
        }
    }

    CubeFace checkActualCubeFace(Vector3 actualPosition)
    {
        if (actualPosition.x > -50.0f && actualPosition.x < 50.0f)
        {
            if (actualPosition.y > 50.0f)
            {
                return CubeFace.Up;
            }
            else if (actualPosition.y < -50.0f)
            {
                return CubeFace.Down;
            }
            else
            {
                if (actualPosition.z > 50.0f)
                {
                    return CubeFace.North;
                }
                else if (actualPosition.z < -50)
                {
                    return CubeFace.South;
                }
            }
        }
        else if (actualPosition.x < -50.0f)
        {
            return CubeFace.West;
        }
        else if (actualPosition.x > 50.0f)
        {
            return CubeFace.East;
        }

        return CubeFace.None;
    }
}
