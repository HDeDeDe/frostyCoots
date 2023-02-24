using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayCoots : MonoBehaviour
{
    [SerializeField]AudioSource cootsSound;
    void Start()
    {
        cootsSound.Play();
    }
    void Update()
    {
        if(!cootsSound.isPlaying) Destroy(gameObject);
    }
}
