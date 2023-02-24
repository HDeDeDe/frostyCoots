using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAnim : MonoBehaviour
{
    Animator animator;

    GameMode m_lastMode;

    private static readonly int MainMenu = Animator.StringToHash("MainMenu");
    private static readonly int InGame = Animator.StringToHash("InGame");

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(PVTools.gm.GetGameMode() == m_lastMode) return;
        m_lastMode = PVTools.gm.GetGameMode();
        switch(m_lastMode)
        {
            case GameMode.MAINMENU:
                animator.CrossFade(MainMenu, 0, 0);
                break;
            default:
                animator.CrossFade(InGame, 0, 0);
                break;
        }
    }
}
