using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{

    // List of enemies we're managing
    private List<BasicEnemy> enemyList;

    // If the enemies are alert, they attack the player. Otherwise, they patrol
    bool alert = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Alert(bool a)
    {
        foreach (BasicEnemy enemy in enemyList)
        {
            if (enemy != null)
            {
                enemy.alert = a;
            }
        }
    }
}
