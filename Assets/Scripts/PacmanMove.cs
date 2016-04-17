using UnityEngine;
using System.Collections;

public class PacmanMove : MonoBehaviour {
    private int state;
    private int MAX_STATE = 5;

	// Use this for initialization
	void Start () {
        state = 0;
	}
	
	// Update is called once per frame
	void Update () {
        float posX, posY, posZ;
        posX = 0.0f;
        posY = 0.0f;
        posZ = 0.0f;

        // Input
        bool move = false;
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) {
            posX = -1.0f;
            move = true;
        }
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) {
            posX = 1.0f;
            move = true;
        }
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)) {
            posZ = 1.0f;
            move = true;
        }
        if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S)) {
            posZ = -1.0f;
            move = true;
        }

        if (move) {
            GetComponent<Animation>().Play("ArmatureActionPlayer", PlayMode.StopAll);
            state = (state++) % MAX_STATE;
        }
        transform.Translate(posX, posY, posZ);
    }
}
