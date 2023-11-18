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

    // Scythe
    [SerializeField] GameObject Scythe;
    private AxeBehavior scytheBehavior;


    // Gun
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] GameObject gun;
    private Animator GunAnimator;

    float COOLDOWN_TIME = 2f;
    float cooldown = 0;


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
            scytheBehavior = Scythe.GetComponent<AxeBehavior>();

            GunAnimator = gun.GetComponent<Animator>();

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
            // EN GARDE
            FaceTarget();

            // Swish swoosh
            if (DistanceFromPlayer() <= 4)
            {
                scytheBehavior.Attack();
            }

            // BANG BANG
            if (cooldown < COOLDOWN_TIME)
            {
                cooldown += Time.deltaTime;
            }
            if (DistanceFromPlayer() >= 6)
            {
                if (cooldown >= COOLDOWN_TIME)
                {
                    ShootGun();
                }
            }
        }
        // BUM BUM BUM BUM BUM BUM BUM BUM
        else if (state == BossState.Dramatic)
        {
            FaceTarget();
        }
    }


    new void FixedUpdate()
    {

        // transform.SetPositionAndRotation(transform.position, new Quaternion(0, transform.rotation.y, 0, 0));
        if (state == BossState.Active) 
        {
            if (DistanceFromPlayer() > maxDistance)
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
                _rb.velocity = direction.normalized * speed;
            }
            else
            {
                _rb.velocity = Vector3.zero;
            }

        }
        else if (state == BossState.Dramatic)
        {
            _rb.velocity = Vector3.up*speed;
        }
    }

    override public bool TakeDamage(float damage)
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


    private void ShootGun()
    {
        // Set the direction of firing
        Vector3 direction = new Vector3(transform.forward.x, DirectionToPlayer().y, transform.forward.z);
        direction = direction.normalized;

        GameObject newBullet = Instantiate(bulletPrefab, transform.position + transform.rotation * (new Vector3(0, 0, -1)), transform.rotation);
        // newBullet.transform.SetPositionAndRotation(transform.position + transform.rotation * (new Vector3(0, 1, 1)), transform.rotation);

        newBullet.GetComponent<BulletBehavior>().SetDirection(direction);
        Debug.Log("Firing bullet at " + newBullet.transform.position);
        GunAnimator.SetTrigger("Attack");


        cooldown = 0;
    }

}
