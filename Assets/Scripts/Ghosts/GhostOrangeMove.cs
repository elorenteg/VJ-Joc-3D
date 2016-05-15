using UnityEngine;
using System.Collections;

public class GhostOrangeMove : GhostMove
{
    private float startTime, duration;
    private static int[] directions = { Globals.UP, Globals.RIGHT, Globals.DOWN, Globals.LEFT };
    private int currentDir;
    private float incMove;

    public GhostOrangeMove()
    {
        currentDir = 0;
        incMove = 0.0f;
    }

    public void SetDirection(int[][] Map)
    {
        newTileX = tileX;
        newTileZ = tileZ;

        if (directions[currentDir] == Globals.UP) newTileX = newTileX + 1;
        else if (directions[currentDir] == Globals.RIGHT) newTileZ = newTileZ + 1;
        else if (directions[currentDir] == Globals.DOWN) newTileX = newTileX - 1;
        else if (directions[currentDir] == Globals.LEFT) newTileZ = newTileZ - 1;

        Debug.Log("ME (" + tileX + "," + tileZ + ")" + " - " + Map[tileX][tileZ] + " -- New (" + newTileX + "," + newTileZ + ")");
    }

    public void onMove(int[][] Map)
    {
        Debug.Log("Moving Orange_GHOST" + " " + this.getBaseGhostSpeed());

        bool ghostCanMove = rotate(directions[currentDir]);

        if (ghostCanMove) currentDir = (currentDir + 1) % directions.Length;

        /*
        bool canMove = Globals.rotate(this.gameObject, getBaseGhostRotateSpeed(), directions[currentDir]);
        if (canMove)
        {
            Debug.Log(incMove);
            if (incMove == Globals.TILE_SIZE)
            {
                // ya ha llegado a su destino
                int aux_tx = new_tx;
                int aux_tz = new_tz;

                SetDirection(Map);

                tile_x = aux_tx;
                tile_z = aux_tz;
            }

            if (new_tx >= 0 && new_tz >= 0 && new_tx < Globals.MAP_HEIGHT && new_tz < Globals.MAP_WIDTH && isValid(Map, new_tx, new_tz))
            {
                transform.Translate(Vector3.left.normalized * 0.2f);
                incMove += 0.2f;
            }
            else
            {
                currentDir = (currentDir + 1) % directions.Length;
                incMove = 0.0f;
            }
            */

        /*
        Debug.Log(currentDir + " --> " + new_tx + " -- " + new_tz + " --- " + isValid(Map, new_tx, new_tz) + " - " + Map[new_tx][new_tz]);

        if (isValid(Map, new_tx, new_tz))
        {
            transform.Translate(Vector3.left * getBaseGhostSpeed() * Time.deltaTime);
            positionToTiles(GetComponentInChildren<SkinnedMeshRenderer>().bounds.center, out tile_x, out tile_z);
        }
        else currentDir = (currentDir + 1) % directions.Length;
        */
        // }

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
