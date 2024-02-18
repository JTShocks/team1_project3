using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{

     [SerializeField] GameObject mainMenu;
     [SerializeField] GameObject optionsMenu;
     [SerializeField] GameObject howToPlayMenu;

     AudioSource audioSource;
     [SerializeField] AudioClip clickSound;
     void Awake()
     {
          audioSource = GetComponent<AudioSource>();
     }

     public void EnableOptionMenu()
     {
          PlaySound(clickSound);
          mainMenu.SetActive(false);
          optionsMenu.SetActive(true);
     }

     public void DisableOptionMenu()
     {    
          PlaySound(clickSound);
          mainMenu.SetActive(true);
          optionsMenu.SetActive(false);
     }

     public void EnableHTPMenu()
     {   
          PlaySound(clickSound);
          mainMenu.SetActive(false);
          howToPlayMenu.SetActive(true);
     }
     public void DisableHTPMenu()
     {    
          PlaySound(clickSound);
          mainMenu.SetActive(true);
          howToPlayMenu.SetActive(false);
     }

   void PlaySound(AudioClip clip)
   {
     audioSource.PlayOneShot(clip);
   }
}
