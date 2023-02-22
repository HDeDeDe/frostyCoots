using UnityEngine;

public class Coots : MonoBehaviour
{
    public bool template = false;
    CootsManager cm;
    int position;
    

    void Start()
    {
        cm = GameObject.Find("GameManager").GetComponent<CootsManager>();
        if(template && cm.GetTemplate() != null) Destroy(this);
        if(cm.CootsCount() > 22 && !template) Destroy(this);
        position = cm.AddMe(this);
        if(position == 1337) Destroy(this);
    }

    
    public void Collected()
    {
        Destroy(this);
    }
}
