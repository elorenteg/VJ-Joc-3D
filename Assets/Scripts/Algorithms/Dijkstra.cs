using UnityEngine;
using System.Collections.Generic;

public class Dijkstra : MonoBehaviour
{

    public static int[] calculatePath(int[][] Map, int si, int sj, int di, int dj)
    {
        int N = LevelCreator.MAP_HEIGHT;
        int M = LevelCreator.MAP_WIDTH;
        int posTile = positionToTile(M, si, sj);

        // Vector distancias
        int[] dist = new int[N * M];
        dist = initializeIntMatrix(dist, int.MaxValue);
        dist[posTile] = 0;

        // Vector precedente
        int[] prei = new int[N * M];
        int[] prej = new int[N * M];
        prei = initializeIntMatrix(prei, -1);
        prej = initializeIntMatrix(prej, -1);

        // Vector visitados
        bool[] vis = new bool[N * M];
        vis = initializeBoolMatrix(vis, false);

        // Cola de prioridad (SortedList no acepta repetidos)
        SortedList<int, int> prior_queue = new SortedList<int, int>();
        prior_queue.Add(0, posTile);

        //while (prior_queue.)

        int[] path;

        return null;
    }

    private static int[] initializeIntMatrix(int[] m, int value)
    {
        int N = m.Length;
        int M = m.GetLength(0);

        for (int i = 0; i < N; i++)
            for (int j = 0; j < M; ++j)
                m[positionToTile(M, i, j)] = value;

        return m;
    }

    private static bool[] initializeBoolMatrix(bool[] m, bool value)
    {
        int N = m.Length;
        int M = m.GetLength(0);

        for (int i = 0; i < N; i++)
            for (int j = 0; j < M; ++j)
                m[positionToTile(M, i, j)] = value;

        return m;
    }

    private static int positionToTile(int w, int i, int j)
    {
        return i * w + j;
    }
}
