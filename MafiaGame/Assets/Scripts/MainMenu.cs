using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public GameObject StartGameMenu, InitialGameMenu;

    private void Awake()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            gameObject.GetComponent<Canvas>().scaleFactor = 2;
        }
    }
    public void ExitGame() //just quit the game nothing else
    {
        Application.Quit();
        
    }


   
   
}
