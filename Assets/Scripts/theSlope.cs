using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 22 degrees loses speed
// 39 degrees is no longer grounded


public class theSlope : MonoBehaviour
{
    [Range(-90f, 90f)][SerializeField]float m_rotation = 0f;
    void Update()
    {
        transform.eulerAngles = new(0f, 0f, m_rotation);
    }
}
