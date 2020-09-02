using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;


public class NetworkManager : Photon.MonoBehaviour
{
   
    public Text status,players;
    private const string roomName = "RoomName";
    private TypedLobby lobbyName = new TypedLobby("New_Lobby", LobbyType.Default);
    private RoomInfo[] roomsList;
    public GameObject player, camera, mainMenuCanvas, position;
    public int[] RolePlayers = { 0, 0, 0, 0, 0, 0, 0, 0 };//the roles of the players
    public int[] DisquilifiedPlayers = { 0, 0, 0, 0, 0, 0, 0, 0 };//who is disqualified
    public int[] VotesPlayers = { 0, 0, 0, 0, 0, 0, 0, 0 };//how are the votes distributed
    public bool gameStarted = false; //signal to start the countdown to all players
    public int criminal1,criminal2; //let's save the positions of the criminals here so that we use them later on faster. Only master knows this
    public int Endgame = 0; //flag to end the game
    public string nickname;
    public InputField Inputfield;
    
    
    //public bool resetSignal;

    void Start()
    {
        PhotonNetwork.ConnectUsingSettings("v4.2");
        status.text = "Wait...";
        
    }

    
    void Update()
    {
        if (PhotonNetwork.connected)
        {
            status.text = "Connected!";
            status.color = Color.green;
        }
        if (PhotonNetwork.isMasterClient)
        {
            players.text = "I am the master";
            if(PhotonNetwork.room.PlayerCount == 8 && gameStarted == false)// 8players
            {
                int previousFlag = -1; //remember last player so you don't assign him twice
               
                for (int i = 0; i < 2; i++)//we need to start the game here and choose 2 killers and 6 innocent
                {
                    int j = Random.Range(0, 8); //choose a player
                    while (previousFlag == j)
                    {
                        Random.Range(0, 8);
                        previousFlag = j;
                    }
                    previousFlag = j;
                    RolePlayers[j] = 1;
                    if( i == 0)
                    {
                        criminal1 = j;
                    }
                    else
                    {
                        criminal2 = j;                    }
                    
                }

                SetRoles();
                gameStarted = true;  //we change this variable to tell when the game should start
                StartGame(); //send this to everyone
            }

            if(GameObject.FindWithTag("Countdown").GetComponent<TimerRound>().isDay == true)
            {
                CheckVotes();
            }

            if(GameObject.FindWithTag("Countdown").GetComponent<TimerRound>().isDay == false)
            {
                CheckVotesNight();
            }
            
            //check the end conditions for the game
            if(DisquilifiedPlayers[criminal1] == 1 && DisquilifiedPlayers[criminal2] == 1)
            {
                Debug.Log("Innocents win");
            }
            else
            {
                if (DisquilifiedPlayers.Sum() == 6)
                {
                    if(DisquilifiedPlayers[criminal1] == 0 || DisquilifiedPlayers[criminal2] == 0)
                    {
                        Debug.Log("Criminals win");
                    }
                }
            }
           


        }
        else
        {
            players.text = "I am not the master";
        }


    }

