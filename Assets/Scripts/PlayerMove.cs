using System;
using UnityEngine;
using TMPro;

public class PlayerMove : MonoBehaviour
{
    GameManager gm;

    [Header("References")]
    [SerializeField] Transform overlapPoint;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] SpriteRenderer sprite;
    Rigidbody2D rb;
    PlayerAnim pAnim;

    //Variables
    Vector2 m_moveVec;
    bool m_breaking;
    float m_storedBreak = 0f;
    float m_topSpeed = 0f;
    bool m_grounded;
    Vector3 m_rotation;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;

        pAnim = GetComponent<PlayerAnim>();

        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    private void Update()
    {
        m_moveVec = gm.input.GetMovementVector();
        //rb.AddForce(m_moveVec * Time.deltaTime, ForceMode2D.Force);
        m_breaking = gm.input.GetHalt();
        
        GroundCheck();

        sprite.transform.localEulerAngles = m_rotation;

        if(rb.velocity.x < 0)
        {
            sprite.flipX = true;
            return;
        }
        sprite.flipX = false;
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
            pAnim.Idle();
            m_topSpeed = 0f;
            m_storedBreak = 0f;
            Vector2 potentialVector = m_moveVec * gm.AirMomentum();
            rb.velocity += potentialVector;
            return;
        }

        if(!m_breaking)
        {
            pAnim.EndBreak();
            Vector2 potentialVector = m_moveVec;
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
            pAnim.Break();
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

    private void CapSpeed()
    {
        rb.velocity = new(Math.Clamp(rb.velocity.x, PVTools.velocityLimit * -1, PVTools.velocityLimit), Math.Clamp(rb.velocity.y, PVTools.velocityLimit * -1, PVTools.velocityLimit));
    }

    private void GroundCheck()
    {
        m_grounded = OverlapCheck(Physics2D.OverlapBox(overlapPoint.position, PVTools.overlapBox, 0f, groundLayer));
    }

    private bool OverlapCheck(Collider2D objectArray)
    {
        if (objectArray == null)
        {
            m_rotation = Vector3.zero;
            return false;
        } 
        m_rotation = objectArray.GetComponent<Transform>().rotation.eulerAngles;
        return true;
    }

    public void Panic()
    {
        rb.velocity = Vector2.zero;
        transform.position = Vector2.zero;
    }

    public Vector2 GetVelocity()
    {
        return rb.velocity;
    }
    public Vector2 GetSpeedInfo()
    {
        Vector2 i = new(m_topSpeed, m_storedBreak);
        return i;
    }
}
