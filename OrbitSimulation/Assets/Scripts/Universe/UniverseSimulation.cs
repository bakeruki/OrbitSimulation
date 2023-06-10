using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniverseSimulation : MonoBehaviour
{
    //real constant: 0.0000000000667f
    public static float GravitationalConstant = 66.7f;
    public static CelestialBody CentralBody;
    public float timeScale = 1f;

    CelestialBody[] bodies;

    void Start()
    {
        bodies = FindObjectsOfType<CelestialBody>();
        CentralBody = GameObject.FindGameObjectWithTag("CentralBody").GetComponent<CelestialBody>();
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
