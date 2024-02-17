using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{

   Animator animator;
   void OnEnable()
   {
      CaptureBox.OnPlayerCaught += ReloadScene;
      Door.OnExitDoorOpened += OpenThisScene;
   }
   void OnDisable()
   {
      CaptureBox.OnPlayerCaught -= ReloadScene;
      Door.OnExitDoorOpened -= OpenThisScene;
   }

   void Awake()
   {
      animator = GetComponent<Animator>();
   }

   void ReloadScene()
   {
      Scene scene = SceneManager.GetActiveScene(); 
      animator.SetTrigger("FadeOut");
      SceneManager.LoadScene(scene.name); 

   }

   public void OpenThisScene(string sceneName)
   {
      animator.SetTrigger("FadeOut");
      SceneManager.LoadScene(sceneName); 

   }

   public void CloseGame()
   {
      Application.Quit();
   }


}
