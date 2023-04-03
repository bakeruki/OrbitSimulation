using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CelestialBody : MonoBehaviour
{
    public float radius;
    public float mass;
    public float surfaceGravity;
    public Vector3 initialVelocity;

    float gravitationalConstant = UniverseSimulation.GravitationalConstant;

    public Vector3 velocity;
    public Vector3 acceleration;

    public Rigidbody rb;
    
    void OnValidate()
    {
        mass = surfaceGravity * radius * radius / gravitationalConstant;
        transform.localScale = Vector3.one * radius;
        velocity = initialVelocity;
        rb.mass = mass;
    }

    public void UpdateVelocity(CelestialBody[] allBodies, float timeScale)
    {
        Vector3 acceleration = Vector3.zero; 
        for(int i = 0; i < allBodies.Length; i++)
        {
            if (allBodies[i] != this)
            {
                acceleration += CalculateAcceleration(allBodies[i]);
            }
            if (allBodies[i] == UniverseSimulation.CentralBody)
            {
                Debug.Log("Orbital period of " + this.name + " around " + allBodies[i].name + " in minutes: " + CalculateOrbitalPeriod(allBodies[i]) / 60);
            }
        }
        velocity += acceleration * timeScale * Time.deltaTime;
    }

    public void UpdatePosition(float timeScale)
    {
        rb.position += velocity * timeScale * Time.deltaTime;
    }

    Vector3 CalculateAcceleration(CelestialBody otherBody)
    {
        Rigidbody rbOther = otherBody.rb;

        Vector3 direction = (rbOther.position - rb.position).normalized;
        float distanceSquared = (rbOther.position - rb.position).sqrMagnitude;
        Vector3 acceleration = direction * gravitationalConstant * otherBody.mass / distanceSquared;

        return acceleration;
    }

    //returns orbital period of this body around other body in seconds (assumes circular orbit)
    float CalculateOrbitalPeriod(CelestialBody otherBody)
    {
        Rigidbody rbOther = otherBody.rb;
        //calculate distance between the two bodies
        float distance = (rbOther.position - rb.position).magnitude;

        //4 pi squared times distance between both planets cubed 
        float numerator = 4 * Mathf.Pow(Mathf.PI, 2) * Mathf.Pow(distance, 3);
        //gravitational constant times other bodies mass
        float denominator = gravitationalConstant * otherBody.mass;
        //divide
        float periodSquared = numerator / denominator;
        //square root and return
        return Mathf.Sqrt(periodSquared);
    }
}
