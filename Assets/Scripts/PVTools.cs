using System;
using UnityEngine;

public enum PlayerState : byte 
{
    MOVING,
    AIRBORNE,
    BREAKING,
    LAUNCHING
}

public static class PVTools
{
    public const float velocityLimit = 2500f;
    public const float velocityLimitNegative = -2500f;
    public readonly static Vector2 overlapBox = new(0.45f, 0.125f);

    public static Vector2 CapSpeed(Vector2 input)
    {
        return new(Math.Clamp(input.x, velocityLimitNegative, velocityLimit), Math.Clamp(input.y, velocityLimitNegative, velocityLimit));
    }
}
