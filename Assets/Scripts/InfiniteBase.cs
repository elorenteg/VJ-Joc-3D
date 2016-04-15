using UnityEngine;
using System.Collections;

public class InfiniteBase : MonoBehaviour {

	public GameObject ball;
	float relativePos;

	// Use this for initialization
	void Start () {
		relativePos = transform.position.z - ball.transform.position.z;
	}
	
	// Update is called once per frame
	void LateUpdate () {
		transform.position = new Vector3(transform.position.x, transform.position.y, Mathf.Floor(ball.transform.position.z + relativePos));
	}
}
