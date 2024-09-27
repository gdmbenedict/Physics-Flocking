using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boid : MonoBehaviour
{
    [Header("Boid References")]
    [SerializeField] private Rigidbody2D rb;

    [Header("Boid Movement Variables")]
    [SerializeField] private float maxSpeed;
    [SerializeField] private float startSpeed;
    [SerializeField] private float acceleration;
    [SerializeField] private Vector2 direction;
    [SerializeField] private float noiseMax;
    [SerializeField] private int noiseChance;

    [Header("Obstacle detection varaibles")]
    [SerializeField] private float obstacleAvoidDist;
    [SerializeField] private float obstacleAvoidStrength;
    [SerializeField] private int maxObstaclesDetected;
    [SerializeField] private LayerMask obstacleLayer;
    private Collider2D[] obstacles;

    // Update is called once per fixed frame
    void FixedUpdate()
    {
        //avoid obstacles
        AvoidObstacles();

        //add noise at random intervals
        if (Random.Range(1, noiseChance+1) == noiseChance)
        {
            GenerateMovementNoise();
        }

        //moving boid
        Move();
    }

    private void Awake()
    {
        //determining start velocity
        StartVelocity();

        //setting size of obstacles array
        obstacles = new Collider2D[maxObstaclesDetected];
    }

    private void StartVelocity()
    {
        //generating random direction
        direction.x = Random.Range(-1f, 1f);
        direction.y = Random.Range(-1f, 1f);
        direction.Normalize();

        //applying starting velocity
        rb.velocity = direction * startSpeed;
    }

    private void Move()
    {
        //normalize the direction for acceleration application
        direction.Normalize();
        rb.velocity += direction * acceleration;

        if (rb.velocity.magnitude > maxSpeed)
        {
            //setting max speed
            //Debug.Log("Setting velocity down to max.");
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }

        //make object change to face movement direction
        //transform.rotation = Quaternion.LookRotation(rb.velocity);
    }

    private void GenerateMovementNoise()
    {
        Debug.Log("Applying Noise");
        direction.x += Random.Range(-noiseMax, noiseMax);
        direction.y += Random.Range(-noiseMax, noiseMax);
    }

    private void AvoidObstacles()
    {
        //detecting nearby obstacles
        int numHits = Physics2D.OverlapCircleNonAlloc(rb.position, obstacleAvoidDist, obstacles, obstacleLayer);

        //get strength of obstacle repulsion based on distancce
        for (int i=0; i < numHits; i++)
        {
            Vector2 distance = obstacles[i].transform.position - transform.position;
            
            if (distance.x < distance.y)
            {
                direction.x -= (1 / distance.x) * obstacleAvoidStrength;
            }
            else
            {
                direction.y -= (1 / distance.y) * obstacleAvoidStrength;
            } 
        }
    }


}
