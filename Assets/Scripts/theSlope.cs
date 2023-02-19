using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 22 degrees begins to lose speed
// 37 degrees is no longer grounded


public class theSlope : MonoBehaviour
{
    [Range(0f, 90f)][SerializeField]float m_rotation = 0f;
    void Update()
    {
        transform.eulerAngles = new(0f, 0f, m_rotation);
    }
}
