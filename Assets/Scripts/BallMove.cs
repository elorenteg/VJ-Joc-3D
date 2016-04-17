using UnityEngine;
using System.Collections;

public class BallMove : MonoBehaviour {

	public float regularSpeed = 1.0f, jumpSpeed = 1.0f, turboSpeed = 1.0f, turboTime = 0.5f;
	float currentSpeed;
	float radius;
	float laneX = 0.0f, vx = 0.0f;
	float starty;
	float posX = 0.0f, posY = 0.0f, posZ = 0.0f;

	void Start () {
		radius = transform.localScale.magnitude / 2.0f;
		starty = transform.position.y;
		currentSpeed = regularSpeed;
	}
	
	void Update () {
		posX = 0.0f;
		posY = 0.0f;
        posZ = 0.0f;

		// Input
		if (Input.GetKey (KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
			posX = -1.0f;
		if (Input.GetKey (KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
			posX = 1.0f;
		if (Input.GetKey (KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
			posZ = 1.0f;
		if (Input.GetKey (KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
			posZ = -1.0f;

        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationX 
                                              | RigidbodyConstraints.FreezeRotationY
                                              | RigidbodyConstraints.FreezeRotationZ;
        transform.Translate(posX*currentSpeed, posY*currentSpeed, posZ*currentSpeed);
    }
}
