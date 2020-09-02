using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vote : Photon.MonoBehaviour
{
    public bool hasVoted;
    public bool isEligibleToVote;
    public bool isCriminal;
    public GameObject countdown;
    public bool resetVote; // reset the vote when we change from day to night.Just keep the last state of isDay variable and when it changes
    // Start is called before the first frame update
    void Start()
    {
        hasVoted = false;
        isEligibleToVote = true;
        isCriminal = false;
        countdown = GameObject.FindWithTag("Countdown");
        resetVote = countdown.GetComponent<TimerRound>().isDay;


    }

    // Update is called once per frame
    void Update()
    {
       if(resetVote != countdown.GetComponent<TimerRound>().isDay) //reset the hasVoted so that we can vote again
       {
            resetVote = !resetVote;
            hasVoted = false;
       }

      // if(gameObject.GetComponent<NetworkManager>().DisquilifiedPlayers[gameObject.GetComponent<PlayerInitialization>().playerNumber] == 1) //cant vote if u are disqulified ut this should not even be possible
      // {
       //     isEligibleToVote = false;
       //}

    }

    public void InitiateVote()
    {
        int playerNumber = 0;
        if (countdown.GetComponent<TimerRound>().isDay == true) //if it is day 
        {
            if (isEligibleToVote == true && hasVoted == false) //we have not been eliminated and we haven't voted yet then we vote
            {
                switch (gameObject.GetComponent<isLookingAt>().lookObject)
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
                GameObject networkManager = GameObject.FindWithTag("NetworkManager");
                if (networkManager.GetComponent<NetworkManager>().DisquilifiedPlayers[playerNumber] == 1)
                {
                    return; //you can't vote someone who is disquilified
                }
                else
                {
                    hasVoted = true; //i need to reset this somewhere?
                    Voting(playerNumber);
                }
               
                    

            }

        } else if (countdown.GetComponent<TimerRound>().isDay == false)//if it is night 
        {
            if (isEligibleToVote == true && hasVoted == false /*&& isCriminal == true*/) //we have not been eliminated and we haven't voted yet then we vote
            {
                switch (gameObject.GetComponent<isLookingAt>().lookObject)
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
                GameObject networkManager = GameObject.FindWithTag("NetworkManager");
                if (networkManager.GetComponent<NetworkManager>().DisquilifiedPlayers[playerNumber] == 1 || networkManager.GetComponent<NetworkManager>().criminal1 == playerNumber || networkManager.GetComponent<NetworkManager>().criminal2 == playerNumber)
                {
                    return; //you can't vote someone who is disquilified or is a fellow criminal
                }
                else
                {
                    hasVoted = true; //i need to reset this somewhere?
                    Voting(playerNumber);
                }

                
                
            }
        }

        
    }

    [PunRPC]
    void Voting(int num)
    {
        
        if (photonView.isMine && !PhotonNetwork.isMasterClient) //send this to masterclient and if u are the master client don't send it anywhere just update the votes duh
        {
            photonView.RPC("Voting", PhotonTargets.MasterClient, num);
        }
        else
        {
           GameObject networkManager = GameObject.FindWithTag("NetworkManager");
           networkManager.GetComponent<NetworkManager>().VotesPlayers[num]++;
          // Debug.Log(networkManager.GetComponent<NetworkManager>().VotesPlayers[num]);
        }
    }
}
