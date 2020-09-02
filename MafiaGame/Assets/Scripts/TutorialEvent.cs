using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TutorialEvent : MonoBehaviour
{
    public GameObject DonCanvas;
    public Text DonText;
    public bool StartFlag; //start the tutorial
    public string Intro, UIexplanation, TimerExplanation, ChatBoxExplanation, TargetSystemUI, ButtonsExplanation;
    private float timer;
    private int order;
    void Start()
    {
        DonCanvas.SetActive(true);
        Intro = "Hello, I am Don Capsulone and I will make you an offer you can't refuse. I will explain you the basics of this game!";
        UIexplanation = "You can look around and target players by moving the camera with your mouse or using the touch screen if you are on mobile.";
        TimerExplanation = "On your bottom right you will see how long each round lasts.";
        ChatBoxExplanation = "On the bottom left you will see messages from other players and you will get feedback for their actions.";
        TargetSystemUI = "On the top of the screen you will see the name of the player you target. You will also be able to check your role in the current game.";
        ButtonsExplanation = "By pressing escape you can use the buttons at the bottom of the screen. By pressing them now I will explain their use. When you are done click the Exit button to exit the tutorial";
        timer = 10;
        order = 0;
        StartFlag = true;
        DonText.text = Intro;
        gameObject.GetComponent<Animator>().Play("ShowTop");
    }

    // Update is called once per frame
    void Update()
    {
        if(StartFlag == true)
        {
            timer -= Time.deltaTime;
            if(timer <= 0)
            {
                if(order <= 4)
                {
                    DonText.text = NextMessage();
                    timer = 10f;
                }
                else
                {
                    return;
                }
                
            }
        }
    }

   

    string NextMessage()
    {
        switch (order)
        {
            case 0:
                order++;
                return UIexplanation;
            case 1:
                order++;
                return TimerExplanation;
            case 2:
                order++;
                return ChatBoxExplanation;
            case 3:
                order++;
                return TargetSystemUI;
            case 4:
                order++;
                return ButtonsExplanation;
        }
        return "Error 37";//error message
    }

    public void BlinkTutorial()//display tutorial for blink
    {
        if (GameObject.Find("Don").GetComponent<TutorialEvent>().order >= 5)
            GameObject.Find("Don").gameObject.transform.Find("Canvas").Find("Panel").Find("Text").gameObject.GetComponent<Text>().text = "By pressing the blink button you blink to someone around the table. In order for someone to see this he needs to look directly to you. Be carefull though, other players can also see you blinking if they look directly to you.";
    }

    public void WaveTutorial()//display tutorial for wave
    {
        if (GameObject.Find("Don").GetComponent<TutorialEvent>().order >= 5)
            GameObject.Find("Don").gameObject.transform.Find("Canvas").Find("Panel").Find("Text").gameObject.GetComponent<Text>().text = "By pressing the wave button you wave to someone around the table. In order for someone to see this he needs to look directly to you. Be carefull though, other players can also see you waving if they look directly to you. Moreover they might also notice your hand moving too.";
    }

    public void BlameTutorial()//display tutorial for blame
    {
        if (GameObject.Find("Don").GetComponent<TutorialEvent>().order >= 5)
            GameObject.Find("Don").gameObject.transform.Find("Canvas").Find("Panel").Find("Text").gameObject.GetComponent<Text>().text = "By pressing the blame button you blame someone of being a killer. This is a global emote and anyone can see this if they look at the bottom left of their screen.";
    }

    public void SuspectTutorial()//display tutorial for suspect
    {
        if (GameObject.Find("Don").GetComponent<TutorialEvent>().order >= 5)
            GameObject.Find("Don").gameObject.transform.Find("Canvas").Find("Panel").Find("Text").gameObject.GetComponent<Text>().text = "By pressing the suspect button you inform the others that someone might be a killer. Use this if you think that someone is acting weird.This is a global emote and anyone can see this if they look at the bottom left of their screen.";
    }

    public void VoteTutorial()//display tutorial for suspect
    {
        if (GameObject.Find("Don").GetComponent<TutorialEvent>().order >= 5)
            GameObject.Find("Don").gameObject.transform.Find("Canvas").Find("Panel").Find("Text").gameObject.GetComponent<Text>().text = "By pressing the vote button you can vote someone to get disquilified if you think that player is one of the killers. Players that get disquilified will have an X mark above their heads. Innocents can only vote in the day while the killers can vote during the day and the night. Also during the night, an explanation mark will appear above their head.";
    }

    public void ExitTutorial() //Go back to the main game scene
    {
        SceneManager.LoadScene(0);
    }

}
