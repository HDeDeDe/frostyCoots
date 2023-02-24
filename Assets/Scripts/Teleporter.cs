using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    [SerializeField]Transform location;

    public Vector3 GetTeleport()
    {
        return location.position;
    }
}
