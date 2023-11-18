using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SwordBehavior : MonoBehaviour
{
    public static SwordBehavior Instance;

    public float throwSpeed = 5.0f;
    public float timeout = 3.0f;
    public float timeoutCount = 0;

    public Camera view;

    public GameObject sword;
    public GameObject weaponHolder;
    private PlayerBehavior player;

    public Vector3 cameraOffset;

    private Animator animator;

    public float damageMult = 1.0f;
    public int throwDamage = 3;
    public int hitDamage = 5;
    public int executionDamage = 10;
    public int knockbackForce;


    private Vector3 objectOffset;
    private Quaternion objectRotation;

    public enum State
    {
        Held,
        Swinging,
        Throwing,
        InObject,
        InEnemy
    }

    public State state = State.Held;

    private Vector3 tragectory;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
        player = PlayerBehavior.Instance;
        animator = transform.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (state == State.Held)
        {
            // view.transform.
            // transform.SetPositionAndRotation(view.transform.position + cameraOffset, view.transform.rotation);
            if (Input.GetButtonDown("Fire1"))
            {
                animator.SetTrigger("Attack");
                state = State.Swinging;
                StartCoroutine(SwingCooldown());
            }
            else if (Input.GetButtonDown("Fire2"))
            {
                // Set parent to null so it moves independantly
                transform.parent = null;
                state = State.Throwing;

                // Disable the animator so it doesn't mess with the sword
                animator.enabled = false;     
                
                // Rotate the sword to a throwing angle
                transform.rotation = transform.rotation * Quaternion.Euler(0, 0, 90f);
                timeoutCount = 0;
            }
        }

        else if (state == State.Throwing)
        {
            if (Input.GetButtonDown("Fire2"))
            {
                Recall();
            }
        }

        // In an object or enemy
        else if (state == State.InEnemy || state == State.InObject)
        {
            if (Input.GetButtonDown("Fire2"))
            {
                Recall();
            }
            
            // Teleport to a thing
            else if (Input.GetButtonDown("Fire1"))
            {
                
                if (transform.parent != null)
                {
                    BasicEnemy enemy = transform.parent.GetComponent<BasicEnemy>();
                    if (transform.parent.tag == "Enemy")
                    {
                        if (!enemy.TakeDamage(executionDamage*damageMult))
                        {
                            Debug.Log("EXECUTED!");
                        }
                    }
                }
                player.Teleport(transform.position);
                Recall();
            }

            // No commands have been given
            else if (state == State.InEnemy)
            {
                transform.SetLocalPositionAndRotation(objectOffset, objectRotation);
            }
        }

        // Swinging the sword
        else if (state == State.Swinging)
        {

        }
    }

    private void FixedUpdate()
    {
        if (state == State.Throwing)
        {
            Vector3 nextPos = transform.position + transform.rotation * Vector3.up * Time.fixedDeltaTime * throwSpeed;
            transform.SetPositionAndRotation(nextPos, transform.rotation);
            timeoutCount += Time.fixedDeltaTime;
            if (timeoutCount >= timeout)
            {
                Recall();
            }
        }
    }

    public void Recall()
    {
        transform.parent = weaponHolder.transform;
        state = State.Held;
        transform.SetLocalPositionAndRotation(new Vector3(0, .3f, 0), Quaternion.Euler(0, 0, 0));
        animator.enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (state == State.Throwing)
        {
            if (other.tag == "Enemy")
            {
                Debug.Log("HYYYAAAAHH!!");
                state = State.InEnemy;
                Vector3 nextPos = transform.position + transform.rotation * Vector3.up * 0.3f;
                transform.SetPositionAndRotation(nextPos, transform.rotation);

                // Damage enemy
                BasicEnemy enemy = other.GetComponent<BasicEnemy>();

                if (enemy != null)
                {
                    
                    // Takedamage returns true if the enemy has health remaining
                    if (enemy.TakeDamage(throwDamage * damageMult))
                    {
                        transform.parent = other.transform;
                        objectOffset = transform.localPosition;
                        objectRotation = transform.localRotation;
                        enemy.Knockback(transform.up * knockbackForce);
                    }
                    else
                    {
                        Recall();
                    }
                }
            }

            else if (other.tag == "Object")
            {
                Debug.Log("AAHHHH");
                state = State.InObject;
                Vector3 nextPos = transform.position + transform.rotation * Vector3.up * 0.3f;
                transform.SetPositionAndRotation(nextPos, transform.rotation);
                
                // Attach the sword to the object it entered
                transform.parent = other.transform;
                // objectOffset = other.transform.localPosition;
                // objectRotation = other.transform.rotation;
            }
            
        }
        else if (state == State.Swinging)
        {
            if (other.tag == "Enemy")
            {
                Debug.Log("KAPOW!");
                BasicEnemy enemy = other.GetComponent<BasicEnemy>();
                PlayerBehavior.Instance.MP += 2*damageMult;

                if (enemy != null)
                {
                    enemy.Knockback((transform.up + Vector3.up) * knockbackForce);
                    enemy.TakeDamage(hitDamage * damageMult);
                }
            }
        }
        
    }

    IEnumerator SwingCooldown()
    {
        yield return new WaitForSeconds(0.5f);
        state = State.Held;
    }

}
