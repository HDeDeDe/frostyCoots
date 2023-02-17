using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    PlayerMove player;
    Transform miniPlayer;

    TMP_Text speed;
    TMP_Text maxSpeed;
    TMP_Text jumpHeight;
    TMP_Text xspeed;

    [Header("Game Variables")]
    [Range(-100, 0)][SerializeField] float m_gravity;
    [Range(0f, 5f)][Tooltip("Affects how high the player jumps.")][SerializeField]  float m_jumpMultiplier = 1.1f;
    [Range(0f, 5f)][Tooltip("Affects how long it takes to break.")][SerializeField] float m_breakAggressiveness = 1.15f;
    [Range(0.000001f, 0.1f)][Tooltip("Affects air momentum.")][SerializeField] float m_airMomentum = 0.0625f;

    void Awake()
    {
        SceneManager.LoadScene("Scenes/Level", LoadSceneMode.Additive);
        SceneManager.LoadScene("Scenes/UI", LoadSceneMode.Additive);
        SceneManager.LoadScene("Scenes/Player", LoadSceneMode.Additive);
    }

    void Start()
    {
        miniPlayer = GameObject.Find("MiniPlayer").transform;
        player = GameObject.Find("the square").GetComponent<PlayerMove>();
        speed = GameObject.Find("Canvas/Speed").GetComponent<TMP_Text>();
        maxSpeed = GameObject.Find("Canvas/MaxSpeed").GetComponent<TMP_Text>();
        jumpHeight = GameObject.Find("Canvas/JumpHeight").GetComponent<TMP_Text>();
        xspeed = GameObject.Find("Canvas/XSpeed").GetComponent<TMP_Text>();
    }
    
    void Update()
    {
        Physics2D.gravity = new(0f, m_gravity);
        miniPlayer.position = player.transform.position;

        UpdateHud();
    }

    void UpdateHud()
    {
        Vector2 velocity = player.GetVelocity();
        Vector2 speedJump = player.GetSpeedInfo();

        speed.SetText(Convert.ToString(velocity.magnitude));
        maxSpeed.SetText(Convert.ToString(speedJump.x));
        jumpHeight.SetText(Convert.ToString(speedJump.y));
        xspeed.SetText(Convert.ToString(Math.Abs(velocity.x)));
    }

    public void Panic()
    {
        player.Panic();
    }

    public float JumpMultiplier()
    {
        return m_jumpMultiplier;
    }

    public float BreakAggro()
    {
        return m_breakAggressiveness;
    }
    
    public float AirMomentum()
    {
        return m_airMomentum;
    }
}
