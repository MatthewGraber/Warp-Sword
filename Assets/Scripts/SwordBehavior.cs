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
    private PlayerBehavior playerBehavior;

    public Vector3 cameraOffset;


    public enum State
    {
        Held,
        Throwing,
        InObject
    }

    public State state = State.Held;

    private Vector3 tragectory;


    // Start is called before the first frame update
    void Start()
    {
        playerBehavior = player.GetComponent<PlayerBehavior>();
    }

    // Update is called once per frame
    void Update()
    {
        if (state == State.Held)
        {
            // view.transform.
            // transform.SetPositionAndRotation(view.transform.position + cameraOffset, view.transform.rotation);
            if (Input.GetKeyDown(KeyCode.E))
            {
                transform.parent = null;
                state = State.Throwing;
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
        transform.parent = view.transform;
        state = State.Held;
        transform.SetPositionAndRotation(view.transform.position + view.transform.rotation * cameraOffset, view.transform.rotation * Quaternion.Euler(0f, 90f, 0));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (state == State.Throwing)
        {
            if (other.name != "Player")
            {
                Debug.Log("AAHHHH");
                state = State.InObject;
                Vector3 nextPos = transform.position + transform.rotation * Vector3.up * 0.3f;
                transform.SetPositionAndRotation(nextPos, transform.rotation);
            }
            
        }
        
    }

}
