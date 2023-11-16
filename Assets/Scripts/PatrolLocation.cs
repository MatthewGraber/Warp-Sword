using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolLocation : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            BasicEnemy enemy = other.GetComponent<BasicEnemy>();
            if (enemy != null)
            {
                enemy.ResetPatrol();
            }
        }
    }
}
