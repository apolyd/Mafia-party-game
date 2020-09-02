using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovementMenu : MonoBehaviour
{
    float rotationSpeed = 2;
    float x = 19.071f;
    float y = 218.6f;
    float z = 0.8430001f;
    int changeRotationFlag = 0;


    // Update is called once per frame
    void Update()
    {
        if (y <= 219f && changeRotationFlag == 0)
        {
            y -= Time.deltaTime * rotationSpeed;
            transform.rotation = Quaternion.Euler(x, y, z);
        }

        if (y <= 170 || changeRotationFlag == 1)
        {
            changeRotationFlag = 1;
            y += Time.deltaTime * rotationSpeed;
            transform.rotation = Quaternion.Euler(x, y, z);
            if (y >= 218f)
            {
                changeRotationFlag = 0;
            }
        }
    }


}

