using System.Collections;
using System.Collections.Generic;
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

    //Function to spawn boids as a background operation
    private IEnumerator SpawnBoids(int numToSpawn)
    {
        for (int i = 0; i < numToSpawn; i++)
        {
            //initialize varaibles to manage spawning
            bool spawned = false;
            Vector2 position;

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
}
