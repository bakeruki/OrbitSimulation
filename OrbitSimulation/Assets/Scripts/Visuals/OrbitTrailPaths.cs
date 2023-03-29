using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitTrailPaths : MonoBehaviour
{
    public bool drawTrails = true;
    public bool useRelativeBody = false;
    public int numPoints = 1000;

    CelestialBody[] bodies;
    Vector3[][] drawPoints;
    Camera mainCam;

    CelestialBody relativeBody;
    Vector3 relativeBodyInitialPosition;
    int relativeBodyIndex;
    void Start()
    {
        bodies = FindObjectsOfType<CelestialBody>();
        drawPoints = new Vector3[bodies.Length][];
        mainCam = FindObjectOfType<Camera>();
        relativeBody = mainCam.GetComponent<CameraFollow>().focusBody;
        relativeBodyInitialPosition = relativeBody.transform.position;

        //initialize drawing points array
        for(int i = 0; i < bodies.Length; i++)
        {
            drawPoints[i] = new Vector3[numPoints];
            for(int j = 0; j < numPoints; j++)
            {
                drawPoints[i][j] = Vector3.zero;
            }

            if (bodies[i] == relativeBody)
            {
                relativeBodyIndex = i;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (drawTrails)
        {
            DrawTrails();
        }   
    }

    void DrawTrails()
    {
        //loop through each planet
        for(int i = 0; i < bodies.Length; i++)
        {
            //loop through all of the current draw points
            for(int j = drawPoints[i].Length - 1; j > 0; j--)
            {
                //make room for the new point while keeping the old ones
                drawPoints[i][j] = drawPoints[i][j - 1];
            }

            //add the new point
            Vector3 newPoint = bodies[i].transform.position;
           
            /*
             * THIS CODE IS BROKEN
            if (useRelativeBody)
            {
                //calculate how much the relative body has moved
                Vector3 relativeBodyOffset = relativeBodyInitialPosition;
                newPoint -= relativeBodyOffset;
            }
            if(i == relativeBodyIndex && useRelativeBody)
            {
                newPoint = relativeBodyInitialPosition;
            }
            */
            drawPoints[i][0] = newPoint;
        }

        //draw the actual points
        for(int i = 0; i < bodies.Length; i++)
        {
            Color color = bodies[i].GetComponent<MeshRenderer>().sharedMaterial.color;
            for (int j = 0; j < drawPoints[i].Length - 1; j++)
            {
                if (drawPoints[i][j] == Vector3.zero || drawPoints[i][j+1] == Vector3.zero)
                {
                    continue;
                }
                Debug.DrawLine(drawPoints[i][j], drawPoints[i][j + 1], color);
            }
        }
    }
}
