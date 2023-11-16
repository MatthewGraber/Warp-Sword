using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class BasicEnemy : MonoBehaviour
{

    public enum EnemyState
    {
        Idle,
        Patrolling,
        Active
    }

    public EnemyState State = EnemyState.Idle;

    [SerializeField] private FloatingHealthBar healthbar;

    public int health;
    public int maxHealth = 10;

    private Rigidbody _rb;
    private CapsuleCollider collider;
    public LayerMask groundLayer;


    private float invincibility = 0f;

    [SerializeField] protected float minDistance = 4;
    [SerializeField] protected float maxDistance = 6;

    [SerializeField] protected int sightDistance = 15;

    public NavMeshAgent agent;

    // Time the enemy will be stunned after being hit by an attack
    protected float stunTime = 0.8f;

    // Limits the amount of time we spend going to one patrol destination
    // Prevents the enemy from getting stuck in one spot
    protected float PATROL_TIME = 10f;
    protected float patrolCountdown = 0f;


    // EnemyManager manager;

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

        if (State == EnemyState.Patrolling)
        {
            patrolCountdown += Time.deltaTime;
            if (patrolCountdown > PATROL_TIME)
            {

            }
        }

        if (State != EnemyState.Active)
        {
            if (DistanceFromPlayer() < sightDistance)
            {
                State = EnemyState.Active;
            }
        }
        
    }


    protected void FixedUpdate()
    {

        if (invincibility <= 0 && isGrounded())
        {

            // Decide whether or not to change the path
            if (invincibility > 0) { }
            else
            {
                SetNextDestination();
            }
            

            if (agent.nextPosition != null)
            {

                // Add a force moving the enemy in the direction of their next position
                if (agent.nextPosition != transform.position)
                    _rb.velocity = (agent.nextPosition - transform.position + Vector3.down).normalized * agent.speed;
                else
                    _rb.velocity -= Vector3.down * 9.81f * Time.deltaTime;
            }
            
        }
        
    }


    void SetNextDestination()
    {
        switch(State)
        {
            case EnemyState.Patrolling:
                if (agent.destination == null)
                {
                    ResetPatrol(); 
                }
                break;


            // Active means we're pursuing the player
            case EnemyState.Active:
                Vector3 playerPos = PlayerBehavior.Instance.transform.position;

                // If the player is far away, move towards them
                if ((transform.position - playerPos).magnitude > maxDistance)
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
                break;


            case EnemyState.Idle:
                break;
        }
        
    }


    public bool TakeDamage(int damage)
    {
        if (State != EnemyState.Active)
        {
            State = EnemyState.Active;
        }

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


    // Resets the patrol time
    public void ResetPatrol()
    {
        patrolCountdown = 0f;
        State = EnemyState.Idle;
    }


    private void OnCollisionEnter(Collision collision)
    {
        // If two enemies collide, give them new destinations so they don't keep staring at each other
        if (collision.gameObject.tag == "Enemy")
        {
            if (State == EnemyState.Patrolling)
            {
                ResetPatrol();
            }
        }
    }

}
