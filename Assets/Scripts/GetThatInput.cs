using System;
using UnityEngine;

public class GetThatInput : MonoBehaviour
{
    [Header("Keys")]
    public KeyCode left;
    public KeyCode right;
    public KeyCode halt;
    public KeyCode quickReady;
    // Variables
    Vector2 m_movementVector;
    bool m_halting;
    bool m_quickJump;

    private void Update()
    {
        m_movementVector.x = Convert.ToInt32(Input.GetKey(right)) + (Convert.ToInt32(Input.GetKey(left)) * -1); 
        m_halting = Input.GetKey(halt);
        m_quickJump = Input.GetKey(quickReady);
    }

    public Vector2 GetMovementVector() { return m_movementVector; }
    public bool GetHalt() { return m_halting; }
    public bool GetQuickJump() { return m_quickJump; }
}
