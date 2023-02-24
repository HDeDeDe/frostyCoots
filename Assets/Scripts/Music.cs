using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music : MonoBehaviour
{
    AudioSource music;
    [Range(0f, 1f)][Tooltip("Affects the volume of music.")][SerializeField] float m_localVolume = 0.6f;

    void Start()
    {
        music = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        music.volume = m_localVolume * PVTools.gm.MusicVolume();
    }
}
