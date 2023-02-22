using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSFX : MonoBehaviour
{
    [Header("References")]
    [SerializeField]AudioSource SFX;
    [SerializeField]AudioSource JumpSFX;
    PlayerMove player;
    GameManager gm;

    PlayerState m_state;

    [SerializeField]AudioClip airSFX;
    [SerializeField]AudioClip groundSFX;

    private void Start()
    {
        player = GetComponent<PlayerMove>();
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    private void Update()
    {
        PlayJump();
        PlaySound();
        SFX.volume = SetVolume();
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
        if(m_state is PlayerState.AIRBORNE or PlayerState.LAUNCHING) volume = volume / 2f;
        if(volume > gm.SoftSpeedCap()) return 1f;
        volume = volume / gm.SoftSpeedCap();
        return volume * gm.SFXVolume();
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