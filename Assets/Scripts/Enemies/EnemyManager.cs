using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class EnemyManager : MonoBehaviour
{

    // List of enemies we're managing
    [SerializeField] private GameObject enemyHolder;
    List<BasicEnemy> enemyList = new List<BasicEnemy>();


    // Locations the enemies will move to while on patrol
    [SerializeField] private GameObject locationHolder;
    List<Vector3> locations = new List<Vector3>();

    // Start is called before the first frame update
    void Start()
    {
        // Add each enemy from the gameobject to the list of enemies
        foreach (BasicEnemy enemy in enemyHolder.GetComponentsInChildren<BasicEnemy>())
        {
            enemyList.Add(enemy);
            Debug.Log("Added" + enemy.name);
        }

        // Add each location from the gameobject to the list of locations
        for (int i = 0; i < locationHolder.transform.childCount; i++)
        {
            locations.Add(locationHolder.transform.GetChild(i).position);
            Debug.Log("Added" + locationHolder.transform.GetChild(i).position);
        }
            

    }

    // Update is called once per frame
    void Update()
    {
        foreach (BasicEnemy enemy in enemyList)
        {
            if (enemy.State == BasicEnemy.EnemyState.Active)
            {
                Alert();
                break;
            }
            else if (enemy.State == BasicEnemy.EnemyState.Idle)
            {
                SetNextDestination(enemy);
            }
        }
    }

    void Alert()
    {
        foreach (BasicEnemy enemy in enemyList)
        {
            if (enemy != null)
            {
                enemy.State = BasicEnemy.EnemyState.Active;
            }
        }
    }


    void SetNextDestination(BasicEnemy enemy)
    {
        int next = Random.Range(0, locations.Count);
        if (enemy.agent == null)
        {
            return;
        }
        if (locations[next] != enemy.agent.destination)
        {
            // Debug.Log(enemy.agent.SetDestination(locations[next]));
            // Debug.Log("Set destination to: " + locations[next]);
            enemy.State = BasicEnemy.EnemyState.Patrolling;
        }
        else
        {
            SetNextDestination(enemy);
        }
    }
}
