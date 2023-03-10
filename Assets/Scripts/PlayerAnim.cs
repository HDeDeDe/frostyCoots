using UnityEngine;

public class PlayerAnim : MonoBehaviour
{
    [Header("References")]
    [SerializeField]SpriteRenderer sprite;
    Animator animator;
    PlayerMove player;

    private static readonly int BreakStart = Animator.StringToHash("BreakStart");
    private static readonly int BreakEnd = Animator.StringToHash("BreakEnd");
    private static readonly int Idle = Animator.StringToHash("Idle");
    private static readonly int Falling = Animator.StringToHash("Falling");
    private static readonly int Jumping = Animator.StringToHash("Jumping");
    private static readonly int PushOff = Animator.StringToHash("PushOff");
    private static readonly int ShortHop = Animator.StringToHash("ShortHop");

    PlayerState m_state;

    void Start()
    {
        player = GetComponent<PlayerMove>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        SetAnim();
        TransformSprite();
    }

    private void SetAnim()
    {
        if(player.state == m_state) return;

        PlayerState oldState = m_state;
        m_state = player.state;

        if(oldState == PlayerState.BREAKING && m_state == PlayerState.IDLE)
        {
            m_state = player.state;
            animator.CrossFade(BreakEnd, 0, 0);
            return;
        }
        if(oldState == PlayerState.HOPPING && m_state == PlayerState.MOVING)
        {
            m_state = player.state;
            animator.CrossFade(Idle, 0, 0);
            return;
        }

        switch(m_state)
        {
            case PlayerState.LAUNCHING:
                animator.CrossFade(Jumping, 0, 0);
                break;
            case PlayerState.BREAKING:
                animator.CrossFade(BreakStart, 0, 0);
                break;
            case PlayerState.AIRBORNE:
                animator.CrossFade(Falling, 0, 0);
                break;
            case PlayerState.MOVING:
                animator.CrossFade(PushOff, 0, 0);
                break;
            case PlayerState.HOPPING:
                animator.CrossFade(ShortHop, 0, 0);
                break;
            default:
                animator.CrossFade(Idle, 0, 0);
                break;
        }
        
    }

    private void TransformSprite()
    {
        if(player.GetVelocity().x < PVTools.crimpLimitNegative)
        {
            sprite.flipX = true;
        }
        if(player.GetVelocity().x > PVTools.crimpLimit)
        {
            sprite.flipX = false;
        }
        sprite.transform.localEulerAngles = player.GetFloorRotation();
    }
}
