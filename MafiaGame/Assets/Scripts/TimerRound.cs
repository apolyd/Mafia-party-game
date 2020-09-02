using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerRound : MonoBehaviour
{
    public GameObject networkManager;

    public float timeRound;
    public bool isDay = true;
    
    // Start is called before the first frame update
    void Start()
    {
        timeRound = 60f;
        isDay = true;
        networkManager = GameObject.FindWithTag("NetworkManager");
    }

    //Update is called once per frame
    void Update()
    {
        
        if (networkManager.GetComponent<NetworkManager>().gameStarted == true) //if we have confirmation that there are 8 players
        {
           // time.text = timeRound.ToString();
            timeRound -= Time.deltaTime;
            if (timeRound < 0)
            {
                if (isDay == true)
                {
                    isDay = false;
                    timeRound = 60f;
                }
                else
                {
                    isDay = true;
                    timeRound = 60f;
                }

            }
        }
        
    }
}
