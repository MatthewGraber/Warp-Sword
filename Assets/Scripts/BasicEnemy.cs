using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BasicEnemy : MonoBehaviour
{

    [SerializeField] private FloatingHealthBar healthbar;

    public int health;
    public int maxHealth = 10;

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        healthbar = GetComponent<FloatingHealthBar>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool TakeDamage(int damage)
    {
        health -= damage;
        healthbar.SetValue(health,maxHealth);
        if (health <= 0)
        {
            Die();
            return false;
        }
        return true;
    }


    private void Die()
    {
        SwordBehavior sword = GetComponentInChildren<SwordBehavior>();
        if (sword != null)
        {
            sword.transform.parent = null;
        }
        Destroy(gameObject);
    }
}
