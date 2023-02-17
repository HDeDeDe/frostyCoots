using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    GameObject player;

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
        player = GameObject.Find("the square");
    }
    
    void Update()
    {
        Physics2D.gravity = new(0f, m_gravity);
    }

    public void Panic()
    {
        player.GetComponent<PlayerMove>().Panic();
    }
    
}
