using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SwordBehavior : MonoBehaviour
{
    public float throwSpeed = 5.0f;
    public float timeout = 3.0f;
    public float timeoutCount = 0;

    public Camera view;

    public GameObject sword;
    public GameObject player;
    public GameObject weaponHolder;
    private PlayerBehavior playerBehavior;

    public Vector3 cameraOffset;

    private Animator animator;

    public int throwDamage = 2;
    public int hitDamage = 2;
    public int executionDamage = 8;


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


    // Start is called before the first frame update
    void Start()
    {
        playerBehavior = player.GetComponent<PlayerBehavior>();
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
        else
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
                        if (!enemy.TakeDamage(executionDamage))
                        {
                            Debug.Log("EXECUTED!");
                        }
                    }
                }
                playerBehavior.Teleport(transform.position);
                Recall();
            }

            // No commands have been given
            else if (state == State.InEnemy)
            {
                transform.SetLocalPositionAndRotation(objectOffset, objectRotation);
            }
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
                    if (enemy.TakeDamage(throwDamage))
                    {
                        transform.parent = other.transform;
                        objectOffset = other.transform.localPosition;
                        objectRotation = other.transform.rotation;
                    }
                    else
                    {
                        Recall();
                    }
                }
            }

            else if (other.name != "Player")
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

                if (enemy != null)
                {
                    enemy.TakeDamage(hitDamage);
                }
            }
        }
        
    }

    IEnumerator SwingCooldown()
    {
        yield return new WaitForSeconds(1.0f);
        state = State.Held;
    }

}
