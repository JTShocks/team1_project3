using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{

    [SerializeField] GameObject mainMenu;
    [SerializeField] GameObject optionsMenu;
    [SerializeField] GameObject howToPlayMenu;
   public void EnableOptionMenu()
   {
        mainMenu.SetActive(false);
        optionsMenu.SetActive(true);
   }

   public void DisableOptionMenu()
   {
        mainMenu.SetActive(true);
        optionsMenu.SetActive(false);
   }

   public void EnableHTPMenu()
   {
          mainMenu.SetActive(false);
          howToPlayMenu.SetActive(true);
   }
   public void DisableHTPMenu()
   {
          mainMenu.SetActive(true);
          howToPlayMenu.SetActive(false);
   }
}
