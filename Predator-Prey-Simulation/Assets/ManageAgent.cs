using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageAgent : MonoBehaviour
{
    private List<GameObject> prey_list, predator_list, obstacle_list;
    public int num_prey = 5; 
    public int num_predator = 2;
    // Start is called before the first frame update
    void Start()
    {
        prey_list = new List<GameObject>();
        predator_list = new List<GameObject>();
        obstacle_list = new List<GameObject>();
        AddObstacles();
        CreatePreys();
        CreatePredators();

    }
    
    private void CreatePreys()
    {
        for (int i = 0; i < num_prey; i++)
        {
            GameObject temp = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            temp.GetComponent<Renderer>().material.color = new Color(0, 1, 0, 1); // color green

            //temp.transform.parent = transform;
            temp.AddComponent<PreyAgent>();
            temp.GetComponent<PreyAgent>().predator_list = predator_list;
            temp.GetComponent<PreyAgent>().prey_list = prey_list;
            temp.GetComponent<PreyAgent>().obstacle_list = obstacle_list;
            temp.name = "Prey" + (i + 1);
            prey_list.Add(temp);
        }
    } 
    
    private void CreatePredators()
    {
        for (int i = 0; i < num_predator; i++)
        {
            GameObject temp = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            temp.GetComponent<Renderer>().material.color = new Color(1, 0, 0, 1); // color red

            //temp.transform.parent = transform;
            temp.AddComponent<PredatorAgent>();
            temp.GetComponent<PredatorAgent>().predator_list = predator_list;
            temp.GetComponent<PredatorAgent>().prey_list = prey_list;
            temp.GetComponent<PredatorAgent>().obstacle_list = obstacle_list;
            temp.name = "Predator" + (i + 1);
            predator_list.Add(temp);
        }
    } 

    private void AddObstacles()
    {
        for (int i = 1; i <= 3; i++)
        {
            GameObject temp = GameObject.Find("Obstacle" + i);
            obstacle_list.Add(temp);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
