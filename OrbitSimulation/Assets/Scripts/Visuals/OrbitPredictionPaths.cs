using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class OrbitPredictionPaths : MonoBehaviour
{
    public float gravitationalConstant;
    UniverseSimulation simulation;
    public bool drawOrbitLines = false;
    public int iterations;

    public Vector3[][] drawPoints;
    public bool useRelativeBody;
    public CelestialBody relativeBody;

    void OnRenderObject()
    {
        if (drawOrbitLines && !Application.isPlaying)
        {
            DrawOrbitLines();
        }
    }

    void DrawOrbitLines()
    {
        Vector3 relativeBodyInitialPosition = Vector3.zero;
        CelestialBody[] realBodies = FindObjectsOfType<CelestialBody>();
        SimulatedBody[] simBodies = new SimulatedBody[realBodies.Length];
        drawPoints = new Vector3[simBodies.Length][];
        simulation = FindObjectOfType<UniverseSimulation>();
        int relativeBodyIndex = 0;

        //create the simulated bodies
        for (int i = 0; i <  realBodies.Length; i++)
        {
            drawPoints[i] = new Vector3[iterations];
            simBodies[i] = new SimulatedBody(realBodies[i].mass, realBodies[i].velocity, realBodies[i].transform.position);

            if (realBodies[i] == relativeBody && useRelativeBody)
            {
                relativeBodyIndex = i;
                relativeBodyInitialPosition = simBodies[relativeBodyIndex].position;
            }
        }

        //simulate the paths of each body
        for(int i = 0; i < iterations; i++)
        {
            //calculating velocity of all bodies first and then calculating
            //acceleration gives a more stable simulation (although its more costly)
            for(int j = 0; j < simBodies.Length; j++)
            {
                //calculate each bodies acceleration
                Vector3 acceleration = CalculateTotalAccelerationOfBody(simBodies[j], simBodies);
                //update each bodies velocity based on acceleration
                UpdateVelocityOfBody(simBodies[j], acceleration);
            }

            for(int k = 0; k < simBodies.Length; k++)
            {
                Vector3 relativeBodyPosition = simBodies[relativeBodyIndex].position;
                //calculate each bodies new position
                Vector3 newPosition = simBodies[k].position + simBodies[k].velocity * simulation.timeScale;
                simBodies[k].position = newPosition;

                if (useRelativeBody)
                {
                    Vector3 relativeBodyOffset = relativeBodyPosition - relativeBodyInitialPosition;
                    newPosition -= relativeBodyOffset;
                }
               
                if (k == relativeBodyIndex && useRelativeBody)
                {
                    newPosition = relativeBodyInitialPosition;
                }

                //add the new position of each body to the array of points 
                drawPoints[k][i] = newPosition;
            }
        }

        //draw the orbit lines
        for(int i = 0; i < simBodies.Length; i++)
        {
            Color color = realBodies[i].GetComponent<MeshRenderer>().sharedMaterial.color; 
            for(int j = 0; j < drawPoints[i].Length - 1; j++)
            {
                Debug.DrawLine(drawPoints[i][j], drawPoints[i][j + 1], color);
            }
        }
    }
    void UpdateVelocityOfBody(SimulatedBody body, Vector3 acceleration)
    {
        body.velocity += acceleration * simulation.timeScale;
    }

    Vector3 CalculateTotalAccelerationOfBody(SimulatedBody body, SimulatedBody[] bodies)
    {
        Vector3 acceleration = Vector3.zero;

        for(int i = 0; i < bodies.Length; i++)
        {
            if (bodies[i] != body)
            {
                Vector3 direction = (bodies[i].position - body.position).normalized;
                float magnitudeSquared = (bodies[i].position - body.position).sqrMagnitude;
                acceleration += direction * gravitationalConstant * bodies[i].mass / magnitudeSquared;
            }
        }

        return acceleration;
    }

    class SimulatedBody
    {
        public float mass;
        public Vector3 velocity;
        public Vector3 position;

        public SimulatedBody(float mass, Vector3 velocity, Vector3 position)
        {
            this.mass = mass;
            this.velocity = velocity;
            this.position = position;
        }
    }
}
