using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CootsManager : MonoBehaviour
{
    GameManager gm;
    List<Coots> m_cootsList;
    Coots templateCoots;
    void Start()
    {
        gm = GetComponent<GameManager>();
    }

    void Update()
    {
        if(!gm.Ready) return;

    }
    
    public int CootsCount()
    {
        return m_cootsList.Count;
    }

    public int AddMe(Coots input)
    {
        
        if(input.template)
        {
            templateCoots = input;
            return 0;
        }
        m_cootsList.Insert(CootsCount(), input);
        return CootsCount() - 1;
    }
    public Coots GetTemplate()
    {
        return templateCoots;
    }
}
