using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CameraFollow : MonoBehaviour
{
    public CelestialBody focusBody;
    public float zoom;

    float defaultY = 20000;
    
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
