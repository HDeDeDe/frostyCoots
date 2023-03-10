using System;
using UnityEngine;

//test
public class PlayerMove : MonoBehaviour
{
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
    [NonSerialized] public bool m_jumping;

    bool m_overrideGroundedMovement;
    [SerializeField]float m_overrideTimer = 3f;

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
        MoveFailsafe();
    }

    private void GetPlayerInput()
    {
        if(PVTools.gm.GetGameMode() != GameMode.PLAYING) 
        {
            m_moveVec = Vector2.zero;
            m_breaking = false;
            m_readyToJump = false;
            return;
        }
        m_moveVec = PVTools.gm.input.GetMovementVector();;
        m_breaking = PVTools.gm.input.GetHalt();
        m_readyToJump = PVTools.gm.input.GetQuickJump();
    }
    private void MovePlayer()
    {
        if(m_overrideGroundedMovement)
        {
            rb.AddForce(m_moveVec, ForceMode2D.Impulse);
            return;
        }
        if(!m_grounded)
        {
            if(rb.velocity.y < 0)
            {
                m_launched = false;
            }
            m_topSpeed = 0f;
            m_storedBreak = 0f;
            Vector2 potentialVector = PVTools.Halve(PVTools.gm.AirMomentum() * PVTools.gm.Speed() * m_moveVec);
            rb.velocity += potentialVector;
            return;
        }
        // ContactFilter2D tempContacts = new();
        // tempContacts.SetLayerMask(groundLayer);
        // m_floorRotation = GetGroundRotation(tempContacts);

        if(!m_breaking)
        {
            m_launched = false;
            Vector2 potentialVector = PVTools.Halve(m_moveVec * PVTools.gm.Speed());
            if(m_readyToJump) potentialVector = PVTools.Halve(potentialVector);
            if(AbsVelocityX() > PVTools.gm.SoftSpeedCap()) potentialVector = Vector2.zero;
            if(m_storedBreak > 0f) 
            {
                m_launched = true;
                potentialVector.y = m_storedBreak;
                m_storedBreak = 0f;
                m_jumping = true;
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
            rb.AddForce(new(0f, PVTools.gm.QuickJump()), ForceMode2D.Impulse);
            m_jumping = true;
            return;
        }

        float percent = m_topSpeed / (PVTools.gm.BreakAggro() * 50);
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
            m_storedBreak += percent * PVTools.gm.JumpMultiplier();
            return;
        }
        rb.velocity = new(0f, rb.velocity.y);
    }

    private void MoveFailsafe()
    {
        
        // if(m_grounded)
        // {
        //     m_overrideTimer = 3f;
        //     m_overrideGroundedMovement = false;
        //     return;
        // }
        if(m_grounded || !m_overrideGroundedMovement && PVTools.Crimp(rb.velocity.magnitude) != 0f)
        {
            m_overrideTimer = 3f;
            m_overrideGroundedMovement = false;
            return;
        }
        if(m_overrideTimer > 0f)
        {
            m_overrideTimer -= Time.fixedDeltaTime;
            return;
        }
        m_overrideGroundedMovement = true;
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
    // private float GetGroundRotation(ContactFilter2D filter)//ContactPoint2D[] input)
    // {
    //     float angle = 0f;
    //     ContactPoint2D[] contacts = new ContactPoint2D[10];
        
    //     int numContacts = rb.GetContacts(filter, contacts);
    //     for(int i = 0; i < numContacts; i++)
    //     {
    //         if(contacts[i].collider == null) continue;
    //         Debug.DrawLine(transform.position, contacts[i].point,  Color.magenta);
    //         angle = (MathF.Atan2(transform.position.y - contacts[i].point.y, transform.position.x - contacts[i].point.x) * 180 / Mathf.PI) - 90f;
    //         //if(temp_angle <= 38f)
    //     }

    //     return angle;
    //     // if(input == null) return Vector3.zero;

    //     // Debug.DrawRay(input[0].point, transform.position, Color.red);
    //     // float temp = Vector2.Angle(input[0].point, transform.position);
    //     // return new(0f, 0f, temp);

    // }

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
        rb.AddForce(new(PVTools.gm.PushMomentum() * temp, 0f), ForceMode2D.Impulse);
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(overlapPoint.position, PVTools.overlapBox * 2);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.layer != 9) return;
        if(other.gameObject.CompareTag("Goal"))
        {
           PVTools.gm.Win();
           return;
        }
        if(other.gameObject.CompareTag("Coots"))
        {
            Coots the = other.gameObject.GetComponent<Coots>();
            the.Collected();
        }
        if(other.gameObject.CompareTag("Teleporter"))
        {
            Teleporter the = other.gameObject.GetComponent<Teleporter>();
            transform.position = the.GetTeleport();
        }
    }
}
