using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DontDestroy : MonoBehaviour
{
    public float LastVolume;
    public bool VolFlag;
   
    void Awake()//Just don't destroy this game object it is the music background and also destroy any new instances that are created on awake when we change scenes
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Music");

        LastVolume = gameObject.GetComponent<AudioSource>().volume;
        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
    }

    void Update()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        if(currentScene.name == "TutorialScene")
        {
            VolFlag = false;
        }
        if (GameObject.FindGameObjectWithTag("VolumeSlider"))
        {
            if(VolFlag == false)
            {
                GameObject.FindGameObjectWithTag("VolumeSlider").gameObject.GetComponent<Slider>().value = LastVolume;
                VolFlag = true;
            }
            gameObject.GetComponent<AudioSource>().volume = GameObject.FindGameObjectWithTag("VolumeSlider").gameObject.GetComponent<Slider>().value;
            LastVolume = GameObject.FindGameObjectWithTag("VolumeSlider").gameObject.GetComponent<Slider>().value;
        }
        
    }
}
