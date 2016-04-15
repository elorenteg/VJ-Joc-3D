using UnityEngine;
using System.Collections;

public class BallMove : MonoBehaviour {

	public float regularSpeed = 1.0f, jumpSpeed = 1.0f, turboSpeed = 1.0f, turboTime = 0.5f, turboSize = 0.5f;
	float currentSpeed;
	float radius;
	float laneX = 0.0f, vx = 0.0f;
	float starty;
	float posX = 0.0f, posY = 0.0f;

	// Use this for initialization
	void Start () {
		radius = transform.localScale.magnitude / 2.0f;
		starty = transform.position.y;
		currentSpeed = regularSpeed;
	}
	
	// Update is called once per frame
	void Update () {
		posX = 0.0f;
		posY = 0.0f;
		// Input
		if (Input.GetKey (KeyCode.LeftArrow))
			posX = 1.0f;
		if (Input.GetKey (KeyCode.RightArrow))
			posX = -1.0f;
		if (Input.GetKey (KeyCode.UpArrow))
			posY = 1.0f;
		if (Input.GetKey (KeyCode.DownArrow))
			posY = -1.0f;

		transform.Translate(posX, posY, 0.0f);
	}
}
