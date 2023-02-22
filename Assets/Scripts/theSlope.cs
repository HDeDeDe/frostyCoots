using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// above 21.9 degrees loses speed
// above 38.8 degrees is no longer grounded


public class theSlope : MonoBehaviour
{
    [Range(-90f, 90f)][SerializeField]float m_rotation = 0f;
    void Update()
    {
        transform.eulerAngles = new(0f, 0f, m_rotation);
    }
}
