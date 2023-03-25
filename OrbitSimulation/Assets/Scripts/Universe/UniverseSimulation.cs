using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniverseSimulation : MonoBehaviour
{
    CelestialBody[] bodies;
    public float timeScale = 0.05f;

    void Awake()
    {
        bodies = FindObjectsOfType<CelestialBody>();
    }


    void Update()
    {
        for (int i = 0; i < bodies.Length; i++)
        {
            bodies[i].UpdateVelocity(bodies, timeScale);
        }

        for(int i = 0; i < bodies.Length; i++)
        {
            bodies[i].UpdatePosition(timeScale);
        }
    }
}
