using System;
using UnityEngine;
using TMPro;

public class PlayerMove : MonoBehaviour
{
    [Header("References")]
    [SerializeField] GetThatInput inputManager;
    [SerializeField] TMP_Text speed;
    [SerializeField] TMP_Text maxSpeed;
    [SerializeField] TMP_Text jumpHeight;
    [SerializeField] TMP_Text xspeed;
    [SerializeField] Transform overlapPoint;
    [SerializeField] LayerMask groundLayer;
    Rigidbody2D rb;

    //Variables
    [Header("Variables")]
    [Range(0f, 5f)][Tooltip("Affects how high the player jumps.")]
    public float m_jumpMultiplier;
    [Range(0f, 5f)][Tooltip("Affects how long it takes to break.")]
    public float m_breakAggressiveness;
    const float m_velocityLimit = 2500f;
    readonly Vector2 m_overlapBox = new(0.45f, 0.125f);
    Vector2 m_moveVec;
    bool m_breaking;
    float m_storedBreak = 0f;
    float m_topSpeed = 0f;
    [SerializeField]bool m_grounded;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
    }

    // Update is called once per frame
    private void Update()
    {
        m_moveVec = inputManager.GetMovementVector();
        //rb.AddForce(m_moveVec * Time.deltaTime, ForceMode2D.Force);
        m_breaking = inputManager.GetHalt();
        
        speed.SetText(Convert.ToString(rb.velocity.magnitude));
        maxSpeed.SetText(Convert.ToString(m_topSpeed));
        jumpHeight.SetText(Convert.ToString(m_storedBreak));
        xspeed.SetText(Convert.ToString(Math.Abs(rb.velocity.x)));

        GroundCheck();
    }

    private void FixedUpdate()
    {
        CapSpeed();
        MovePlayer();
    }

    private void MovePlayer()
    {
        if(!m_grounded)
        {
            m_topSpeed = 0f;
            m_storedBreak = 0f;
            return;
        }

        if(!m_breaking)
        {
            Vector2 potentialVector = m_moveVec;
            //potentialVector.y = -50f;
            if(m_storedBreak > 0f) 
            {
                potentialVector.y = m_storedBreak;
                m_storedBreak = 0f;
            }
            rb.velocity += potentialVector;
            m_topSpeed = Math.Abs(rb.velocity.x);
            return;
        }

        if(Math.Abs(rb.velocity.x) > 0)
        {
            Vector2 potentialVector;
            float percent = m_topSpeed / (m_breakAggressiveness * 50);
            if(Math.Abs(rb.velocity.x) > percent)
            {
                potentialVector.x = percent;
                potentialVector.y = 0f;
                if(rb.velocity.x > 0)
                {
                    rb.velocity -= potentialVector;
                }
                else
                {
                    rb.velocity += potentialVector;
                }
                m_storedBreak += percent * m_jumpMultiplier;
                return;
            }

            rb.velocity = new(0f, rb.velocity.y);
        }

        m_topSpeed = 0f;
        m_storedBreak = 0f;
    }

    private void CapSpeed()
    {
        rb.velocity = new(Math.Clamp(rb.velocity.x, m_velocityLimit * -1, m_velocityLimit), Math.Clamp(rb.velocity.y, m_velocityLimit * -1, m_velocityLimit));
    }

    private void GroundCheck()
    {
        m_grounded = OverlapCheck(Physics2D.OverlapBox(overlapPoint.position, m_overlapBox, 0f, groundLayer));
    }

    private bool OverlapCheck(Collider2D objectArray)
    {
        if (objectArray == null) return false;
        return true;
    }

    public void Panic()
    {
        rb.velocity = Vector2.zero;
        transform.position = Vector2.zero;
    }
}
