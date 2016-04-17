using UnityEngine;
using System.Collections;

public class FollowBall : MonoBehaviour
{

    public GameObject ball;
    public GameObject cube;

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
    CubeFace lastFace;
    CubeFace actualFace;

    void Start()
    {
        lastFace = CubeFace.Up;
        actualFace = CubeFace.Up;
    }

    void LateUpdate()
    {
        Vector3 baseRotation = new Vector3(90.0f, 0.0f, 0.0f);
        Vector3 perspecRotation = new Vector3(0.0f, 0.0f, 0.0f);
        Vector3 basePosition = new Vector3(0.0f, 100.0f, 0.0f);
        Vector3 perspecPosition = new Vector3(0.0f, 0.0f, 0.0f);

        actualFace = checkActualCubeFace(ball.transform.position);
        switch (actualFace)
        {
            case CubeFace.Up:
                //baseRotation = new Vector3(90.0f, 0.0f, 0.0f);
                //perspecRotation = new Vector3(-20.0f, 0.0f, 0.0f);
                basePosition = new Vector3(0.0f, 100.0f, 0.0f);
                perspecPosition = new Vector3(0.0f, 50.0f, -40.0f);
                break;
            case CubeFace.Down:
                //baseRotation = new Vector3(-90.0f, 0.0f, 0.0f);
                //perspecRotation = new Vector3(-20.0f, 0.0f, 0.0f);
                basePosition = new Vector3(0.0f, -100.0f, 0.0f);
                perspecPosition = new Vector3(0.0f, -50.0f, 40.0f);
                break;
            case CubeFace.East:
                //baseRotation = new Vector3(0.0f, -90.0f, -90.0f);
                //perspecRotation = new Vector3(0.0f, 20.0f, 0.0f);
                basePosition = new Vector3(100.0f, 0.0f, 0.0f);
                perspecPosition = new Vector3(50.0f, 0.0f, -40.0f);
                break;
            case CubeFace.West:
                //baseRotation = new Vector3(0.0f, 90.0f, 90.0f);
                //perspecRotation = new Vector3(0.0f, -20.0f, 0.0f);
                basePosition = new Vector3(-100.0f, 0.0f, 0.0f);
                perspecPosition = new Vector3(-50.0f, 0.0f, -40.0f);
                break;
            case CubeFace.North:
                //baseRotation = new Vector3(0.0f, 180.0f, 180.0f);
                //perspecRotation = new Vector3(20.0f, 0.0f, 0.0f);
                basePosition = new Vector3(0.0f, 0.0f, 100.0f);
                perspecPosition = new Vector3(0.0f, 40.0f, 50.0f);
                break;
            case CubeFace.South:
                //baseRotation = new Vector3(0.0f, 0.0f, 0.0f);
                //perspecRotation = new Vector3(-20.0f, 0.0f, 0.0f);
                basePosition = new Vector3(0.0f, 0.0f, -100.0f);
                perspecPosition = new Vector3(0.0f, -40.0f, -50.0f);
                break;
            case CubeFace.None:
                //baseRotation = new Vector3(0.0f, 0.0f, 0.0f);
                //perspecRotation = new Vector3(0.0f, 0.0f, 0.0f);
                basePosition = new Vector3(0.0f, 0.0f, 0.0f);
                perspecPosition = new Vector3(0.0f, 0.0f, 0.0f);
                break;
        }

        if (actualFace != CubeFace.None)
        {
            //transform.position = basePosition + ball.transform.position;
            //transform.position = basePosition + perspecPosition;
            transform.position = Vector3.Lerp(this.transform.position, basePosition + perspecPosition, 0.05f);
            //transform.rotation = Quaternion.Euler(baseRotation + perspecRotation);
        }
        
        transform.LookAt(cube.transform);

        lastFace = actualFace;

        Debug.Log("BallPosition: " + ball.transform.position + "ActualFace:" + actualFace);
    }

    bool isCameraCentered(Vector3 actualPosition, CubeFace actualFace)
    {
        switch (actualFace)
        {
            case CubeFace.Up:
                return actualPosition.y > 100;

            case CubeFace.Down:
                return actualPosition.y < -100;
                
            case CubeFace.East:
                return actualPosition.x > 100;
                
            case CubeFace.West:
                return actualPosition.x < -100;
                
            case CubeFace.North:
                return actualPosition.z > 100;

            case CubeFace.South:
                return actualPosition.z < -100;

            case CubeFace.None:
                return false;
        }

        return false;
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
