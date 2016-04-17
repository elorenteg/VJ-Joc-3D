using UnityEngine;
using System.Collections;

public class FollowBall : MonoBehaviour
{
    public GameObject player;

    void Start()
    {

    }

    void LateUpdate()
    {
        Vector3 baseRotation = new Vector3(90.0f, 0.0f, 0.0f);
        Vector3 perspecRotation = new Vector3(0.0f, 0.0f, 0.0f);
        Vector3 basePosition = new Vector3(0.0f, 100.0f, 0.0f);
        Vector3 perspecPosition = new Vector3(0.0f, 0.0f, 0.0f);

        //transform.position = basePosition + ball.transform.position;
        //transform.position = basePosition + perspecPosition;
        //transform.position = Vector3.Lerp(this.transform.position, basePosition + perspecPosition, 0.05f);
        //transform.rotation = Quaternion.Euler(baseRotation + perspecRotation);

        //transform.LookAt(cube.transform);
    }
}
