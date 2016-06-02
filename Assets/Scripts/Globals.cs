using UnityEngine;
using System.Collections;

public static class Globals
{

    public static bool ARE_CHEATS_ON = true;
    public static string TAG_PACMAN = "pacman";
    public static string TAG_GHOST = "ghost";
    public static string TAG_COIN = "coin";
    public static string TAG_BONUS = "bonus";
    public static string TAG_CHERRY = "cherry";
    public static string TAG_WALL = "wall";
    public static string TAG_FLOOR = "floor";
    public static string TAG_GHOST_BLUE = "ghost_blue";
    public static string TAG_GHOST_ORANGE = "ghost_orange";
    public static string TAG_GHOST_PINK = "ghost_pink";
    public static string TAG_GHOST_RED = "ghost_red";
    public static string TAG_BATTERY = "battery";
    public static string TAG_TURTLE = "turtle";
    public static string TAG_GUM = "gum";

    public static float PACMAN_OFFSET_X = 0.38f;
    public static float PACMAN_OFFSET_Z = -6.2f;

    private const float ERROR = 1.5f;

    public const int NONE = 0;
    public const int LEFT = 1;
    public const int RIGHT = 2;
    public const int UP = 3;
    public const int DOWN = 4;
}
