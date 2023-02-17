using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    GameObject player;
    Transform miniPlayer;

    [Range(-100, 0)]
    [SerializeField] float m_gravity;
    void Awake()
    {
        SceneManager.LoadScene("Scenes/Level", LoadSceneMode.Additive);
        SceneManager.LoadScene("Scenes/UI", LoadSceneMode.Additive);
        SceneManager.LoadScene("Scenes/Player", LoadSceneMode.Additive);
    }

    void Start()
    {
        miniPlayer = GameObject.Find("MiniPlayer").transform;
        player = GameObject.Find("the square");
    }
    
    void Update()
    {
        Physics2D.gravity = new(0f, m_gravity);
        miniPlayer.position = player.transform.position;
    }

    public void Panic()
    {
        player.GetComponent<PlayerMove>().Panic();
    }
    
}
