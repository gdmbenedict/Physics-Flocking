using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityMovement : MonoBehaviour
{
    [Header("Player References")]
    [SerializeField] private Rigidbody2D rb;
    //[SerializeField] private Boid boid;

    [Header("Object Detection")]
    [SerializeField] private float obstacleDetectionRadius;
    [SerializeField] private float obstacleRepulsionStrength;
    [SerializeField] private int maxObstaclesDetected;
    [SerializeField] private LayerMask obstaclesLayer;
    private Collider2D[] hitsBufferObstacles;

    [Header("Player Movement Variables")]
    [SerializeField] private float rotationSpeed;

    // Start is called before the first frame update
    void Awake()
    {
        hitsBufferObstacles = new Collider2D[maxObstaclesDetected];
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        AvoidObstales();
        RotateToFaceMovement();
    }

    public void ApplyForces()
    {

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
}
