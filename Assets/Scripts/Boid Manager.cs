using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using UnityEngine;

public class BoidManager : MonoBehaviour
{
    //Internal Lists
    private List<Boid> boidList1;
    private List<Boid> boidList2;
    private List<EntityMovement> entityList1;
    private List<EntityMovement> entityList2;
    private bool list1 = true; //bool to track which list is being used

    [Header("Prefabs")]
    [SerializeField] private GameObject boidPrefab;

    [Header("Boid Manager Variables")]
    [SerializeField] private int numBoids;
    private float widthBoundry = 40;
    private float heightBoundry = 18;

    // Start is called before the first frame update
    void Start()
    {
        //initializing lists
        boidList1 = new List<Boid>();
        boidList2 = new List<Boid>();
        entityList1 = new List<EntityMovement>();
        entityList2 = new List<EntityMovement>();

        //Start Spawning boids in the background
        StartCoroutine(SpawnBoids(numBoids));

        //toggle which list is being used
        list1 = !list1;
    }

    // Update is called once per frame
    void Update()
    {
        if (list1)
        {
            for (int i=0; i<boidList1.Count; i++)
            {
                entityList1[i].ApplyForces();
                boidList1[i].BoidsBehaviour();
            }
        }
        else
        {
            for (int i = 0; i < boidList2.Count; i++)
            {
                entityList2[i].ApplyForces();
                boidList2[i].BoidsBehaviour();
            }
        }

        //toggle which list is being used
        list1 = !list1;
    }

    //Function to change number of boids to a specified amount
    public void ChangeNumberOfBoids(int newNumBoids)
    {
        if (numBoids < newNumBoids)
        {
            StartCoroutine(SpawnBoids(newNumBoids - numBoids));
        }
        else if (numBoids > newNumBoids)
        {
            StartCoroutine(RemoveBoids(numBoids - newNumBoids));
        }
        else
        {
            return;
        }

        numBoids = newNumBoids;
    }

    //Function to spawn boids as a background operation
    private IEnumerator SpawnBoids(int numToSpawn)
    {
        for (int i = 0; i < numToSpawn; i++)
        {
            //initialize varaibles to manage spawning
            bool spawned = false;
            Vector2 position;

            //loop through operation until spawned
            while (!spawned)
            {
                //generate random spawning location
                position = new Vector2(Random.Range(-widthBoundry, widthBoundry), Random.Range(-heightBoundry, heightBoundry));

                if (!Physics2D.OverlapPoint(position))
                {
                    //generating starting velocity
                    Vector2 initialVelocity = new Vector2(Random.Range(0f, 1f), Random.Range(0f, 1f));

                    //spawning object
                    GameObject boid = Instantiate(boidPrefab, new Vector3(position.x, position.y, 0f), Quaternion.identity);

                    //Determine which list to add the boid to
                    if (list1)
                    {
                        boidList1.Add(boid.GetComponent<Boid>());
                        entityList1.Add(boid.GetComponent<EntityMovement>());
                    }
                    else
                    {
                        boidList2.Add(boid.GetComponent<Boid>());
                        entityList2.Add(boid.GetComponent<EntityMovement>());
                    }
                    
                    spawned = true;
                }
            }

            yield return null;
        } 
    }

    //Function to remove boids as a background process
    private IEnumerator RemoveBoids(int numToRemove)
    {
        for (int i=0; i<numToRemove; i++)
        {
            if (list1)
            {
                GameObject boid = boidList1[0].gameObject;
                boidList1.Remove(boidList1[0]);
                entityList1.Remove(entityList1[0]);
                Destroy(boid);
            }
            else
            {
                GameObject boid = boidList2[0].gameObject;
                boidList1.Remove(boidList2[0]);
                entityList1.Remove(entityList2[0]);
                Destroy(boid);
            }

            yield return null;
        }
    }

    //Function to start co-routine for obstacle detection range removal
    public void ChangeObstacleDetectionRange(float newRange)
    {
        StartCoroutine(ModifyObstacleDetectionRange(newRange));
    }

    //Function that changes the obstacles detection range as a background process
    private IEnumerator ModifyObstacleDetectionRange(float newRange)
    {
        int total = entityList1.Count + entityList2.Count;

        for (int i=0; i<total; i++)
        {
            if (list1)
            {
                entityList1[i / 2].ModifyObstacleDetectionRadius(newRange);
            }
            else
            {
                entityList1[i / 2].ModifyObstacleDetectionRadius(newRange);
            }

            yield return null;
        }
    }

    //Function to start co-routine for obstacle repulsion force removal
    public void ChangeObstacleRepulsionForce(float newForce)
    {
        StartCoroutine(ModifyObstacleRepulsionForce(newForce));
    }

    //Function that changes the obstacles repulsion strength as a background process
    private IEnumerator ModifyObstacleRepulsionForce(float newForce)
    {
        int total = entityList1.Count + entityList2.Count;

        for (int i = 0; i < total; i++)
        {
            if (list1)
            {
                entityList1[i / 2].ModifyObstacleDetectionRadius(newForce);
            }
            else
            {
                entityList1[i / 2].ModifyObstacleDetectionRadius(newForce);
            }

            yield return null;
        }
    }

    //Function to start co-routine for obstacle repulsion force removal
    public void ChangeForwardForce(float newForce)
    {
        StartCoroutine(ModifyForwardForce(newForce));
    }

    //Function that changes the obstacles repulsion strength as a background process
    private IEnumerator ModifyForwardForce(float newForce)
    {
        int total = entityList1.Count + entityList2.Count;

        for (int i = 0; i < total; i++)
        {
            if (list1)
            {
                entityList1[i / 2].ModifyForwardForce(newForce);
            }
            else
            {
                entityList1[i / 2].ModifyForwardForce(newForce);
            }

            yield return null;
        }
    }

    //Function that starts the coroutine to chance noise chance
    public void ChangeNoiseChance(float noiseChance)
    {
        StartCoroutine(ModifyNoiseChance(noiseChance));
    }

    //Function that modifies the chance of noise on all boids as a background process
    private IEnumerator ModifyNoiseChance(float newChance)
    {
        int total = entityList1.Count + entityList2.Count;

        for (int i = 0; i < total; i++)
        {
            if (list1)
            {
                entityList1[i / 2].ModifyNoiseChance(newChance);
            }
            else
            {
                entityList1[i / 2].ModifyNoiseChance(newChance);
            }

            yield return null;
        }
    }

    public void ChangeMaxSpeed(float newSpeed)
    {
        StartCoroutine(ModifyMaxSpeed(newSpeed));
    }

    //Function that modifies the chance of noise on all boids as a background process
    private IEnumerator ModifyMaxSpeed(float newSpeed)
    {
        int total = entityList1.Count + entityList2.Count;

        for (int i = 0; i < total; i++)
        {
            if (list1)
            {
                entityList1[i / 2].ModifyNoiseChance(newSpeed);
            }
            else
            {
                entityList1[i / 2].ModifyNoiseChance(newSpeed);
            }

            yield return null;
        }
    }


}
