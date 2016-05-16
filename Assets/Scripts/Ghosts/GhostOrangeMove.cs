using UnityEngine;
using System.Collections;

public class GhostOrangeMove : GhostMove
{
    protected static int[] directions = { Globals.UP, Globals.RIGHT, Globals.DOWN, Globals.LEFT };
    protected static int[] directionsGoingOutBase = { Globals.LEFT, Globals.LEFT, Globals.UP, Globals.UP };
    protected int currentDir;

    public GhostOrangeMove()
    {
        currentDir = 0;
        isMoving = false;
    }

    public void SetDirection(int[][] Map)
    {
        newTileX = tileX;
        newTileZ = tileZ;

        if (directions[currentDir] == Globals.UP) newTileZ = newTileZ + 1;
        else if (directions[currentDir] == Globals.RIGHT) newTileX = newTileX + 1;
        else if (directions[currentDir] == Globals.DOWN) newTileZ = newTileZ - 1;
        else if (directions[currentDir] == Globals.LEFT) newTileX = newTileX - 1;

        //Debug.Log("ME (" + tileX + "," + tileZ + ")" + " - " + Map[tileZ][tileX] + " -- New (" + newTileX + "," + newTileZ + ")");
    }

    public override void chasingPacman(int[][] Map)
    {
        //Debug.Log("Moving Orange_GHOST" + " " + this.getBaseGhostSpeed());

        if (isMoving && transform.position == newPosition)
        {
            isMoving = false;
            tileX = newTileX;
            tileZ = newTileZ;
            SetDirection(Map);
        }

        if (isMoving)
        {
            float distCovered = (Time.time - startTime) * GHOST_SPEED;
            float fracJourney = distCovered / duration;
            transform.position = Vector3.MoveTowards(transform.position, newPosition, fracJourney);
        }
        else
        {
            bool ghostCanMove = doRotation(directions[currentDir]);

            if (ghostCanMove)
            {
                if (LevelCreator.isValidTile(newTileX, newTileZ) && isValid(Map, newTileX, newTileZ))
                {
                    Debug.Log("Calculating duration");

                    newPosition = LevelCreator.TileToPosition(newTileX, newTileZ, transform.position.y);
                    startTime = Time.time;
                    duration = Vector3.Distance(transform.position, newPosition);
                    isMoving = true;
                }
                else
                {
                    Debug.Log("Calculating direction");
                    currentDir = (currentDir + 1) % directions.Length;
                    SetDirection(Map);
                }
            }
            else Debug.Log("Cant move");
        }
    }
}
