using System;
using System.Collections.Generic;
using UnityEngine;

public class CootsManager : MonoBehaviour
{
    bool setUp;
    GameManager gm;
    [SerializeField]List<Coots> m_cootsList;
    [SerializeField]List<Sprite> m_sprites;
    [SerializeField]Coots templateCoots;
    public int m_cootsTotal;
    public int m_collectedCoots = 0;
    void Awake()
    {
        m_cootsList.Clear();
    }
    void Start()
    {
        gm = GetComponent<GameManager>();
    }

    void Update()
    {
        if(!gm.Ready) return;
        if(!setUp) SetUp();
    }

    private void SetUp()
    {
        if(templateCoots == null) Debug.Break();
        List<Sprite> rngCoots = m_sprites;
        for(int i = 0; i < CootsCount(); i++)
        {
            int temp = Convert.ToInt32(Mathf.Floor(UnityEngine.Random.Range(0, rngCoots.Count - 1)));
            m_cootsList[i].SetCoots(rngCoots[temp]);
            rngCoots.RemoveAt(temp);
            rngCoots.TrimExcess();
        }
        m_cootsTotal = CootsCount();
        setUp = true;
    }
    
    public int CootsCount()
    {
        return m_cootsList.Count;
    }

    public int AddMe(Coots input)
    {
        if(!input.template && CootsCount() < 23)
        {
            m_cootsList.Insert(CootsCount(), input);
            return CootsCount() - 1;
        }
        if(templateCoots == null)
        {
            templateCoots = input;
            return 0;
        }
        return 1337;
    }

    public void ImDead(int position)
    {
        m_cootsList[position] = templateCoots;
        m_collectedCoots++;
    }

    public Coots GetTemplate()
    {
        return templateCoots;
    }
    public bool AllCoots()
    {
        if(m_collectedCoots == m_cootsTotal) return true;
        return false;
    }
}
