using UnityEngine;
using System.Collections;

public class FollowBall : MonoBehaviour {

	public GameObject ball;
	Vector3 relativePos;

	// Use this for initialization
	void Start () {
		relativePos = transform.position - ball.transform.position;
	}
	
	// Update is called once per frame
	void LateUpdate () {
		transform.position = ball.transform.position + relativePos;
	}
}
