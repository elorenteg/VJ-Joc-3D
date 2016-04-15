using UnityEngine;
using System.Collections;

public class ObstacleGenerator : MonoBehaviour {

	public GameObject ball;
	public float obstacleDistance = 20.0f;

	public GameObject avoidObstacle;
	public float startLocationAvoid = 20.0f, timeBetweenAvoid = 3.0f;
	float timeSincePreviousAvoid = 0.0f;

	public GameObject jumpObstacle;
	public float startLocationJump = 20.0f, timeBetweenJump = 3.0f;
	float timeSincePreviousJump = 0.0f;

	public GameObject topObstacle;
	public float startLocationTop = 20.0f, timeBetweenTop = 3.0f;
	float timeSincePreviousTop = 0.0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		timeSincePreviousAvoid -= Time.deltaTime;
		if (ball.transform.position.z > startLocationAvoid && timeSincePreviousAvoid <= 0.0f) {
			timeSincePreviousAvoid = timeBetweenAvoid;
			Vector3 pos = new Vector3(Mathf.Floor(2.99f * Random.value) - 1, avoidObstacle.transform.position.y, Mathf.Floor(ball.transform.position.z) + avoidObstacle.transform.position.z + obstacleDistance);
			GameObject obstacle = (GameObject)Instantiate(avoidObstacle, pos, avoidObstacle.transform.rotation);
			obstacle.transform.parent = transform;
		}
	
		timeSincePreviousJump -= Time.deltaTime;
		if (ball.transform.position.z > startLocationJump && timeSincePreviousJump <= 0.0f) {
			timeSincePreviousJump = timeBetweenJump;
			Vector3 pos = new Vector3(0.0f, jumpObstacle.transform.position.y, Mathf.Floor(ball.transform.position.z) + jumpObstacle.transform.position.z + obstacleDistance);
			GameObject obstacle = (GameObject)Instantiate(jumpObstacle, pos, jumpObstacle.transform.rotation);
			obstacle.transform.parent = transform;
		}

		timeSincePreviousTop -= Time.deltaTime;
		if (ball.transform.position.z > startLocationTop && timeSincePreviousTop <= 0.0f) {
			timeSincePreviousTop = timeBetweenTop;
			Vector3 pos = new Vector3(0.0f, topObstacle.transform.position.y, Mathf.Floor(ball.transform.position.z) + topObstacle.transform.position.z + obstacleDistance);
			GameObject obstacle = (GameObject)Instantiate(topObstacle, pos, topObstacle.transform.rotation);
			obstacle.transform.parent = transform;
		}

	}
}
