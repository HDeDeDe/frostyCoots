using System;
using UnityEngine;

public class GetThatInput : MonoBehaviour
{
    [Header("Keys")]
    public KeyCode left;
    public KeyCode right;
    public KeyCode halt;
    // Variables
    Vector2 m_movementVector;
    bool m_halting;

    private void Update()
    {
        m_movementVector.x = Convert.ToInt32(Input.GetKey(right)) + (Convert.ToInt32(Input.GetKey(left)) * -1); 
        m_halting = Input.GetKey(halt);
    }

    public Vector2 GetMovementVector()
    {
        return m_movementVector;
    }

    public bool GetHalt()
    {
        return m_halting;
    }
}
