using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehavior : MonoBehaviour
{

    [SerializeField] float speed;
    [SerializeField] int damage;
    Vector3 direction;

    float DespawnCounter = 0f;

    // Start is called before the first frame update
    void Start()
    {
        transform.parent = null;
    }

    // Update is called once per frame
    void Update()
    {
        transform.SetPositionAndRotation(transform.position + direction*speed*Time.deltaTime, transform.rotation);
        DespawnCounter += Time.deltaTime;
        if (DespawnCounter > 5)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player") { 
            PlayerBehavior.Instance.TakeDamage(damage);
            Destroy(gameObject);
        }
        else if (other.gameObject.tag == "Object" || other.gameObject.tag == "MovingSurface")
            Destroy(gameObject);
    }

    public void SetDirection(Vector3 dir)
    {
        direction = dir;
    }
}
