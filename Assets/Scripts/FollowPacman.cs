using UnityEngine;
using System.Collections;

public class FollowPacman : MonoBehaviour
{
    public float smooth = 5.0f;
    
    private SkinnedMeshRenderer skinnedMeshRenderer;

    public void Start()
    {
        skinnedMeshRenderer = null;
    }

    public void SetPacman(GameObject player)
    {
        skinnedMeshRenderer = player.GetComponentInChildren<SkinnedMeshRenderer>();

        Update();

        transform.rotation = Quaternion.Euler(50, 0, 0);
    }

    private void Update()
    {
        if (skinnedMeshRenderer != null)
        {
            Vector3 pacmanPosition = skinnedMeshRenderer.bounds.center;
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
