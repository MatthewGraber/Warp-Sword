using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ShipBehavior : MonoBehaviour
{
    public bool active = false;

    public Vector3 playerOffset;

    private Vector3 acceleration;
    private Vector3 speed;

    public float baseSpeed;
    private float maxSpeed = 20;

    public float rotSpeed;

    private OnBoatTrigger boatTrigger;

    // The physics object that is actually used to move the boat around
    // [SerializeField] private GameObject mover;

    private Rigidbody _rb;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        boatTrigger = GetComponentInChildren<OnBoatTrigger>();
    }

    // Update is called once per frame
    void Update()
    {
        // Get movement inputs
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");



        if (active)
        {
            transform.Rotate(0, x*rotSpeed*Time.deltaTime, 0);
            // _rb.velocity = _rb.velocity.
            speed = (transform.forward * z) * (Time.deltaTime * baseSpeed);
            // Debug.Log(speed);
            

            
            // mover.transform.position = mover.transform.position + (move * Time.deltaTime * baseSpeed);

            // PlayerBehavior.Instance.transform.position = transform.position + playerOffset;
            // PlayerBehavior.Instance.transform.rotation = transform.rotation;
        }
        else
        {
            speed = Vector3.zero;
        }

        // Apply forces to the ship
        _rb.velocity = (_rb.velocity + speed);
        if (_rb.velocity.magnitude > maxSpeed)
        {
            _rb.velocity = _rb.velocity.normalized * maxSpeed;
        }
        
        

        // mover.transform.position = mover.transform.position + (move * Time.deltaTime);
        // transform.SetPositionAndRotation(mover.transform.position, mover.transform.rotation);
    }

    private void FixedUpdate()
    {
        if (active)
        {
            PlayerBehavior.Instance.transform.SetPositionAndRotation(transform.position + transform.rotation * playerOffset, PlayerBehavior.Instance.transform.rotation);
        }
        else if (boatTrigger.PlayerOnBoat)
        {
            PlayerBehavior.Instance.controller.Move(_rb.velocity * Time.deltaTime);
        }
    }
}
