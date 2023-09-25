using System.Collections;
using System.Collections.Generic;
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
            }
            else if (Input.GetKeyDown(KeyCode.E))
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
            if (Input.GetKeyDown(KeyCode.E))
            {
                Recall();
            }
        }

        else
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                Recall();
            }
            else if (Input.GetKeyDown(KeyCode.Q))
            {
                playerBehavior.Teleport(transform.position);
                Recall();
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

    private void Recall()
    {
        transform.parent = weaponHolder.transform;
        state = State.Held;
        transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.Euler(0, 0, 0));
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
                transform.parent = other.transform;

                // TODO: Damage enemy
            }

            else if (other.name != "Player")
            {
                Debug.Log("AAHHHH");
                state = State.InObject;
                Vector3 nextPos = transform.position + transform.rotation * Vector3.up * 0.3f;
                transform.SetPositionAndRotation(nextPos, transform.rotation);
            }
            
        }
        
    }

    IEnumerator SwingCooldown()
    {
        yield return new WaitForSeconds(1.0f);
        state = State.Held;
    }

}
