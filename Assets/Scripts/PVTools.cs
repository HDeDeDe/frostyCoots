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
    public readonly static Vector2 overlapBox = new(0.45f, 0.125f);
}
