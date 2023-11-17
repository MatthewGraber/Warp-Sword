using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterferenceCollider : MonoBehaviour
{
    public bool avoiding = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Object")
        {
            avoiding = true;
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Object")
        {
            avoiding = false;
        }
    }
}
