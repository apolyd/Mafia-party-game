using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInitialization : Photon.MonoBehaviour
{
    public Material[] material;
    public GameObject[] waypoint;
    public GameObject teleport,playerMenuCanvas,networkManager,countdown,innocentCanvas;
    public int playerNumber;
    public bool edw;
    public string nickname;
    

    void Awake()
    {
        if (photonView.isMine)
        {
            gameObject.transform.Find("Sphere").Find("Camera").gameObject.SetActive(true);
            //gameObject.transform.Find("ExplanationMark").gameObject.SetActive(true);
            gameObject.transform.Find("InnocentCanvas").gameObject.SetActive(false);
            gameObject.transform.Find("PlayerCanvas").gameObject.SetActive(true);
            countdown = GameObject.FindWithTag("Countdown");
            MaterialChange();
            //maybe we setup position after awake in start to face the right direction
            SetTag();
            SetNickname();
            playerMenuCanvas = GameObject.FindWithTag("PlayerCanvas");  //get the player canvas
            playerMenuCanvas.transform.Find("Role").gameObject.GetComponent<Text>().text = "Your role will be displayed here "+nickname;
            playerMenuCanvas.transform.Find("TimerRound").gameObject.GetComponent<Text>().text = "Your timer will be displayed here " + nickname;
            networkManager = GameObject.FindWithTag("NetworkManager"); //get the network manager
           // Debug.Log(gameObject.tag);
            switch (gameObject.tag)    //depending on the tag asign a player number player1 = 1 etc
            {
                case "Player1":
                    playerNumber = 0;
                    break;
                case "Player2":
                    playerNumber = 1;
                    break;
                case "Player3":
                    playerNumber = 2;
                    break;
                case "Player4":
                    playerNumber = 3;
                    break;
                case "Player5":
                    playerNumber = 4;
                    break;
                case "Player6":
                    playerNumber = 5;
                    break;
                case "Player7":
                    playerNumber = 6;
                    break;
                case "Player8":
                    playerNumber = 7;
                    break;
                default:
                    return;
            }

            
        }

    }
    
    void Start()  //propably we don't do anything here
    {
        SetPosition();
    }

    
    void Update()
    {
        if (photonView.isMine)
        {
            playerMenuCanvas = GameObject.FindWithTag("PlayerCanvas"); //propably dont need to search it again but let's do it
           // playerMenuCanvas.transform.Find("GlobalText1").gameObject.GetComponent<Text>().text = networkManager.GetComponent<NetworkManager>().gameStarted.ToString();
           if(networkManager.GetComponent<NetworkManager>().Endgame == 1)//check conditions for win
            {
                gameObject.transform.Find("PlayerCanvas").transform.Find("GlobalText2").gameObject.GetComponent<Text>().text = "Innocents win!";
            }else if(networkManager.GetComponent<NetworkManager>().Endgame == 2)
            {
                gameObject.transform.Find("PlayerCanvas").transform.Find("GlobalText2").gameObject.GetComponent<Text>().text = "Crimianls win!";
            }
           
            if (networkManager.GetComponent<NetworkManager>().gameStarted == true)
            {
                //  Debug.Log("to gamestarted einai true");
                gameObject.transform.Find("PlayerCanvas").transform.Find("TimerRound").gameObject.GetComponent<Text>().text = countdown.GetComponent<TimerRound>().timeRound.ToString();
                if (networkManager.GetComponent<NetworkManager>().RolePlayers[playerNumber] == 1) //if number equals 1 we are criminal just the the text so that the player know he is a criminal
                {
                    gameObject.transform.Find("PlayerCanvas").transform.Find("Role").gameObject.GetComponent<Text>().text = "Criminal";
                    gameObject.GetComponent<Vote>().isCriminal = true;
                }
                else
                {
                    gameObject.transform.Find("PlayerCanvas").transform.Find("Role").gameObject.GetComponent<Text>().text = "Innocent";
                    gameObject.GetComponent<Vote>().isCriminal = false;
                }

                if(networkManager.GetComponent<NetworkManager>().DisquilifiedPlayers[playerNumber] == 1) //things that happen when u get disquilified
                {
                    DisquilifyMe();
                }

                if(countdown.GetComponent<TimerRound>().isDay == false && networkManager.GetComponent<NetworkManager>().DisquilifiedPlayers[playerNumber] == 0) // it's night and we are not disquilified
                {
                    if(networkManager.GetComponent<NetworkManager>().RolePlayers[playerNumber] == 0)// and we are innocent so maybe we should see the black screen also disable canvas
                    {
                        gameObject.transform.Find("Sphere").Find("Camera").gameObject.SetActive(false);//turn off camera
                        gameObject.transform.Find("Sphere").Find("CameraInnocent").gameObject.SetActive(true); //black screen camera
                        gameObject.transform.Find("PlayerCanvas").transform.gameObject.SetActive(false); //close normal canvas
                        gameObject.transform.Find("InnocentCanvas").transform.gameObject.SetActive(true);
                        gameObject.transform.Find("InnocentCanvas").transform.gameObject.transform.Find("Time").gameObject.GetComponent<Text>().text = countdown.GetComponent<TimerRound>().timeRound.ToString();
                    }
                    else //we are criminals we need to enable the explanation mark so the other criminal can see us
                    {
                        EnableExplanationMark(1);
                    }
                    
                }else if(countdown.GetComponent<TimerRound>().isDay == true && networkManager.GetComponent<NetworkManager>().DisquilifiedPlayers[playerNumber] == 0) //it's day everyone should act normal
                {
                    gameObject.transform.Find("Sphere").Find("Camera").gameObject.SetActive(true);//turn on camera
                    gameObject.transform.Find("Sphere").Find("CameraInnocent").gameObject.SetActive(false); //black screen camera off
                    gameObject.transform.Find("PlayerCanvas").transform.gameObject.SetActive(true); //close normal canvas
                    gameObject.transform.Find("InnocentCanvas").gameObject.SetActive(false); //close normal canvas
                    EnableExplanationMark(0);
                }
                
            }//end of the game somewhere here
        }
    }

    

    //change the color of each player depending on when they join the room
    void MaterialChange()
    {
        MaterialChangeTo(PhotonNetwork.room.PlayerCount-1);
    }

    //remote call this to everyone else in the room
    [PunRPC]
    void MaterialChangeTo(int num)
    {
        GetComponent<Renderer>().material = material[num];

        if (photonView.isMine)
        {
            photonView.RPC("MaterialChangeTo", PhotonTargets.OthersBuffered, num);
        }
    }

    void SetPosition()
    {
        SetPositionToTable(PhotonNetwork.room.PlayerCount-1);
    }

    [PunRPC]
    void SetPositionToTable(int pos)
    {
        transform.position = waypoint[pos].transform.position;
        transform.position += new Vector3(0, 0.05f, 0);
        transform.rotation = waypoint[pos].transform.rotation;

        if (photonView.isMine)
        {
            photonView.RPC("SetPositionToTable", PhotonTargets.OthersBuffered, pos);
        }

    }

    void SetTag()
    {
        SetTagPlayer(PhotonNetwork.room.PlayerCount);
    }

    [PunRPC]
    void SetTagPlayer(int pos)
    {
        gameObject.tag = "Player" + pos.ToString();
        gameObject.transform.Find("Sphere").tag = "Player" + pos.ToString();
        gameObject.transform.Find("Sphere").Find("Cube").tag = "Player" + pos.ToString();

        if (photonView.isMine)
        {
            photonView.RPC("SetTagPlayer", PhotonTargets.OthersBuffered, pos);
        }

    }

    void SetNickname()
    {
        SetNicknamePlayer(GameObject.FindGameObjectWithTag("NetworkManager").gameObject.GetComponent<NetworkManager>().nickname);
    }

    [PunRPC]
    void SetNicknamePlayer(string name)
    {
        if (System.String.IsNullOrEmpty(name))
        {
            nickname = gameObject.tag;
        }
        else
        {
            nickname = name;
        }
            
        if (photonView.isMine)
        {
            photonView.RPC("SetNicknamePlayer", PhotonTargets.OthersBuffered, name);
        }

    }

    void DisquilifyMe()
    {
        GetDisquilified();
    }

    [PunRPC]
    void GetDisquilified()
    {
        gameObject.transform.Find("Xmark").gameObject.SetActive(true);
        EnableExplanationMark(0);


        if (photonView.isMine)
        {
            gameObject.transform.Find("PlayerCanvas").gameObject.SetActive(false);//maybe this should be inside the isMine but for now its working
            gameObject.transform.Find("InnocentCanvas").gameObject.SetActive(false);
            gameObject.transform.Find("Sphere").Find("CameraInnocent").gameObject.SetActive(false);
            gameObject.transform.Find("Sphere").Find("Camera").gameObject.SetActive(true);
            photonView.RPC("GetDisquilified", PhotonTargets.OthersBuffered);
        }

    }

    void EnableExplanationMark(int state)
    {
        EnableExplanationMarkToCriminals(state);
    }

    //remote call this to everyone else in the room
    [PunRPC]
    void EnableExplanationMarkToCriminals(int state)
    {
        if (state == 1)
        {
            gameObject.transform.Find("ExplanationMark").gameObject.SetActive(true);
        }
        else
        {
            gameObject.transform.Find("ExplanationMark").gameObject.SetActive(false);
        }
        

        if (photonView.isMine)
        {
            photonView.RPC("EnableExplanationMarkToCriminals", PhotonTargets.OthersBuffered, state);
        }
    }

  
}
