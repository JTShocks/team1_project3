using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{

    [SerializeField] GameObject mainMenu;
    [SerializeField] GameObject optionsMenu;
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
}
