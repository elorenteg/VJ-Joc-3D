using UnityEngine;
using System.Collections;

public static class Globals {

    public static int TILE_SIZE = 2;
    public static int MAP_WIDTH;
    public static int MAP_HEIGHT;

    public static float GHOST_OFFSET_X = 11.6f;
    public static float GHOST_OFFSET_Z = 13.9f;

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

    public static bool rotate(GameObject obj, float turnSpeed, int dir)
    {
        bool rotateLeft = true;

        bool fixAngle = false;
        float fixedAngle = 0.0f;
        float incAngle = turnSpeed * Time.deltaTime;

        float prevAngle = obj.transform.rotation.eulerAngles.y;
        float leftAngle = (prevAngle - incAngle) % 360;
        float rightAngle = (prevAngle + incAngle) % 360;

        bool canMove = false;
        bool rotate = false;
        if (dir == LEFT)
        {
            if (prevAngle >= 180.0f) rotateLeft = false;

            if ((rotateLeft && leftAngle < 360.0f && leftAngle > 180.0f) || (!rotateLeft && rightAngle > 0.0f && rightAngle < 180.0f))
            {
                fixAngle = true;
                fixedAngle = 0.0f;
            }

            if (prevAngle <= 0.0f + ERROR)
            {
                canMove = true;
                rotate = true;
                fixAngle = true;
                fixedAngle = 0.0f;
            }
            else rotate = true;
        }
        else if (dir == RIGHT)
        {
            if (prevAngle < 180.0f) rotateLeft = false;

            if ((rotateLeft && leftAngle < 180.0f && leftAngle > 0.0f) || (!rotateLeft && rightAngle > 180.0f && rightAngle < 360.0f))
            {
                fixAngle = true;
                fixedAngle = 180.0f;
            }

            if (prevAngle == 180.0f) canMove = true;
            else rotate = true;
        }
        else if (dir == UP)
        {
            if (prevAngle > 270.0f || prevAngle < 90.0f) rotateLeft = false;

            if ((rotateLeft && (leftAngle < 90.0f || leftAngle > 270.0f)) || (!rotateLeft && rightAngle > 90.0f && rightAngle < 270.0f))
            {
                fixAngle = true;
                fixedAngle = 90.0f;
            }

            if (prevAngle == 90.0f) canMove = true;
            else rotate = true;
        }
        else if (dir == DOWN)
        {
            if (prevAngle > 90.0f && prevAngle < 270.0f) rotateLeft = false;

            if ((rotateLeft && leftAngle < 270.0f && leftAngle > 90.0f) || (!rotateLeft && (rightAngle > 270.0f || rightAngle < 90.0f)))
            {
                fixAngle = true;
                fixedAngle = 270.0f;
            }

            if (prevAngle == 270.0f) canMove = true;
            else rotate = true;
        }

        if (rotate)
        {
            if (fixAngle) fixEulerAngle(obj, fixedAngle);
            else {
                SkinnedMeshRenderer skinnedMeshRenderer = obj.GetComponentInChildren<SkinnedMeshRenderer>();

                if (rotateLeft)
                    obj.transform.RotateAround(skinnedMeshRenderer.bounds.center, new Vector3(0, -1, 0), incAngle);
                else
                    obj.transform.RotateAround(skinnedMeshRenderer.bounds.center, new Vector3(0, 1, 0), incAngle);
            }

            return fixAngle;
        }

        return canMove;
    }

    public static void fixEulerAngle(GameObject obj, float fixedAngle)
    {
        Vector3 euler = obj.transform.eulerAngles;
        euler.y = fixedAngle;
        obj.transform.eulerAngles = euler;
    }
}
