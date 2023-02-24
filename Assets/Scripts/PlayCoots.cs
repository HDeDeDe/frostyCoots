using UnityEngine;

public class PlayCoots : MonoBehaviour
{
    [SerializeField]AudioSource cootsSound;
    void Start()
    {
        cootsSound.volume = PVTools.gm.SFXVolume();
        cootsSound.Play();
    }
    void Update()
    {
        if(!cootsSound.isPlaying) Destroy(gameObject);
    }
}
