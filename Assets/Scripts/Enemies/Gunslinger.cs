using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gunslinger : BasicEnemy
{
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] GameObject gun;

    float COOLDOWN_TIME = 2f;
    float cooldown = 0;

    // Start is called before the first frame update
    void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
    }


    private void FixedUpdate()
    {
        base.FixedUpdate();
        
        if (cooldown < COOLDOWN_TIME)
        {
            cooldown += Time.fixedDeltaTime;
        }
        if (DistanceFromPlayer() >= minDistance)
        {
            if (cooldown >= COOLDOWN_TIME)
            {
                Attack();
            }
        }
    }


    private void Attack()
    {
        // Set the direction of firing
        Vector3 direction = new Vector3(transform.forward.x, DirectionToPlayer().y, transform.forward.z);
        direction = direction.normalized;

        GameObject newBullet = Instantiate(bulletPrefab);
        newBullet.GetComponent<BulletBehavior>().SetDirection(direction);

        cooldown = 0;
    }
}
