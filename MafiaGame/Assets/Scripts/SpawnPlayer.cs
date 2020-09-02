using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPlayer : MonoBehaviour
{
    public GameObject Player;
    public GameObject[] SpawnPoint;

     void Spawn()
    {
        GameObject Example = Instantiate(Player, SpawnPoint[0].transform.position, Quaternion.identity);
        Example.gameObject.tag = "Test";
        //Instantiate(Player, transform.position, transform.rotation);
    }
    
    
   
}
