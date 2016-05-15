using UnityEngine;
using System.Collections;

public class GhostOrangeMove : GhostMove
{
    private float startTime, duration;
    private int[] directions = { Utilities.UP, Utilities.RIGHT, Utilities.DOWN, Utilities.LEFT };
    private int currentDir = 0;
    private float incMove = 0.0f;

    public GhostOrangeMove()
    {

    }

    public void onMove(int[][] Map)
    {
        //Debug.Log("Moving Orange_GHOST" + " " + this.getBaseGhostSpeed());

        bool canMove = Utilities.rotate(this.gameObject, getBaseGhostRotateSpeed(), directions[currentDir]);
        if (canMove)
        {
            Debug.Log(incMove);
            if (incMove == Utilities.TILE_SIZE)
            {
                // ya ha llegado a su destino
                new_tx = tile_x;
                new_tz = tile_z;
                if (directions[currentDir] == Utilities.UP) new_tx = new_tx + 1;
                else if (directions[currentDir] == Utilities.RIGHT) new_tz = new_tz + 1;
                else if (directions[currentDir] == Utilities.DOWN) new_tx = new_tx - 1;
                else if (directions[currentDir] == Utilities.LEFT) new_tz = new_tz - 1;
            }

            if (new_tx >= 0 && new_tz >= 0 && new_tx < Utilities.MAP_HEIGHT && new_tz < Utilities.MAP_WIDTH && isValid(Map, new_tx, new_tz))
            {
                transform.Translate(Vector3.left.normalized * 0.2f);
                incMove += 0.2f;
            }
            else {
                currentDir = (currentDir + 1) % directions.Length;
                incMove = 0.0f;
            }

            /*
            Debug.Log(currentDir + " --> " + new_tx + " -- " + new_tz + " --- " + isValid(Map, new_tx, new_tz) + " - " + Map[new_tx][new_tz]);
            
            if (isValid(Map, new_tx, new_tz))
            {
                transform.Translate(Vector3.left * getBaseGhostSpeed() * Time.deltaTime);
                positionToTiles(GetComponentInChildren<SkinnedMeshRenderer>().bounds.center, out tile_x, out tile_z);
            }
            else currentDir = (currentDir + 1) % directions.Length;
            */
        }

        /*
        float distCovered = (Time.time - startTime) * getBaseGhostSpeed();
        float fracJourney = distCovered / duration;

        if (new_tx == GetTileX() && new_tz == GetTileZ() || fracJourney == 1.0f)
        {
            Debug.Log("NEW TILE");

            // Find new tiles
            

            startTime = Time.time;
            duration = Vector3.Distance(transform.position, GetPosition(new_tx, new_tz));
        }
        Debug.Log("MOVING");
        transform.position = Vector3.Lerp(transform.position, GetPosition(new_tx, new_tz), fracJourney);
        */
    }
}
