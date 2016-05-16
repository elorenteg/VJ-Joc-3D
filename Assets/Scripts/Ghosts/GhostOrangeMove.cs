using UnityEngine;
using System.Collections;

public class GhostOrangeMove : GhostMove
{
    private static int[] directions = { Globals.UP, Globals.RIGHT, Globals.DOWN, Globals.LEFT };

    public GhostOrangeMove() { }

    public override void chasingPacman(int[][] Map)
    {
        bool baseIsValid = false;
        int tx, tz;
        do
        {
            tx = Random.Range(0, LevelCreator.MAP_WIDTH);
            tz = Random.Range(0, LevelCreator.MAP_HEIGHT);
        } while (!isValid(Map, tx, tz, baseIsValid));

        currentPath = BFS.calculatePath(Map, tileX, tileZ, tx, tz, baseIsValid);
    }
}
