using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    GameObject player;
    void Awake()
    {
        SceneManager.LoadScene("Scenes/UI", LoadSceneMode.Additive);
        SceneManager.LoadScene("Scenes/Player", LoadSceneMode.Additive);
    }

    void Start()
    {
        player = GameObject.Find("the square");
    }

    public void Panic()
    {
        player.GetComponent<PlayerMove>().Panic();
    }
    
}
