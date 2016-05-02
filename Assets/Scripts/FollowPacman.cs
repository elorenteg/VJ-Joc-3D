using UnityEngine;
using System.Collections;

public class FollowPacman : MonoBehaviour
{
    public float smooth = 5.0f;

    private GameObject pacman = null;

    public void Start()
    {
    }

    public void SetPacman(GameObject player)
    {
        pacman = player;

        Update();

        transform.rotation = Quaternion.Euler(50, 0, 0);
    }

    public void SetInitPosition(int h, int w)
    {
        Vector3 cameraPosition = new Vector3(h/2, 30, w/2);

        transform.position = cameraPosition;
        //transform.position = Vector3.Lerp(transform.position, cameraPosition, Time.deltaTime * smooth);
    }

    private void Update()
    {
        if (pacman != null)
        {
            Vector3 pacmanPosition = pacman.GetComponentInChildren<SkinnedMeshRenderer>().bounds.center;
            Vector3 cameraPosition = new Vector3(0, 30, -30);
            transform.position = Vector3.Lerp(transform.position, pacmanPosition + cameraPosition, Time.deltaTime * smooth);
        }
    }
    /*

    private void LateUpdate()
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
    */
}
