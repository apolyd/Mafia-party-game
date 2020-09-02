using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class isLookingAt : Photon.MonoBehaviour
{
    public string lookObject;
    public GameObject playerCanvas;
    public string whoAmI;
    
    void Start()
    {
        playerCanvas = GameObject.FindWithTag("PlayerCanvas");
        whoAmI = gameObject.tag;
    }


    void Update()
    {

        var ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)); //Look the middle of the camera to check who are u looking at
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 10f))
        {
            var selection = hit.transform;
            Debug.Log("Vlepw to "+selection.gameObject.tag);
            lookObject = selection.gameObject.tag;
            if (lookObject != "Untagged")//if u are looking at something that is tagged display it at the screen
            {
                playerCanvas.gameObject.transform.Find("GlobalText1").gameObject.gameObject.GetComponent<Text>().text = "You are looking at " + GameObject.FindGameObjectWithTag(lookObject).GetComponent<PlayerInitialization>().nickname;
            }
            else
            {
                playerCanvas.gameObject.transform.Find("GlobalText1").gameObject.gameObject.GetComponent<Text>().text = " ";
            }
            
            //if (selection.CompareTag("Slime"))
        }

    }

    public void blinkAt()
    {
        if (lookObject != "Untagged")
            blinkingAt(lookObject,whoAmI);
    }

    public void waveAt()
    {
        if (Application.platform == RuntimePlatform.Android)//if we are on android send different angles
        {
            if (gameObject.transform.Find("Sphere").Find("Camera").GetComponent<camMouseLook>().xAngle >= 0)
            {
                PlayWaveAnimation(1);//play the animation for everyone
            }
            else
            {
                PlayWaveAnimation(0);
            }
        }
        else
        {
            if (gameObject.transform.Find("Sphere").Find("Camera").GetComponent<camMouseLook>().mouseLook.x >= 0)
            {
                PlayWaveAnimation(1);//play the animation for everyone
            }
            else
            {
                PlayWaveAnimation(0);
            }
        }
       
            
        if (lookObject != "Untagged")
            wavingAt(lookObject,gameObject.tag);
    }

    public void suspectAt()
    {
        if (lookObject != "Untagged")
            suspectingAt(lookObject,gameObject.tag);
    }

    public void blameAt()
    {
        if (lookObject != "Untagged")
            blamingAt(lookObject,gameObject.tag);
    }

    //remote call this to everyone else in the room in order to blink at someone.Can only been seen if the player is watching you or can been seen by other if they look at you
    [PunRPC]
    void blinkingAt(string tagName, string tagWho)
    {
        
        
        if (photonView.isMine)
        {
            photonView.RPC("blinkingAt", PhotonTargets.Others, lookObject,whoAmI);
          //  playerCanvas.gameObject.transform.Find("EventsPanel").Find("Message3").gameObject.GetComponent<Text>().text = "Was called by initialy " + whoAmI;
        }
        else
        {
           // playerCanvas.gameObject.transform.Find("EventsPanel").Find("Message3").gameObject.GetComponent<Text>().text = "Was called by " + whoAmI;
            if (lookObject.Equals(tagWho))
            {
                if(whoAmI.Equals(tagName))
                {
                    playerCanvas.gameObject.transform.Find("EventsPanel").Find("Message1").gameObject.GetComponent<Text>().text = tagWho + " is blinking to you";
                }
                else
                {
                    playerCanvas.gameObject.transform.Find("EventsPanel").Find("Message1").gameObject.GetComponent<Text>().text = GameObject.FindGameObjectWithTag(tagWho).GetComponent<PlayerInitialization>().nickname + "is blinking to "+ GameObject.FindGameObjectWithTag(tagName).GetComponent<PlayerInitialization>().nickname;
                }
                
            }
          
        }
    }
    //remote call this to everyone else in the room in order to wave at someone.Can only been seen if the player is watching you or can been seen by other if they look at you
    [PunRPC]
    void wavingAt(string tagName, string tagWho)
    {


        if (photonView.isMine)
        {
            photonView.RPC("wavingAt", PhotonTargets.Others, lookObject, gameObject.tag);
        }
        else
        {
           // GameObject.FindGameObjectWithTag(tagWho).gameObject.transform.Find("RightHand").gameObject.GetComponent<Animator>().Play("Wave"); //send the animation to the others
            if (lookObject.Equals(tagWho))
            {
                if (gameObject.tag.Equals(tagName))
                {
                    playerCanvas.gameObject.transform.Find("EventsPanel").Find("Message2").gameObject.GetComponent<Text>().text = tagWho + " is waving to you";
                }
                else
                {
                    playerCanvas.gameObject.transform.Find("EventsPanel").Find("Message2").gameObject.GetComponent<Text>().text = GameObject.FindGameObjectWithTag(tagWho).GetComponent<PlayerInitialization>().nickname + " is waving to " + GameObject.FindGameObjectWithTag(tagName).GetComponent<PlayerInitialization>().nickname;
                }

            }

        }
    }

    //remote call this to everyone else in the room in order to blame someone.This can be seen by anyone besides you in the room
    [PunRPC]
    void blamingAt(string tagName,string tagWho)
    {


        if (photonView.isMine)
        {
            photonView.RPC("blamingAt", PhotonTargets.Others, lookObject,gameObject.tag);
        }
        else
        {
            playerCanvas.gameObject.transform.Find("EventsPanel").Find("Message3").gameObject.GetComponent<Text>().text = GameObject.FindGameObjectWithTag(tagWho).GetComponent<PlayerInitialization>().nickname + " is blaming " + GameObject.FindGameObjectWithTag(tagName).GetComponent<PlayerInitialization>().nickname + " as killer";
        }
    }

    //remote call this to everyone else in the room in order to suspect someone.This can be seen by anyone besides you in the room 
    [PunRPC]
    void suspectingAt(string tagName, string tagWho)
    {

        if (photonView.isMine)
        {
            photonView.RPC("suspectingAt", PhotonTargets.Others, lookObject, gameObject.tag);
        }
        else
        {
            playerCanvas.gameObject.transform.Find("EventsPanel").Find("Message4").gameObject.GetComponent<Text>().text = GameObject.FindGameObjectWithTag(tagWho).GetComponent<PlayerInitialization>().nickname + " is suspecting " + GameObject.FindGameObjectWithTag(tagName).GetComponent<PlayerInitialization>().nickname + " as killer";
        }
    }

    [PunRPC]
    void PlayWaveAnimation(int direction)
    {
        if(direction == 1)
        {
            gameObject.transform.Find("LeftHand").gameObject.GetComponent<Animator>().Play("WaveLeft"); //send the animation to the others no matter what and play it for this current object
        }
        else
        {
            gameObject.transform.Find("RightHand").gameObject.GetComponent<Animator>().Play("Wave"); //send the animation to the others no matter what and play it for this current object
        }
        
        if (photonView.isMine)
        {
            photonView.RPC("PlayWaveAnimation", PhotonTargets.Others, direction);
        }
    }

}
