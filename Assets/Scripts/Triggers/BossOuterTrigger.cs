using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossOuterTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            MusicManager.Instance.FadeStarting();
        }
    }
}
