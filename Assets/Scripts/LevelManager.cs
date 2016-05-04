using UnityEngine;
using System.Collections;


public class LevelManager : MonoBehaviour {

    private string currentScore;
    private string currentLifes;

    void Start () {
	
	}
	
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Escape) == true)
        {
            Application.Quit();
        }
    }
}
