using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreyAgent : MonoBehaviour
{
    public List<GameObject> prey_list, predator_list, obstacle_list;
    private enum State { Wander, Flee };
    private State state;

    private float speed, changeDirectionTime, timer, acceleration, fleeDist;
    private Vector3 moveSpot;
    private float minX = -7.8f;
    private float maxX = 7.8f;
    private float minZ = -7.8f;
    private float maxZ = 7.8f;
    private float yHeight = 0.5f;
    private GameObject chasing_predator;

    void Start()
    {
        speed = 2f;
        changeDirectionTime = 4f;
        timer = 0;
        acceleration = 1.5f; 
        fleeDist = 2.5f;
        RandomGenerate(); 
        moveSpot = new Vector3(Random.Range(minX, maxX), yHeight, Random.Range(minZ, maxZ)); //moving towards
    }

    void Update()
    {
        
        CheckPredators();
        switch (state)
        {
            case State.Wander:
                timer += Time.deltaTime;
                if (timer > changeDirectionTime)
                {
                    moveSpot = new Vector3(Random.Range(minX - 5, maxX + 5), yHeight, Random.Range(minZ - 5, maxZ + 5)); //moving towards
                    speed = 2f;
                    timer = 0f;
                }
                break;
            case State.Flee:
                speed = 5f;
                moveSpot.x = 4 * (transform.position.x - chasing_predator.transform.position.x);
                moveSpot.z = 4 * (transform.position.z - chasing_predator.transform.position.z);
                Debug.Log("Flee");
                break;
            default:
                break;
        }
        CheckSelfs();        
        CheckObstacles();
        CheckWalls();
        transform.position = Vector3.MoveTowards(transform.position, moveSpot, speed * Time.deltaTime);
    }
    private void RandomGenerate()
    {
        float xPos = Random.Range(minX, maxX);
        float zPos = Random.Range(minZ, 0);
        foreach (GameObject obstacle in obstacle_list)
        {
            if (Vector3.Distance(obstacle.transform.position, transform.position) < 1.1f)
            {
                xPos = Random.Range(minX, maxX);
                zPos = Random.Range(minZ, 0);
            }
        }
        gameObject.transform.position = new Vector3(xPos, yHeight, zPos);
    }

    private void CheckObstacles()
    {
        foreach (GameObject obstacle in obstacle_list)
        {

            if (Vector3.Distance(obstacle.transform.position, transform.position) < 1.1f)
            {
                moveSpot.x = -5 * (obstacle.transform.position.x - transform.position.x);
                moveSpot.z = -5 * (obstacle.transform.position.z - transform.position.z);
                if (speed < 3f)
                {
                    speed = speed * acceleration;
                }
            }
            
        }
    }

    private void CheckWalls()
    {

        if (transform.position.x < minX)
        {
            moveSpot.x = 3f;
        }
        if (transform.position.x > maxX)
        {
            moveSpot.x = -3f;
        }
        if (transform.position.z < minZ)
        {
            moveSpot.z = 3f;
        }
        if (transform.position.z > maxZ)
        {
            moveSpot.x = -3f;
        }

    }
    
    private void CheckPredators()
    {

        foreach (GameObject predator in predator_list)
        {
            if (Vector3.Distance(predator.transform.position, transform.position) < fleeDist)
            {
                state = State.Flee;
                chasing_predator = predator;
                if (Vector3.Distance(predator.transform.position, transform.position) < 1.1f)
                {
                    RandomGenerate();
                }
            }
            else
            {
               state = State.Wander;
            }
        }

    }
    
    private void CheckSelfs()
    {
        foreach (GameObject prey in prey_list)
        {

            if (!Equals(transform.name, prey.name) && Vector3.Distance(prey.transform.position, transform.position) < 1.1f)
            {
                moveSpot.x = -5 * (prey.transform.position.x - transform.position.x);
                moveSpot.z = -5 * (prey.transform.position.z - transform.position.z);
                if (speed < 3f)
                {
                    speed = speed * acceleration;
                }
            }

        }
    } 
}
