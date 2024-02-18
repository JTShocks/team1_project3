using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadMenuScene : MonoBehaviour
{

    void Awake()
    {
        LoadBackgroundScene();
        Cursor.lockState = CursorLockMode.None;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
    
    
    // Start is called before the first frame update
    void LoadBackgroundScene()
    {
        SceneManager.LoadScene("Level_BackDrop", LoadSceneMode.Additive);
    }
}
