using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ActivateTutorial : MonoBehaviour
{
    public void LoadTutorialScene()
    {
        SceneManager.LoadScene(1);
    }
}
