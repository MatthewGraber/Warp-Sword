using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeBehavior : MonoBehaviour
{
    private Animator animator;
    float COOLDOWN_TIME = 3.0f;
    int damage = 3;

    enum State
    {
        idle,
        swinging
    }

    State state = State.idle;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void Attack()
    {
        if (state == State.idle)
        {
            // Debug.Log("Axeman attacking!");
            animator.SetTrigger("Attack");
            state = State.swinging;
            StartCoroutine(SwingCooldown());
        }  
    }

    IEnumerator SwingCooldown()
    {
        yield return new WaitForSeconds(COOLDOWN_TIME);
        state = State.idle;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            PlayerBehavior.Instance.TakeDamage(damage);
        }
    }
}
