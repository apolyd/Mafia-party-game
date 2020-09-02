using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tele : Photon.MonoBehaviour,IPunObservable
{
    public GameObject teleport;
    public Material[] material;
    private GameObject manager;
    public int test = 5;
    // Start is called before the first frame update
    void Awake()
    {
        if (photonView.isMine)
        {
            gameObject.transform.Find("Sphere").Find("Camera").gameObject.SetActive(true);
            Debug.Log("This is what I see");
            manager = GameObject.FindGameObjectWithTag("NetworkManager");
            InputColorChange();
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.isMine)
        {
            if (Input.GetKey(KeyCode.W))
            {
                transform.position = teleport.transform.position;
            }
        }
        test++;
    }

    void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            Debug.Log("Stelnw");
            //send the others our data
            stream.SendNext(test);
        }
        else
        {  //others receive i guess
            test = (int)stream.ReceiveNext();
        }
    }

    void InputColorChange()
    {
        ChangeColorTo(PhotonNetwork.room.PlayerCount);
    }

    [PunRPC]
    void ChangeColorTo(int num)
    {
        GetComponent<Renderer>().material = material[num];

        if (photonView.isMine)
        {
            photonView.RPC("ChangeColorTo", PhotonTargets.OthersBuffered, num);
        }
    }
}
