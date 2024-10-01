using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boid : MonoBehaviour
{
    [Header("Entity References")]
    [SerializeField] private Rigidbody2D rb;

    [Header("Boid Detection")]
    [SerializeField] private float boidsDetectionRadius;
    [SerializeField] private int maxBoidsDetected;
    [SerializeField] private LayerMask boidsLayer;
    private Collider2D[] hitsBufferBoids;

    [Header("Boid Behaviour Variables")]
    [SerializeField] private float boidSeperationStrength;
    [SerializeField] private float boidAlignmentStrength;
    [SerializeField] private float boidCohesionStrength;
    [SerializeField] private float forceCutOff;

    // Start is called before the first frame update
    void Awake()
    {
        hitsBufferBoids = new Collider2D[maxBoidsDetected]; 
    }

    private void FixedUpdate()
    {
        BoidsBehaviour();
    }

    public void BoidsBehaviour()
    {
        //initializing temp vectors for data storage
        Vector2 seperationVector = Vector2.zero;
        Vector2 alignmentVector = Vector2.zero;
        Vector2 cohesionVector = Vector2.zero;

        //getting raw data from boids in detection range
        int numHits = Physics2D.OverlapCircleNonAlloc(transform.position, boidsDetectionRadius, hitsBufferBoids, boidsLayer);
        for (int i=0; i<numHits; i++)
        {
            seperationVector += Seperation(hitsBufferBoids[i]);
            alignmentVector += hitsBufferBoids[i].GetComponent<Rigidbody2D>().velocity;
            cohesionVector = hitsBufferBoids[i].transform.position;
        }

        if (numHits > 0)
        {
            //getting average data
            seperationVector /= numHits;
            alignmentVector /= numHits;
            cohesionVector /= numHits;

            //getting cohesion vector from cohesion position
            Vector2 postion = new Vector2(transform.position.x, transform.position.y);
            cohesionVector = cohesionVector - postion;

            //applying strength of force
            seperationVector *= boidSeperationStrength;
            alignmentVector *= boidAlignmentStrength;
            cohesionVector *= boidCohesionStrength;

            //applying forces
            
            if (seperationVector.magnitude >= forceCutOff)
            {
                //Debug.Log(seperationVector);
                rb.AddForce(seperationVector);
            }

            if (alignmentVector.magnitude >= forceCutOff)
            {
                rb.AddForce(alignmentVector);
            }

            if (cohesionVector.magnitude >= forceCutOff)
            {
                rb.AddForce(cohesionVector);
            }
        }          
    }

    private Vector2 Seperation(Collider2D otherBoid)
    {
        if (otherBoid.gameObject != gameObject)
        {
            Vector2 vectorToBoid = transform.position - otherBoid.transform.position;
            //Debug.Log(vectorToBoid);
            Vector2 force = vectorToBoid.normalized * (1 / vectorToBoid.magnitude);
            Debug.Log(force);
            return force;
        }
        else
        {
            return Vector2.zero;
        }
    }

}
