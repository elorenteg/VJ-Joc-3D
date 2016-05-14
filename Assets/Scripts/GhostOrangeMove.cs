using UnityEngine;
using System.Collections;

public class GhostOrangeMove : GhostMove
{

    public GhostOrangeMove()
    {

    }

    public void onMove()
    {
        Debug.Log("Moving Orange_GHOST" + this.getBaseGhostSpeed());

        transform.Translate(Vector3.left * this.getBaseGhostSpeed() * Time.deltaTime);
    }
}
