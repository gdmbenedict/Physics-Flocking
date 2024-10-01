using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidManager : MonoBehaviour
{
    //Lists
    private List<Boid> boidList;
    private List<EntityMovement> entityList;

    [Header("Prefabs")]
    [SerializeField] private GameObject boidPrefab;

    [Header("Boid Manager Variables")]
    [SerializeField] private int numBoids;
    private float widthBoundry = 38;
    private float heightBoundry = 17;

    // Start is called before the first frame update
    void Start()
    {
        boidList = new List<Boid>();
        entityList = new List<EntityMovement>();

        for (int i=0; i<numBoids; i++)
        {
            SpawnBoid();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SpawnBoid()
    {
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
                boidList.Add(boid.GetComponent<Boid>());
                entityList.Add(boid.GetComponent<EntityMovement>());

                spawned = true;
            }
        }
    }
}
