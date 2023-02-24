using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayCoots : MonoBehaviour
{
    GameManager gm;
    [SerializeField]AudioSource cootsSound;
    void Start()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        cootsSound.volume = gm.SFXVolume();
        cootsSound.Play();
    }
    void Update()
    {
        if(!cootsSound.isPlaying) Destroy(gameObject);
    }
}
