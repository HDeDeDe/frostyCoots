using UnityEngine;

public class PlayerAnim : MonoBehaviour
{
    [Header("References")]
    [SerializeField]SpriteRenderer sprite;
    Animator animator;
    PlayerMove player;

    private static readonly int BreakStart = Animator.StringToHash("BreakStart");
    //private static readonly int Breaking = Animator.StringToHash("Breaking");
    private static readonly int BreakEnd = Animator.StringToHash("BreakEnd");
    private static readonly int Idling = Animator.StringToHash("Idle");

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
        m_state = player.state;
        switch(m_state)
        {
            case PlayerState.LAUNCHING:
                animator.CrossFade(BreakEnd, 0, 0);
                break;
            case PlayerState.BREAKING:
                animator.CrossFade(BreakStart, 0, 0);
                break;
            default:
                animator.CrossFade(Idling, 0, 0);
                break;
        }
        
    }

    private void TransformSprite()
    {
        if(player.GetVelocity().x < -0.01f)
        {
            sprite.flipX = true;
        }
        if(player.GetVelocity().x > 0.01f)
        {
            sprite.flipX = false;
        }
        sprite.transform.localEulerAngles = player.GetFloorRotation();
    }
}
