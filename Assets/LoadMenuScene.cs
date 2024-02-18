using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadMenuScene : MonoBehaviour
{

    void Awake()
    {
        LoadBackgroundScene();
    }
    
    // Start is called before the first frame update
    void LoadBackgroundScene()
    {
        SceneManager.LoadScene("mainMenu_backdrop", LoadSceneMode.Additive);
    }
}
