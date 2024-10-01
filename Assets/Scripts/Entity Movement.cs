using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityMovement : MonoBehaviour
{
    [Header("Entity References")]
    [SerializeField] private Rigidbody2D rb; //rigidbody of the enity

    [Header("Obstacle Handling")]
    [SerializeField] private float obstacleDetectionRadius; //the distance that is checked for the presence of obstacles
    [SerializeField] private float obstacleRepulsionStrength; //strength of obstacle repulsion
    [SerializeField] private int maxObstaclesDetected; //max number of obstacles that can be detected
    [SerializeField] private LayerMask obstaclesLayer; //the layer on which obstacles are found
    private Collider2D[] hitsBufferObstacles; //array which contains obstacles that are detected

    [Header("Entity Movement Variables")]
    [SerializeField] private float rotationSpeed; //speed with which the entity will rotate
    [SerializeField] private float forwardForce; //propelling force to make individual Entity move forward
    [SerializeField] private float noiseRange; //direction range of the noise
    [SerializeField] private float noiseChance; //chance that noise will be added to movement
    [SerializeField] private float maxSpeed;

    [Header("Starting Varaibles")]
    [SerializeField] private Vector2 initialForce; //initial force that is added on the first frame

    // Start is called before the first frame update
    void Awake()
    {
        //setting initial force
        initialForce = new Vector2(Random.Range(0f, 1f), Random.Range(0f, 1f));
        rb.AddForce(initialForce, ForceMode2D.Impulse);
        hitsBufferObstacles = new Collider2D[maxObstaclesDetected];  
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        ApplyForces();
        RotateToFaceMovement();
    }

    public void ApplyForces()
    {
        AvoidObstales();
        MoveForward();
        if (Random.Range(0f, 100f) <= noiseChance)
        {
            AddNoise();
        }
        CapSpeed();
    }

    private void AvoidObstales()
    {
        int numHits = Physics2D.OverlapCircleNonAlloc(transform.position, obstacleDetectionRadius, hitsBufferObstacles, obstaclesLayer);

        for (int i =0; i < numHits; i++)
        {
            Vector2 direction = transform.position - hitsBufferObstacles[i].transform.position;
            Vector2 force = direction.normalized * (1 / direction.magnitude) * obstacleRepulsionStrength;
            if (force.magnitude >= 0.01)
            {
                rb.AddForce(force);
            }   
        }
    }

    private void RotateToFaceMovement()
    {
        if (rb.velocity.magnitude > 0)
        {
            Quaternion toRotation = Quaternion.LookRotation(Vector3.forward, rb.velocity.normalized);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }
    }

    private void MoveForward()
    {
        Vector2 force = rb.velocity.normalized * forwardForce;
        rb.AddForce(force);
    }

    private void AddNoise()
    {
        float randomRotationAngle = Random.Range(-noiseRange/2, noiseRange/2);
        Vector2 direction = Quaternion.AngleAxis(randomRotationAngle, Vector3.forward) * rb.velocity.normalized;
        rb.velocity = direction.normalized * rb.velocity.magnitude;
    }

    private void CapSpeed()
    {
        if (rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }
    }
}