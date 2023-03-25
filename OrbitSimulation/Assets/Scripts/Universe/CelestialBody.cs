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

    public float gravitationalConstant;

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
        }
        velocity += acceleration * timeScale;
    }

    public void UpdatePosition(float timeScale)
    {
        rb.position += velocity * timeScale;
    }

    Vector3 CalculateAcceleration(CelestialBody otherBody)
    {
        Rigidbody rbOther = otherBody.rb;

        Vector3 direction = (rbOther.position - rb.position).normalized;
        float distanceSquared = (rbOther.position - rb.position).sqrMagnitude;
        Vector3 acceleration = direction * gravitationalConstant * otherBody.mass / distanceSquared;

        return acceleration;
    }
}
