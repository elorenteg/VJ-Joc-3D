﻿using UnityEngine;
using System.Collections;

public class GhostPinkMove : GhostMove
{
    public GhostPinkMove()
    {

    }

    public override void chasingPacman(int[][] Map)
    {
        bool baseIsValid = false;
        int pactx, pactz;
        PacmanMove moveScript = pacmanObj.GetComponent<PacmanMove>();
        moveScript.ActualTiles(out pactx, out pactz);

        Debug.Log(pactx + " " + pactz);

        int[] allPath = BFS.calculatePath(Map, tileX, tileZ, pactx, pactz, baseIsValid);
        int size = Mathf.Min(5, allPath.Length);
        currentPath = new int[size];
        for (int i = 0; i < size; ++i)
        {
            currentPath[i] = allPath[i];
        }
    }
}
