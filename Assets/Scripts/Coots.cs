using UnityEngine;

public class Coots : MonoBehaviour
{
    GameManager gm;

    void Start()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
    
    public void Collected()
    {
        Destroy(this);
    }
}
