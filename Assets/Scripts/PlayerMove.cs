using System;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    GameManager gm;
    [Header("References")]
    [SerializeField] Transform overlapPoint;
    [SerializeField] LayerMask groundLayer;
    Rigidbody2D rb;

    //Variables
    [NonSerialized] public PlayerState state;
    Vector2 m_moveVec;
    bool m_breaking;
    float m_storedBreak = 0f;
    float m_topSpeed = 0f;
    bool m_grounded;
    bool m_launched;

    Vector3 m_floorRotation;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;

        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    private void Update()
    {
        GetPlayerInput();
        GroundCheck();
        state = StateHandler();
    }

    private void FixedUpdate()
    {
        rb.velocity = PVTools.CapSpeed(rb.velocity);
        MovePlayer();
    }

    private void GetPlayerInput()
    {
        m_moveVec = gm.input.GetMovementVector();;
        m_breaking = gm.input.GetHalt();
    }
    private void MovePlayer()
    {
        if(!m_grounded)
        {
            if(rb.velocity.y < 0)
            {
                m_launched = false;
            }
            m_topSpeed = 0f;
            m_storedBreak = 0f;
            Vector2 potentialVector = m_moveVec * gm.AirMomentum();
            rb.velocity += potentialVector;
            return;
        }

        if(!m_breaking)
        {
            m_launched = false;
            Vector2 potentialVector = m_moveVec;
            if(m_storedBreak > 0f) 
            {
                m_launched = true;
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
            float percent = m_topSpeed / (gm.BreakAggro() * 50);
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
                m_storedBreak += percent * gm.JumpMultiplier();
                return;
            }

            rb.velocity = new(0f, rb.velocity.y);
        }

        m_topSpeed = 0f;
        m_storedBreak = 0f;
    }

    private void GroundCheck()
    {
        m_grounded = OverlapCheck(Physics2D.OverlapBox(overlapPoint.position, PVTools.overlapBox, 0f, groundLayer));
    }

    private bool OverlapCheck(Collider2D objectArray)
    {
        if (objectArray == null)
        {
            return false;
        } 
        m_floorRotation = objectArray.GetComponent<Transform>().rotation.eulerAngles;
        return true;
    }
    private PlayerState StateHandler()
    {
        if(m_launched) return PlayerState.LAUNCHING;
        if(m_breaking) return PlayerState.BREAKING;
        if(m_grounded && PVTools.Crimp(Math.Abs(rb.velocity.x)) > 0f) return PlayerState.MOVING;
        if(m_grounded) return PlayerState.IDLE;
        return PlayerState.AIRBORNE;
    }

    public void Panic()
    {
        rb.velocity = Vector2.zero;
        transform.localPosition = Vector2.zero;
    }

    public void Push()
    {
        float temp = rb.velocity.x / Math.Abs(rb.velocity.x);
        rb.AddForce(new(gm.PushMomentum() * temp, 0f), ForceMode2D.Impulse);
    }

    public Vector2 GetVelocity() 
    {
        if(rb == null) return Vector2.zero;
        return rb.velocity; 
    }
    //Returns m_topSpeed as x and m_storedBreak as y
    public Vector2 GetSpeedInfo() { return new(m_topSpeed, m_storedBreak); }
    public Vector3 GetFloorRotation() { return m_floorRotation; }
    public PlayerState GetState() { return state; }
}
