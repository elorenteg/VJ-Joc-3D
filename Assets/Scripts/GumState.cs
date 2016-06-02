using UnityEngine;
using System.Collections;

public class GumState : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == Globals.TAG_WALL)
        {
            Destroy(this.gameObject);
        }
        if (collision.gameObject.tag == Globals.TAG_PACMAN)
        {
            PacmanMove moveScript = collision.gameObject.GetComponent<PacmanMove>();
            moveScript.GumCollision();
            Destroy(this.gameObject);
        }
    }
}
