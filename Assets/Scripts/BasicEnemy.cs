using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class BasicEnemy : MonoBehaviour
{

    [SerializeField] private FloatingHealthBar healthbar;

    public int health;
    public int maxHealth = 10;

    private Rigidbody _rb;

    private float invincibility = 0f;

    private float idealDistance = 5;

    private NavMeshAgent agent;

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        healthbar = GetComponent<FloatingHealthBar>();
        _rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (invincibility > 0)
        {
            invincibility -= Time.deltaTime;
        }
    }


    void FixedUpdate()
    {
        Vector3 playerPos = PlayerBehavior.Instance.transform.position;

        if (invincibility > 0) { }

        // If the player is far away, move towards them
        else if ((transform.position - playerPos).magnitude > idealDistance + 1)
        {
            agent.SetDestination(playerPos);
        }
        else if ((transform.position - playerPos).magnitude < idealDistance - 1)
        {
            // If we're too close, set the ideal position to somewhere 1m away from where we currently are, moving away from the player
            agent.SetDestination(transform.position + (transform.position - playerPos).normalized);
        }
        else
        {
            agent.SetDestination(transform.position);
        }
    }


    public bool TakeDamage(int damage)
    {
        // If in invincibility, return immediately
        if (invincibility > 0) { return true; }

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
        // If in invincibility, return immediately
        if (invincibility > 0) { return; }

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
