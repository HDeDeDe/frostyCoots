using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSystem : MonoBehaviour
{
    GameObject gm;
    void Start()
    {
        gm = GameObject.Find("GameManager");
        CheckScene();
    }

    void CheckScene()
    {
        if(gm == null) SceneManager.LoadScene("Scenes/Systems", LoadSceneMode.Single);
        Destroy(this);
    }
}
