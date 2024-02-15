using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
   void OnEnable()
   {
      CaptureBox.OnPlayerCaught += ReloadScene;
   }
   void OnDisable()
   {
      CaptureBox.OnPlayerCaught -= ReloadScene;
   }

   void ReloadScene()
   {
    Scene scene = SceneManager.GetActiveScene(); 
    SceneManager.LoadScene(scene.name); 
   }

   public void OpenThisScene(string sceneName)
   {
      SceneManager.LoadScene(sceneName); 
   }

   public void CloseGame()
   {
      Application.Quit();
   }
}
