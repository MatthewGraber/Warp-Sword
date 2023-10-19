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
    private float maxSpeed = 50;

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
        
        
        

        // mover.transform.position = mover.transform.position + (move * Time.deltaTime);
        // transform.SetPositionAndRotation(mover.transform.position, mover.transform.rotation);
    }

    private void FixedUpdate()
    {
        // Get movement inputs
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        if (active)
        {
            transform.Rotate(0, x * rotSpeed * Time.deltaTime, 0);
            speed = (transform.forward * z) * (Time.deltaTime * baseSpeed);
            // Debug.Log(speed);

            PlayerBehavior.Instance.transform.SetPositionAndRotation(transform.position + transform.rotation * playerOffset, PlayerBehavior.Instance.transform.rotation);
        }
        else if (boatTrigger.PlayerOnBoat)
        {
            speed = Vector3.zero;
            PlayerBehavior.Instance.controller.Move(_rb.velocity * Time.deltaTime);
        }

        else
        {
            speed = Vector3.zero;
        }

        // Apply forces to the ship
        _rb.velocity = (_rb.velocity + speed);
        _rb.velocity= _rb.velocity.magnitude*transform.forward;


        transform.rotation.Set(0, transform.rotation.y, 0, 0);

        // Normalize the speed of the boat
        if (_rb.velocity.magnitude > maxSpeed)
        {
            _rb.velocity = _rb.velocity.normalized * maxSpeed;
        }
    }
}
