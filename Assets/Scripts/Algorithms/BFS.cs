using UnityEngine;
using System.Collections;

public class BFS : MonoBehaviour
{
    private static int[] X = { -1, 0, 1, 0 };
    private static int[] Y = { 0, 1, 0, -1 };

    private struct Position
    {
        public int f;
        public int c;
    };

    public static int[] calculatePath(int[][] Map, int sTx, int sTz, int dTx, int dTz)
    {
        int si = sTz;
        int sj = sTx;
        int di = dTz;
        int dj = dTx;

        int N = LevelCreator.MAP_HEIGHT;
        int M = LevelCreator.MAP_WIDTH;
        Position sPos = initPosition(si, sj);

        // Vector distancia
        int[,] dist = new int[N, M];
        dist = initializeIntMatrix(dist, int.MaxValue);
        dist[si, sj] = 0;

        // Vector visitados
        bool[,] vis = new bool[N, M];
        vis = initializeBoolMatrix(vis, false);
        vis[si, sj] = true;

        // Cola
        Queue queue = new Queue();
        queue.Enqueue(sPos);

        bool found = false;
        while (queue.Count != 0 && !found)
        {
            Position p = (Position) queue.Dequeue();
            for (int i = 0; i < 4 && !found; ++i)
            {
                int f = p.f + Y[i];
                int c = p.c + X[i];

                if (LevelCreator.isValidTile(c, f) && !LevelCreator.isWall(c, f) && !vis[f,c])
                {
                    dist[f, c] = dist[p.f, p.c] + 1;
                    vis[f, c] = true;

                    if (f == di && c == dj) found = true;

                    Position p2 = initPosition(f, c);
                    queue.Enqueue(p2);
                }
            }

        }

        if (!found) Debug.Log("Destination not found");
        int[] path = recoverPath(dist, si, sj, di, dj);

        return path;
    }

    private static int[,] initializeIntMatrix(int[,] m, int value)
    {
        for (int i = 0; i < m.GetLength(0); i++)
            for (int j = 0; j < m.GetLength(1); ++j)
                m[i, j] = value;

        return m;
    }

    private static bool[,] initializeBoolMatrix(bool[,] m, bool value)
    {
        for (int i = 0; i < m.GetLength(0); i++)
            for (int j = 0; j < m.GetLength(1); ++j)
                m[i, j] = value;

        return m;
    }

    private static Position initPosition(int f, int c)
    {
        Position p;
        p.f = f;
        p.c = c;

        return p;
    }

    private static int[] recoverPath(int[,] dist, int si, int sj, int di, int dj)
    {
        Debug.Log("Source: " + si + " " + sj);
        Debug.Log("Destin: " + di + " " + dj);
        Debug.Log("DIST: " + dist[di, dj]);

        int numMoves = dist[di, dj];
        Position[] path = new Position[numMoves + 1];
        Position end = initPosition(di, dj);
        path[path.Length-1] = end;

        for (int k = path.Length-1; k > 0; --k)
        {
            Position p = path[k];
            int d = dist[p.f, p.c];

            bool found = false;
            for (int i = 0; i < 4 && !found; ++i)
            {
                int f = p.f + Y[i];
                int c = p.c + X[i];

                if (LevelCreator.isValidTile(c, f))
                {
                    int d2 = dist[f, c];
                    if (d2 < d) found = true;

                    Position p2 = initPosition(f, c);
                    path[k-1] = p2;
                }
            }
        }

        Debug.Log("PATH");

        for (int i = 0; i < path.Length; ++i)
        {
            Debug.Log(path[i].f + " " + path[i].c);
        }

        int[] directions = new int[numMoves];
        for (int k = 0; k < numMoves; ++k)
        {
            Position source = path[k];
            Position destin = path[k + 1];
            directions[k] = direction(source.f, source.c, destin.f, destin.c);
        }

        Debug.Log("PATH");

        for (int i = 0; i < directions.Length; ++i)
        {
            Debug.Log(directions[i]);
        }

        return directions;
    }

    private static int direction(int si, int sj, int di, int dj)
    {
        if (sj < dj) return Globals.RIGHT;
        else if (sj > dj) return Globals.LEFT;
        else if (si < di) return Globals.UP;
        else return Globals.DOWN;
    }
}
