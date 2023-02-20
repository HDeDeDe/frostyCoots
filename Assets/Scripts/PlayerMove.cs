using System;
using UnityEngine;

//test
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
    bool m_readyToJump;
    float m_storedBreak = 0f;
    float m_topSpeed = 0f;
    bool m_grounded;
    bool m_launched;
    Vector3 m_floorRotation;

    // Return variables
    private float Halve(float input) { return input / 2f;}
    private Vector2 Halve(Vector2 input) { return input / 2f;}
    private float AbsVelocityX() { return PVTools.Crimp(Math.Abs(rb.velocity.x)); }
    public Vector2 GetVelocity() 
    {
        if(rb == null) return Vector2.zero;
        return rb.velocity; 
    }
    //Returns m_topSpeed as x and m_storedBreak as y
    public Vector2 GetSpeedInfo() { return new(m_topSpeed, m_storedBreak); }
    public Vector3 GetFloorRotation() { return m_floorRotation; }
    public PlayerState GetState() { return state; }

    //Code
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
        m_readyToJump = gm.input.GetQuickJump();
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
            Vector2 potentialVector = Halve(gm.AirMomentum() * gm.Speed() * m_moveVec);
            rb.velocity += potentialVector;
            return;
        }

        if(!m_breaking)
        {
            m_launched = false;
            Vector2 potentialVector = Halve(m_moveVec * gm.Speed());
            if(m_readyToJump) potentialVector = Halve(potentialVector);
            if(AbsVelocityX() > gm.SoftSpeedCap()) potentialVector = Vector2.zero;
            if(m_storedBreak > 0f) 
            {
                m_launched = true;
                potentialVector.y = m_storedBreak;
                m_storedBreak = 0f;
            }
            rb.velocity += potentialVector;
            m_topSpeed = AbsVelocityX();
            return;
        }

        if(AbsVelocityX() <= PVTools.crimpLimit)
        {
            m_topSpeed = 0f;
            m_storedBreak = 0f;
            return;
        }

        if(m_readyToJump)
        {
            rb.AddForce(new(0f, gm.QuickJump()), ForceMode2D.Impulse);
            return;
        }

        float percent = m_topSpeed / (gm.BreakAggro() * 50);
        if(AbsVelocityX() > percent)
        {
            Vector2 potentialVector;
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
        if(m_grounded && m_readyToJump) return PlayerState.HOPPING;
        if(m_grounded && AbsVelocityX() > 0f) return PlayerState.MOVING;
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
        if(AbsVelocityX() == 0f) return;
        float temp = rb.velocity.x / AbsVelocityX();
        rb.AddForce(new(gm.PushMomentum() * temp, 0f), ForceMode2D.Impulse);
    }


}
