using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public enum PlayerState
{
    active,
    sailing,
    dead
}

public class PlayerBehavior : MonoBehaviour
{

    public static PlayerBehavior Instance;

    [SerializeField] public Camera mainCam;
    [SerializeField] Camera shipCam;

    public CharacterController controller;

    // Speed variables
    public float baseSpeed;
    public float sprintSpeed = 3f;
    private float slowSpeed = 0.5f;
    private float speedMult = 1f;

    public float gravity = -9.81f;
    public float jumpHeight = 3f;

    public float bouyancy = 10f;
    public float waterLevel = 0;
    
    //public GameObject sword;

    public LayerMask groundLayer;


    public PlayerState state = PlayerState.active;

    public Interactable currentInteractable;


    // Mana costs
    private float JUMP_COST = 2;

    bool invincible = false;

    float health;
    public float maxHealth = 10;
    public float HP { get { return health; }
        set
        {
            if (state != PlayerState.dead)
            {
                health = value;
                if (health < 0)
                {
                    health = 0;
                    state = PlayerState.dead;
                    GameManager.Instance.PlayerDied();
                }

                else if (health > maxHealth) health = maxHealth;

                CanvasBehavior.instance.DisplayHealth(health, maxHealth);
            }
        }
    }

    float mana;
    public float maxMana = 10;
    public float MP { get { return mana; }
        set
        {
            if (state != PlayerState.dead)
            {
                mana = value;
                if (mana < 0) mana = 0;

                if (mana > maxMana) mana = maxMana;

                CanvasBehavior.instance.DisplayMana(mana, maxMana);
            }
        }
    }



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

            if (Input.GetKey(KeyCode.E))
            {
                if (MP > 0)
                {
                    speedMult = slowSpeed;
                    HP += Time.deltaTime * 4;
                    MP -= Time.deltaTime * 8;
                }
            }
            else if (Input.GetButton("Fire3"))
            {
                if (MP > 0)
                {
                    speedMult = sprintSpeed;
                    MP -= Time.deltaTime;
                }
            }
            else
            {
                speedMult = 1;
                // Regain mana
                MP += Time.deltaTime / 2;
            }
                

            Vector3 move = transform.right * x + transform.forward * z;

            // Move the player horizontally
            controller.Move(move * (baseSpeed*speedMult) * Time.deltaTime);

            // Jump!
            if (Input.GetButtonDown("Jump"))
            {
                if (isGrounded())
                    velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
                else if (MP >= JUMP_COST)
                {
                    velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
                    MP -= JUMP_COST;
                    TutorialBehavior.Instance.Jump();
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
            
            // Regain mana
            MP += Time.deltaTime / 2;
        }
        else if (state == PlayerState.dead)
        {

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

}