    void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby(lobbyName);
    }

    void OnReceivedRoomUpdateList()
    {
        roomsList = PhotonNetwork.GetRoomList();
    }

    void OnJoinedLobby()
    {
        Debug.Log("Joined Lobby");
    }

    void OnJoinedRoom()
    {
       // camera.SetActive(false);
        Debug.Log("Connected to Room");
        camera.SetActive(false);
        mainMenuCanvas.SetActive(false);
      //playerMenuCanvas.SetActive(true);
        GameObject playerPrefab = PhotonNetwork.Instantiate(player.name, position.transform.position, position.transform.rotation, 0);
        //Debug.Log(PhotonNetwork.room.PlayerCount);
      
    }


    public void createRoom()
    {
        PhotonNetwork.CreateRoom(roomName, new RoomOptions() { MaxPlayers = 8, IsOpen = true, IsVisible = true }, lobbyName);
    }

    public void joinRoom()
    {
       
        PhotonNetwork.JoinRoom("RoomName");
        
    }

    public void SetNickname()
    {
        nickname = Inputfield.text;
    }

    void SetRoles()  //send the roles to everyone so that they get updated
    {
        SetRolesPlayer(RolePlayers);
    }

    [PunRPC]
    void SetRolesPlayer(int[] pos)  //send the roles to the others
    {
        RolePlayers[0] = pos[0];
        RolePlayers[1] = pos[1];
        RolePlayers[2] = pos[2];
        RolePlayers[3] = pos[3];
        RolePlayers[4] = pos[4];
        RolePlayers[5] = pos[5];
        RolePlayers[6] = pos[6];
        RolePlayers[7] = pos[7];

        if (PhotonNetwork.isMasterClient)
        {
            photonView.RPC("SetRolesPlayer", PhotonTargets.OthersBuffered, RolePlayers);
        }

    }

    void StartGame()  //start the game
    {
        StartGameAll(gameStarted);
    }

    [PunRPC]
    void StartGameAll(bool start) //send the start to everyone else so that we get going
    {
        gameStarted = start;

        if (PhotonNetwork.isMasterClient)
        {
            photonView.RPC("StartGameAll", PhotonTargets.OthersBuffered, start);
        }

    }

    void CheckVotes() //check the votes to see if everyone has voted and when this happens 1)kick the player 2)go to night 3)reset for morning
    {
        int sumOfVotes = VotesPlayers.Sum();
        int sumOfEligible = DisquilifiedPlayers.Sum();
        int sumOfAll = sumOfVotes + sumOfEligible;
        GameObject countdown = GameObject.FindGameObjectWithTag("Countdown");
        if (sumOfAll == 8 || countdown.GetComponent<TimerRound>().timeRound <= 0.1f) //8
        {
            int maxValue = VotesPlayers.Max();
            int maxIndex = VotesPlayers.ToList().IndexOf(maxValue);
            Array.Clear(VotesPlayers, 0, VotesPlayers.Length);//clear the array of the votes
            DisquilifiedPlayers[maxIndex] = 1;
            Debug.Log("kick " + maxIndex);
            DisquilifyUpdate();   //disquilify
            ResetTimeUpdate();    //reset timer to go to next stage
        }
    }

    void CheckVotesNight()
    {
        int criminalsOut = 0;//how many criminals are out disquilified
        for (int i = 0; i < 8; i++)
        {
            if(DisquilifiedPlayers[i] == 1)
            {
                if(i == criminal1 || i == criminal2)
                {
                    criminalsOut++;
                }
            }
        }
        int sumOfVotes = VotesPlayers.Sum();
        int eligibleVotes = 2 - criminalsOut;//this is how many criminals can vote
        GameObject countdown = GameObject.FindGameObjectWithTag("Countdown");
        if(sumOfVotes == eligibleVotes || countdown.GetComponent<TimerRound>().timeRound <= 0.1f) //if we have all the votes from the criminals
        {
            int maxValue = VotesPlayers.Max();
            int maxIndex = VotesPlayers.ToList().IndexOf(maxValue);
            Array.Clear(VotesPlayers, 0, VotesPlayers.Length);
            DisquilifiedPlayers[maxIndex] = 1;
            DisquilifyUpdate();   //disquilify
            ResetTimeUpdate();    //reset timer to go to next stage
        }


    }

    void ResetTimeUpdate()
    {
        ResetTime();
    }

    [PunRPC]
    void ResetTime()
    {
        GameObject.FindWithTag("Countdown").GetComponent<TimerRound>().timeRound = -1f;
        if (PhotonNetwork.isMasterClient)
        {
            photonView.RPC("ResetTime", PhotonTargets.Others);
        }
    }

    void DisquilifyUpdate()//send a message to kick someone
    {
        Disquilify(DisquilifiedPlayers);
    }

    [PunRPC]
    void Disquilify(int[] disqPlayers)//send a message to update someone that needs to be disquikified
    {
        DisquilifiedPlayers[0] = disqPlayers[0];
        DisquilifiedPlayers[1] = disqPlayers[1];
        DisquilifiedPlayers[2] = disqPlayers[2];
        DisquilifiedPlayers[3] = disqPlayers[3];
        DisquilifiedPlayers[4] = disqPlayers[4];
        DisquilifiedPlayers[5] = disqPlayers[5];
        DisquilifiedPlayers[6] = disqPlayers[6];
        DisquilifiedPlayers[7] = disqPlayers[7];

        if (PhotonNetwork.isMasterClient)
        {
            photonView.RPC("Disquilify", PhotonTargets.OthersBuffered, DisquilifiedPlayers);
        }
    }

    void EndScreenUpdate(int num)
    {
        EndScreen(num);
    }

    [PunRPC]
    void EndScreen(int num)
    {
        if(num == 1)
        {
            Endgame = 1;//innocents win
        }
        else
        {
            Endgame = 2;//criminals win
        }
        if (PhotonNetwork.isMasterClient)
        {
            photonView.RPC("EndScreen", PhotonTargets.OthersBuffered, num);
        }
    }

}
