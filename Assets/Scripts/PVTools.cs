using System;
using UnityEngine;

public enum AnimState : byte
{
    IDLE,
    BREAKING,
    BREAKSTART,
    BREAKEND
}

public static class PVTools
{
    public const float velocityLimit = 2500f;
    public readonly static Vector2 overlapBox = new(0.45f, 0.125f);
}
