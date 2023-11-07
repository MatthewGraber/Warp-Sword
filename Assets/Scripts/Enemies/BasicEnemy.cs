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

    public bool alert;

    private Rigidbody _rb;
    private CapsuleCollider collider;
    public LayerMask groundLayer;


    private float invincibility = 0f;

    [SerializeField] protected float minDistance = 4;
    [SerializeField] protected float maxDistance = 6;

    protected NavMeshAgent agent;

    // Time the enemy will be stunned after being hit by an attack
    protected float stunTime = 0.8f;

    // Start is called before the first frame update
    protected void Start()
    {
        health = maxHealth;
        healthbar = GetComponent<FloatingHealthBar>();
        _rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        collider = GetComponent<CapsuleCollider>();
        agent.updatePosition = false;
    }

    // Update is called once per frame
    protected void Update()
    {
        if (invincibility > 0)
        {
            invincibility -= Time.deltaTime;
        }
    }


    protected void FixedUpdate()
    {
        Vector3 playerPos = PlayerBehavior.Instance.transform.position;

        if (invincibility <= 0 && isGrounded())
        {
            // agent.updatePosition = true;

            // Decide whether or not to change the path
            if (invincibility > 0) { }

            // If the player is far away, move towards them
            else if ((transform.position - playerPos).magnitude > maxDistance)
            {
                agent.SetDestination(playerPos);
            }
            else if ((transform.position - playerPos).magnitude < minDistance)
            {
                // If we're too close, set the ideal position to somewhere 1m away from where we currently are, moving away from the player
                agent.SetDestination(transform.position + (transform.position - playerPos).normalized);
            }
            else
            {
                agent.SetDestination(transform.position);
                FaceTarget();
            }

            if (agent.nextPosition != null)
            {
                // Update the enemy's velocity
                if (agent.nextPosition != transform.position)
                    _rb.velocity = (agent.nextPosition - transform.position + Vector3.down).normalized * agent.speed;
                else
                    _rb.velocity -= Vector3.down * 9.81f * Time.deltaTime;
            }
            
        }
        // If the agent is disabled or we aren't grounded
        else
        {
            // Disabling this will prevent the agent from teleporting
            agent.ResetPath();
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

        agent.ResetPath();
        // agent.enabled = false;
        _rb.AddForce(force, ForceMode.Impulse);
        StartCoroutine(StunTimer());
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

    // Get the distance from the player
    public float DistanceFromPlayer()
    {
        Vector3 playerPos = PlayerBehavior.Instance.transform.position;
        return (transform.position - playerPos).magnitude;
    }


    // Returns a normalized vector in the direction of the player
    public Vector3 DirectionToPlayer()
    {
        return (PlayerBehavior.Instance.transform.position - transform.position).normalized;
    }


    void FaceTarget()
    {
        // var turnTowardNavSteeringTarget = PlayerBehavior.Instance.transform.position;

        Vector3 direction = (PlayerBehavior.Instance.transform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5);
    }


    private bool isGrounded()
    {
        Vector3 capsuleBottom = new Vector3(collider.bounds.center.x, collider.bounds.min.y, collider.bounds.center.z);

        bool grounded = Physics.CheckCapsule(collider.bounds.center, capsuleBottom, 0.2f, groundLayer, QueryTriggerInteraction.Ignore);
        return grounded;
    }


    IEnumerator StunTimer()
    {
        yield return new WaitForSeconds(stunTime);
        agent.enabled = true;
    }

}
