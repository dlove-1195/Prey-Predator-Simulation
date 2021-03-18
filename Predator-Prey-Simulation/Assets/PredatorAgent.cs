using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PredatorAgent : MonoBehaviour
{
    public List<GameObject> prey_list, predator_list, obstacle_list;
    private enum State { Wander, Chase };
    private State state;

    private float speed, changeDirectionTime, timer, acceleration, chaseDist, cosAngle;
    private Vector3 moveSpot;
    private float minX = -7.8f;
    private float maxX = 7.8f;
    private float minZ = -7.8f;
    private float maxZ = 7.8f;
    private float yHeight = 0.5f;
    private GameObject chasing_prey;

    void Start()
    {
        speed = 2f;
        changeDirectionTime = 3f;
        timer = 0;
        acceleration = 1.5f;
        chaseDist = 3.0f;
        cosAngle = 0.7f;
        RandomGenerate();
        moveSpot = new Vector3(Random.Range(minX, maxX), yHeight, Random.Range(minZ, maxZ)); //moving towards
    }

    void Update()
    {
        CheckPreys();
  
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
            case State.Chase:
                Debug.Log("Chase");
                speed = 6f;
                moveSpot.x = -5 * (transform.position.x - chasing_prey.transform.position.x);
                moveSpot.z = -5 * (transform.position.z - chasing_prey.transform.position.z);
                break;
            default:
                break;
        }
        CheckSelfs();
        CheckWalls();
        CheckObstacles();
        transform.position = Vector3.MoveTowards(transform.position, moveSpot, speed * Time.deltaTime);
    }
    private void RandomGenerate()
    {
        float xPos = Random.Range(minX, maxX);
        float zPos = Random.Range(0, maxZ);
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
            moveSpot.x = 2f;
        }
        if (transform.position.x > maxX)
        {
            moveSpot.x = -2f;
        }
        if (transform.position.z < minZ)
        {
            moveSpot.z = 2f;
        }
        if (transform.position.z > maxZ)
        {
            moveSpot.x = -2f;
        }

    }

    private void CheckPreys()
    {

        foreach (GameObject prey in prey_list)
        {
            if (Vector3.Distance(prey.transform.position, transform.position) < chaseDist)
            {
                Vector3 agentToVertex = prey.transform.position - transform.position;
                if (Vector3.Dot(agentToVertex.normalized, moveSpot.normalized) > cosAngle)
                {
                    state = State.Chase;
                    chasing_prey = prey;
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
        foreach (GameObject predator in predator_list)
        {

            if (!Equals(transform.name, predator.name) && Vector3.Distance(predator.transform.position, transform.position) < 1.1f)
            {
                moveSpot.x = -5 * (predator.transform.position.x - transform.position.x);
                moveSpot.z = -5 * (predator.transform.position.z - transform.position.z);
                if (speed < 3f)
                {
                    speed = speed * acceleration;
                }
            }

        }
    }
}
