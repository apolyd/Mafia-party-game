using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camMouseLook : Photon.MonoBehaviour
{
    
    public bool cursorFlag; //flag to release/lock the mouse
    public bool disableCamera; //flag to disable the camera so it doesnt move
    [SerializeField]
    public Vector2 mouseLook;
    Vector2 smoothV;
    public float sensitivity = 5.0f;
    public float smoothing = 2.0f;
    GameObject character,capsule;
    //for android
    private Vector3 firstpoint;
    private Vector3 secondpoint;
    public float xAngle = 0.0f; //angle for axes x for rotation
    public float yAngle = 0.0f;
    private float xAngTemp = 0.0f; //temp variable for angle
    private float yAngTemp = 0.0f;

    
    void Start()
    {
        if (Application.platform == RuntimePlatform.Android) //if we are on android
        {
            firstpoint = new Vector3(0, 0, 0);
            secondpoint = new Vector3(0, 0, 0);
            xAngle = 0.0f;
            yAngle = 0.0f;
            transform.rotation = Quaternion.Euler(yAngle, xAngle, 0.0f);
        }
        
        character = this.transform.parent.gameObject;
        capsule = this.transform.parent.parent.gameObject;
        if (photonView.isMine)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false; //cursor not visible
            cursorFlag = true;
            disableCamera = false;
        }
    }

    
    void Update()
    {
        if (photonView.isMine)
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                if (Input.touchCount > 0)
                {
                    //Touch began, save position
                    if (Input.GetTouch(0).phase == TouchPhase.Began)
                    {
                        firstpoint = Input.GetTouch(0).position;
                        xAngTemp = xAngle;
                        yAngTemp = yAngle;
                    }
                    //Move finger by screen
                    if (Input.GetTouch(0).phase == TouchPhase.Moved)
                    {
                        secondpoint = Input.GetTouch(0).position;
                        //Mainly, about rotate camera. For example, for Screen.width rotate on 180 degree
                        yAngle = yAngTemp - (secondpoint.y - firstpoint.y) * 90.0f / Screen.height;

                        yAngle = Mathf.Clamp(yAngle, 0f, 0f);
                        

                        xAngle = xAngTemp + (secondpoint.x - firstpoint.x) * 180.0f / Screen.width;

                       /// xAngle = Mathf.Clamp(xAngle, -80f, 80f);

                        transform.rotation = Quaternion.Euler(yAngle, xAngle, 0.0f);
                        character.transform.localRotation = Quaternion.AngleAxis(xAngle, character.transform.up);
                    }
                }
            }
            else
            {
                if (disableCamera == false)
                {
                    var md = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));


                    md = Vector2.Scale(md, new Vector2(sensitivity * smoothing, sensitivity * smoothing));
                    smoothV.x = Mathf.Lerp(smoothV.x, md.x, 1f / smoothing);
                    smoothV.y = Mathf.Lerp(smoothV.y, md.y, 1f / smoothing);
                    mouseLook += smoothV;
                    mouseLook.y = Mathf.Clamp(mouseLook.y, -90f, 90f);
                    mouseLook.x = Mathf.Clamp(mouseLook.x, -80f, 80f);
                    // Debug.Log(mouseLook.y);
                    if (mouseLook.y > 0)  //don't have to look up do it is disabled but can be enable again if needed
                    {
                        mouseLook.y = 0;
                    }
                    if (mouseLook.y < 0)
                    {
                        mouseLook.y = 0;
                    }

                    //  if (mouseLook.x > 80)
                    //  {
                    //      mouseLook.x = 80;
                    //  }
                    //  if (mouseLook.x < -80)
                    //   {
                    //       mouseLook.x = -80;
                    //   }

                    transform.localRotation = Quaternion.AngleAxis(-mouseLook.y, Vector3.right);
                    character.transform.localRotation = Quaternion.AngleAxis(mouseLook.x, character.transform.up);
                    //capsule.transform.localRotation = Quaternion.AngleAxis(mouseLook.x, character.transform.up);
                }
            }

            

            

            if (Input.GetKeyDown(KeyCode.Escape) && cursorFlag == true) //release cursor
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true; //cursor visible
                cursorFlag = false;
                disableCamera = true;
            }
            else if(Input.GetKeyDown(KeyCode.Escape) && cursorFlag == false) //lock cursor
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                cursorFlag = true;
                disableCamera = false;
            }

        }
    }

   
}
