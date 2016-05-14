using UnityEngine;
using System.Collections;

public class GhostBlueMove : GhostMove
{

    public GhostBlueMove()
    {

    }

    public void onMove()
    {
        Debug.Log("Moving BLUE_GHOST" + this.getBaseGhostSpeed());

        transform.Translate(levelManager.getPacmanPosition().normalized * this.getBaseGhostSpeed() * Time.deltaTime);
    }

}
