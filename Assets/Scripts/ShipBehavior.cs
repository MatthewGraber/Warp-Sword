using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipBehavior : MonoBehaviour
{
    
    public bool active = false;

    public Vector3 playerOffset;

    private Vector3 acceleration;
    private Vector3 speed;

    public float baseSpeed = 0.3f;
    private float maxSpeed = 5;

    // The physics object that is actually used to move the boat around
    // [SerializeField] private GameObject mover;

    private Rigidbody _rb;

    // Start is called before the first frame update
    void Start()
    {
        // _rb = mover.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        // Get movement inputs
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = Vector3.zero;


        if (active)
        {
            
            move = (transform.forward * z) * (Time.deltaTime * baseSpeed);

            

            // _rb.
            // mover.transform.position = mover.transform.position + (move * Time.deltaTime * baseSpeed);

            // PlayerBehavior.Instance.transform.position = transform.position + playerOffset;
            // PlayerBehavior.Instance.transform.rotation = transform.rotation;
        }

        // mover.transform.position = mover.transform.position + (move * Time.deltaTime);
        // transform.SetPositionAndRotation(mover.transform.position, mover.transform.rotation);
    }
}
