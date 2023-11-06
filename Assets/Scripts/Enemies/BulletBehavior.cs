using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehavior : MonoBehaviour
{

    [SerializeField] float speed;
    [SerializeField] int damage;
    Vector3 direction;

    // Start is called before the first frame update
    void Start()
    {
        transform.parent = null;
    }

    // Update is called once per frame
    void Update()
    {
        transform.SetPositionAndRotation(transform.position + direction*speed*Time.deltaTime, transform.rotation);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player") { 
            PlayerBehavior.Instance.TakeDamage(damage);
        }

        Destroy(gameObject);
    }

    public void SetDirection(Vector3 dir)
    {
        direction = dir;
    }
}
