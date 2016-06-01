using UnityEngine;
using System.Collections;

public class GhostOrangeMove : GhostMove
{
    public GhostOrangeMove() { }

    // Completely random
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
