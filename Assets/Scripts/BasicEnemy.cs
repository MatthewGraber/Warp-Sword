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

    private Rigidbody _rb;

    private float invincibility = 0f;

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        healthbar = GetComponent<FloatingHealthBar>();
        _rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (invincibility > 0)
        {
            invincibility -= Time.deltaTime;
        }
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
        invincibility = 0.3f;
        return true;
    }


    public void Knockback(Vector3 force)
    {
        _rb.AddForce(force, ForceMode.Impulse);
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
