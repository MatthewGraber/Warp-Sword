using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBehavior : MonoBehaviour
{
    [SerializeField] Slider healthBar;
    [SerializeField] Slider manaBar;

    public CharacterController controller;
    public float baseSpeed = 12f;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;
    public float sprintSpeed = 5f;

    public GameObject sword;

    public LayerMask groundLayer;

    int health;
    int maxHealth = 20;
    public int HP { get { return health; }
        set
        {
            health = value;
            if (health < 0) health = 0;

            if (health > maxHealth ) health = maxHealth;

            healthBar.value = health*1.0f/maxHealth;
        }
    }

    int mana;
    int maxMana = 20;
    public int MP { get { return mana; }
        set
        {
            mana = value;
            if (mana < 0) mana = 0;

            if (mana > maxMana) mana = maxMana;

            manaBar.value = mana*1.0f/maxMana;
        }
    }



    float speedBoost = 1f;
    Vector3 velocity;
    void Start()
    {
        health = maxHealth; 
        mana = maxMana;
    }

    void Update()
    {
        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        if (Input.GetButton("Fire3"))
            speedBoost = sprintSpeed;
        else
            speedBoost = 1f;


        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * (baseSpeed + speedBoost) * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && isGrounded())
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }

    public void Teleport(Vector3 location)
    {
        controller.enabled = false; // Disable the controller so we can teleport
        controller.transform.SetPositionAndRotation(location, transform.rotation);
        controller.enabled = true;  // Re-enable it
    }


    private bool isGrounded()
    {
        Vector3 capsuleBottom = new Vector3(controller.bounds.center.x, controller.bounds.min.y, controller.bounds.center.z);

        bool grounded = Physics.CheckCapsule(controller.bounds.center, capsuleBottom, 0.2f, groundLayer, QueryTriggerInteraction.Ignore);
        return grounded;
    }

}
