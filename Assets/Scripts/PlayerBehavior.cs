using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public enum PlayerState
{
    active,
    sailing
}

public class PlayerBehavior : MonoBehaviour
{

    public static PlayerBehavior Instance;

    [SerializeField] Slider healthBar;
    [SerializeField] Slider manaBar;

    [SerializeField] Camera mainCam;
    [SerializeField] Camera shipCam;

    public CharacterController controller;
    public float baseSpeed = 12f;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;
    public float sprintSpeed = 5f;

    public float bouyancy = 10f;
    public float waterLevel = 0;
    
    public GameObject sword;

    public LayerMask groundLayer;


    public PlayerState state = PlayerState.active;

    public Interactable currentInteractable;


    // Mana costs
    private int JUMP_COST = 2;

    bool invincible = false;

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

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {
        health = maxHealth; 
        mana = maxMana;
    }

    void Update()
    {
        if (state == PlayerState.active)
        {
            // Camera
            if (!mainCam.enabled)
            {
                mainCam.enabled = true;
                shipCam.enabled = false;
            }

            // Falling
            if (controller.isGrounded && velocity.y < 0)
            {
                velocity.y = -2f;
            }


            // Get movement inputs
            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");

            if (Input.GetButton("Fire3"))
                speedBoost = sprintSpeed;
            else
                speedBoost = 1f;

            Vector3 move = transform.right * x + transform.forward * z;

            // Move the player horizontally
            controller.Move(move * (baseSpeed + speedBoost) * Time.deltaTime);

            // Jump!
            if (Input.GetButtonDown("Jump"))
            {
                if (isGrounded())
                    velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
                else if (MP >= JUMP_COST)
                {
                    velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
                    MP -= JUMP_COST;
                }
            }

            if (transform.position.y < waterLevel)
            {
                
                // Water floating physics
                if (velocity.y < 0)
                {
                    velocity.y += bouyancy * 5 * Time.deltaTime;
                }
                else if (velocity.y < bouyancy)
                {
                    velocity.y += bouyancy * Time.deltaTime;
                }
                else if (velocity.y > bouyancy)
                {
                    velocity.y = bouyancy;
                }

            }
            else
            {
                velocity.y += gravity * Time.deltaTime;
            }
            controller.Move(velocity * Time.deltaTime);

            
        }
        else if (state == PlayerState.sailing) 
        {
            if (!shipCam.enabled)
            {
                shipCam.enabled = true;
                mainCam.enabled = false;
            }
            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");

            Vector3 move = transform.right * x + transform.forward * z;
        }

        // Interact
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (currentInteractable != null)
            {
                currentInteractable.Interact();
            }
        }

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


    public void ChangeState(PlayerState newState)
    {
        switch (newState)
        {
            case PlayerState.sailing:
                {

                }
                break;
            case PlayerState.active:
                {

                }
                break;
        }
        state = newState;
    }


    public void TakeDamage(int damage)
    {
        if (!invincible)
        {
            HP -= damage;
            StartCoroutine(InvicibilityCooldown());
        }
    }


    // Prevents the player from being hit by the same object multiple times
    IEnumerator InvicibilityCooldown()
    {
        invincible = true;
        yield return new WaitForSeconds(0.5f);
        invincible = false;
    }


    /*public void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.layer == LayerMask.NameToLayer("MovingSurface"))
        {
            transform.parent = collision.transform;
            Debug.Log("Hit a ship!");
        }
        else
        {
            Debug.Log("Collided with something!");
        }
    }


    public void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("MovingSurface"))
        {
            transform.parent = null;
            Debug.Log("Left the ship!");
        }
    }*/

}
