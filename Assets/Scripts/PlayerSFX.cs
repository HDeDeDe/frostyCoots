using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSFX : MonoBehaviour
{
    [Header("References")]
    [SerializeField]AudioSource SFX;
    [SerializeField]AudioSource JumpSFX;
    [SerializeField]AudioClip airSFX;
    [SerializeField]AudioClip groundSFX;
    PlayerMove player;

    PlayerState m_state;

    [Header("Variables")]
    [Range(0f, 1f)][Tooltip("Affects the volume of sfx.")][SerializeField] float m_localVolume = 0.65f;

    private void Start()
    {
        player = GetComponent<PlayerMove>();
    }

    private void Update()
    {
        PlayJump();
        PlaySound();
        SFX.volume = SetVolume();
        JumpSFX.volume = PVTools.gm.SFXVolume();
    }

    private void PlayJump()
    {
        if(player.m_jumping)
        {
            JumpSFX.Play();
            player.m_jumping = false;
        }
    }

    private float SetVolume()
    {
        float volume = PVTools.Crimp(player.GetVelocity().magnitude);
        if(m_state is PlayerState.AIRBORNE or PlayerState.LAUNCHING) volume /= 2f;
        if(volume > PVTools.gm.SoftSpeedCap()) return 1f;
        volume /= PVTools.gm.SoftSpeedCap();
        return volume * m_localVolume * PVTools.gm.SFXVolume();
    }

    private void PlaySound()
    {
        if(player.state == m_state) return;
        m_state = player.state;
        switch(m_state)
        {
            case PlayerState.MOVING:
            case PlayerState.HOPPING:
            case PlayerState.BREAKING:
            {
                SFX.loop = true;
                SFX.clip = groundSFX;
                SFX.Play();
                return;
            }
            case PlayerState.LAUNCHING:
            case PlayerState.AIRBORNE:
            {
                SFX.loop = true;
                SFX.clip = airSFX;
                SFX.Play();
                return;
            }
            default:
            {
                SFX.Stop();
                return;
            }
        }
    }
}