using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RememberMusicValue : MonoBehaviour
{
    void Awake()//Just don't destroy this game object it is the music background and also destroy any new instances that are created on awake when we change scenes
    {
        if (!gameObject.activeSelf)
        {
            gameObject.SetActive(true);
        }
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Music");


        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
    }
}
