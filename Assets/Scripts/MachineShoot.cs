using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class MachineShoot : MonoBehaviour {
    public GameObject gum;

    private int MAX_TIME_BETWEEN_GUMS = 2;
    private float timeLastGum;

    private int shootDirection;

    private List<GameObject> shoots;

    private LevelManager levelManager;

    void Start () {
        timeLastGum = MAX_TIME_BETWEEN_GUMS;
        shoots = new List<GameObject>();

        GameObject gameManager = GameObject.Find("GameManager");
        levelManager = gameManager.GetComponent<LevelManager>();
    }

    public void SetShootDirection(int dir)
    {
        shootDirection = dir;
    }
	
	void Update () {
        if (!levelManager.getGamePaused())
        {
            timeLastGum -= Time.deltaTime;

            MoveShoots();

            if (timeLastGum <= 0)
            {
                timeLastGum = MAX_TIME_BETWEEN_GUMS;
                ShootGum();
            }
        }
	}

    void ShootGum()
    {
        Debug.Log("GUM");

        Vector3 cellPosition = transform.position;
        cellPosition.y = 5.0f;
        if (shootDirection == Globals.RIGHT) cellPosition.x += 4;
        else cellPosition.x -= 4;
        Vector3 cellScale = new Vector3(2.5f, 2.5f, 2.5f);

        GameObject newObject = Instantiate(gum, cellPosition, gum.transform.rotation) as GameObject;
        newObject.SetActive(true);
        newObject.transform.localScale = cellScale;
        //newObject.transform.parent = transform;
        newObject.tag = Globals.TAG_GUM;

        MeshRenderer rend = newObject.GetComponent<MeshRenderer>();
        float rand = Random.value;
        if (rand <= 0.25f) rend.material.SetColor("_Color", Color.red);
        else if (rand <= 0.5f) rend.material.SetColor("_Color", Color.blue);
        else if (rand <= 0.5f) rend.material.SetColor("_Color", Color.green);
        else rend.material.SetColor("_Color", Color.yellow);

        shoots.Add(newObject);
    }

    void MoveShoots()
    {
        List<int> removeIDShoots = new List<int>();
        for (int i = 0; i < shoots.Count; ++i)
        {
            GameObject shoot = shoots[i];

            if (shoot == null) removeIDShoots.Add(i);
            else
            {
                float inc = 1.0f;
                if (shootDirection == Globals.LEFT) inc = -1.0f;

                Vector3 newPosition = shoot.transform.position;
                newPosition.x += inc;
                shoot.transform.position = newPosition;
            }
        }

        for (int i = removeIDShoots.Count - 1; i >= 0; --i)
        {
            shoots.RemoveAt(removeIDShoots[i]);
        }
    }
}
