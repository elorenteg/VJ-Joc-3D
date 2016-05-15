using UnityEngine;
using System.Collections;

public static class Globals {

    public static int TILE_SIZE = 2;
    public static int MAP_WIDTH;
    public static int MAP_HEIGHT;

    public static string TAG_GHOST_BLUE = "ghost_blue";
    public static string TAG_GHOST_ORANGE = "ghost_orange";
    public static string TAG_GHOST_PINK = "ghost_pink";
    public static string TAG_GHOST_RED = "ghost_red";

    public static float PACMAN_OFFSET_X = 0.38f;
    public static float PACMAN_OFFSET_Z = -6.2f;

    public static void SetMapSizes(int w, int h)
    {
        MAP_WIDTH = w;
        MAP_HEIGHT = h;
    }

    private const float ERROR = 1.5f;

    public const int LEFT = 0;
    public const int RIGHT = 1;
    public const int UP = 2;
    public const int DOWN = 3;
}
