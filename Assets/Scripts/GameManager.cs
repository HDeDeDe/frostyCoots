using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    

    [Header("References")]
    [SerializeField] GameObject playerObject;
    PlayerMove player;
    Transform miniPlayer;
    Transform miniMapCamera;
    public GetThatInput input;
    CootsManager cm;

    TMP_Text speed;
    TMP_Text maxSpeed;
    TMP_Text jumpHeight;
    TMP_Text xspeed;
    TMP_Text rotation;
    TMP_Text winner;
    TMP_Text cootsCount;
    RawImage miniMap;

    [Header("Game Variables")]
    [Tooltip("Set starting game mode.")][SerializeField] GameMode gameMode = GameMode.MAINMENU;
    [Range(-100, 0)][Tooltip("Affects Stardenburdenhardenbart.")][SerializeField] float m_gravity;
    [Range(0f, 5f)][Tooltip("Affects how high the player jumps.")][SerializeField] float m_jumpMultiplier = 1.1f;
    [Range(0f, 5f)][Tooltip("Affects how long it takes to break.")][SerializeField] float m_breakAggressiveness = 1.15f;
    [Range(0.000001f, 0.1f)][Tooltip("Affects air momentum.")][SerializeField] float m_airMomentum = 0.0625f;
    [Range(0f, 15f)][Tooltip("Affects the push when starting to move.")][SerializeField] float m_pushMomentum = 5f;
    [Range(0.25f, 2f)][Tooltip("Affects speed gain.")][SerializeField] float m_speed = 1f;
    [Range(1, 50)][Tooltip("Affects quick jump height.")][SerializeField] int m_quickHeight = 5;
    [Range(50f, 500f)][Tooltip("Affects soft speed cap.")][SerializeField] float m_softSpeedCap = 250f;
    [Tooltip("Affects minimap")][SerializeField] bool m_miniMap = true;
    [Range(0f, 1f)][Tooltip("Affects SFX volume.")][SerializeField] float m_sfxVolume = 1f;
    [Tooltip("Determines if all coots are required to win.")][SerializeField] bool m_cootsCondition = false;
    [Range(0f, 1f)][Tooltip("Music volume.")][SerializeField] float m_musicVolume = 1f;
    [Range(0f, 1f)][Tooltip("Global volume.")][SerializeField] float m_globalVolume = 1f;

    public bool Ready {get; private set;} = false;

    void Awake()
    {
        PVTools.SetManager(this);
        SceneManager.LoadScene("Scenes/Level", LoadSceneMode.Additive);
        SceneManager.LoadScene("Scenes/UI", LoadSceneMode.Additive);
    }

    void Start()
    {
        Transform temp = GameObject.FindGameObjectWithTag("PlayerSpawn").transform;
        player = Instantiate(playerObject, temp).GetComponent<PlayerMove>();

        miniPlayer = GameObject.Find("MiniMap/MiniPlayer").transform;
        miniMapCamera = GameObject.Find("MiniMap/MiniMapCamera").transform;
        speed = GameObject.Find("Canvas/GameUI/Speed").GetComponent<TMP_Text>();
        maxSpeed = GameObject.Find("Canvas/GameUI/MaxSpeed").GetComponent<TMP_Text>();
        jumpHeight = GameObject.Find("Canvas/GameUI/JumpHeight").GetComponent<TMP_Text>();
        xspeed = GameObject.Find("Canvas/GameUI/XSpeed").GetComponent<TMP_Text>();
        rotation = GameObject.Find("Canvas/GameUI/Rotation").GetComponent<TMP_Text>();
        miniMap = GameObject.Find("Canvas/GameUI/MiniMap").GetComponent<RawImage>();
        winner = GameObject.Find("Canvas/GameUI/YOU'RE WINNER !").GetComponent<TMP_Text>();
        cootsCount = GameObject.Find("Canvas/GameUI/CootsCount").GetComponent<TMP_Text>();
        cm = GetComponent<CootsManager>();
    }
    
    void Update()
    {
        Ready = true;
        Physics2D.gravity = new(0f, m_gravity);

        UpdateHud();
    }

    public void Win()
    {
        if(m_cootsCondition && !cm.AllCoots()) return;
        gameMode = GameMode.GAMEOVER;
    }

    void UpdateHud()
    {
        miniPlayer.position = new(player.transform.position.x, player.transform.position.y, -1f);
        miniMapCamera.position = new(player.transform.position.x, miniMapCamera.position.y, -10f);

        Vector2 velocity = player.GetVelocity();
        Vector2 speedJump = player.GetSpeedInfo();
        Vector3 p_rotation = player.GetFloorRotation();

        speed.SetText(Convert.ToString(PVTools.Crimp(velocity.magnitude)));
        maxSpeed.SetText(Convert.ToString(PVTools.Crimp(speedJump.x)));
        jumpHeight.SetText(Convert.ToString(PVTools.Crimp(speedJump.y)));
        xspeed.SetText(Convert.ToString(Math.Abs(PVTools.Crimp(velocity.x))));
        rotation.SetText(Convert.ToString(PVTools.Crimp(p_rotation.z)));
        miniMap.uvRect = new(0f, 0f, Convert.ToInt32(m_miniMap), 1f);

        string cc = Convert.ToString(cm.m_collectedCoots) + " / " + Convert.ToString(cm.m_cootsTotal);
        cootsCount.SetText(cc);

        if(gameMode == GameMode.GAMEOVER)
        {
            winner.SetText("YOU WIN!");
            return;
        }
        winner.SetText("");
    }

    public void SetGameMode(GameMode mode)
    {
        gameMode = mode;
    }

    public void Panic() { player.Panic(); }
    public float JumpMultiplier() { return m_jumpMultiplier; }
    public float BreakAggro() { return m_breakAggressiveness; }
    public float AirMomentum() { return m_airMomentum; }
    public float PushMomentum() { return m_pushMomentum; }
    public float Speed() { return m_speed; }
    public float QuickJump() { return m_quickHeight; }
    public float SoftSpeedCap() { return m_softSpeedCap; }
    public float SFXVolume() { return m_sfxVolume * m_globalVolume; }
    public float MusicVolume() { return m_musicVolume * m_globalVolume; }
    public GameMode GetGameMode(){ return gameMode; }
}
