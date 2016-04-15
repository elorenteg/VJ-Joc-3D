using UnityEngine;
using System.Collections;

public class ObstacleBehaviour : MonoBehaviour {

	GameObject ball;

	// Use this for initialization
	void Start () {
		ball = GameObject.Find ("Ball");
	}
	
	// Update is called once per frame
	void Update () {
		if (ball.transform.position.z > transform.position.z + 4.0f)
			Destroy (gameObject);
	}

}
