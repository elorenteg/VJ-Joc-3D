using UnityEngine;
using System.Collections;

public class GhostBlueMove : GhostMove
{
    private static int MAX_PATHFINDERS_CHASING_PACMAN = 10;
    private int timeChasingPacman;
    private bool haveSeenPacman;

    public GhostBlueMove() {}

    protected override void initSpecificGhost() {
        haveSeenPacman = false;
    }

    // Random but chasing Pacman if he sees him
    public override void chasingPacman(int[][] Map)
    {
        if (!haveSeenPacman && CanSeePacman(Map))
        {
            haveSeenPacman = true;
            timeChasingPacman = MAX_PATHFINDERS_CHASING_PACMAN;
        }

        if (haveSeenPacman && timeChasingPacman > 0) base.chasingPacman(Map);
        else
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

        --timeChasingPacman;

        if (timeChasingPacman == 0) haveSeenPacman = false;
    }
}
