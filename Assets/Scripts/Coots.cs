using System;
using UnityEngine;

public class Coots : MonoBehaviour
{
    public bool template = false;
    CootsManager cm;
    [NonSerialized]public SpriteRenderer sprite;
    [SerializeField] GameObject sfx;

    int m_position;

    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        cm = GameObject.Find("GameManager").GetComponent<CootsManager>();
        m_position = cm.AddMe(this);
        if(m_position == 1337) Destroy(gameObject);
    }

    public void SetCoots(Sprite input)
    {
        if(template) return;
        sprite.sprite = input;
    }
    
    public void Collected()
    {
        if(template) Application.Quit(1);
        cm.ImDead(m_position);
        Debug.Log("Response");
        Instantiate(sfx, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
