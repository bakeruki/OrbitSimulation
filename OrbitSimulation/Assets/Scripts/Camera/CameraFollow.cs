using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject focusBody;
    public float zoom;

    float defaultY = 12178;
    
    void Update()
    {
        if(zoom != 0)
        {
            transform.position = new Vector3(focusBody.transform.position.x, defaultY / zoom, focusBody.transform.position.z);
        }
        else
        {
            transform.position = new Vector3(focusBody.transform.position.x, defaultY, focusBody.transform.position.z);
        }
    }
}
