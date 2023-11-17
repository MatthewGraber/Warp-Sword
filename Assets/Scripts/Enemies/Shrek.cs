using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using static BasicEnemy;

public class Shrek : BasicEnemy
{
    public static Shrek Instance;

    [SerializeField] GameObject BossCanvas;
    [SerializeField] Slider healthBar;


    private float speed = 3f;
    private Vector3 direction;
    [SerializeField] GameObject interference;
    InterferenceCollider interferenceCollider;

    enum BossState
    {
        Waiting,
        Dramatic,
        Active
    }

    [SerializeField] BossState state = BossState.Waiting;

    // Start is called before the first frame update
    new void Start()
    {
        if (Instance == null)
        {
            Instance = this;

            health = maxHealth;
            collider = GetComponent<CapsuleCollider>();
            _rb = GetComponent<Rigidbody>();
            interferenceCollider = interference.GetComponent<InterferenceCollider>();
            Debug.Log("Get Shreked");
        }

        else
        {
            Destroy(this);
        }

    }

    // Update is called once per frame
    new void Update()
    {
        if (invincibility > 0)
        {
            invincibility -= Time.deltaTime;
        }

        if (state == BossState.Active)
        {

        }
    }


    new void FixedUpdate()
    {
        if (state == BossState.Active) 
        {
            direction = DirectionToPlayer();

            if (interferenceCollider.avoiding)
            {
                if (direction.y < 0)
                {
                    direction.y = 1;
                }
                else
                {
                    direction.y += 1;
                }
            }

            _rb.velocity = direction.normalized*speed;
        }
        else if (state == BossState.Dramatic)
        {
            _rb.velocity = Vector3.up*speed;
        }
    }

    override public bool TakeDamage(int damage)
    {

        // If in invincibility, return immediately
        if (invincibility > 0) { return true; }

        health -= damage;
        healthBar.value = 1.0f*health/maxHealth;

        if (health <= 0)
        {
            Die();
            return false;
        }
        invincibility = 0.3f;
        return true;
    }

    override protected void Die()
    {
        SwordBehavior sword = GetComponentInChildren<SwordBehavior>();
        if (sword != null)
        {
            sword.transform.parent = null;
        }
        Destroy(gameObject);
    }


    public void Activate()
    {
        state = BossState.Dramatic;
        BossCanvas.SetActive(true);
        StartCoroutine(DramaticOpening());
    }


    IEnumerator DramaticOpening()
    {
        yield return new WaitForSeconds(3f);
        state = BossState.Active;
    }

}
