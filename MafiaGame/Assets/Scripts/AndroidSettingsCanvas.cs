using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AndroidSettingsCanvas : MonoBehaviour
{
    private void Awake()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            gameObject.GetComponent<Canvas>().scaleFactor = 2;
        }
    }
}
