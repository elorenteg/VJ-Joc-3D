using UnityEngine;
using System.Collections;

public class GhostOrangeMove : GhostMove
{
    public GhostOrangeMove() { }

    // Completely random
    public override void chasingPacman(int[][] Map)
    {
        completelyRandom(Map);
    }
}
