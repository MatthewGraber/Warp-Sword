using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void Interact()
    {

    }

    public void OnTriggerEnter(Collider other)
    {
        PlayerBehavior.Instance.currentInteractable = this;
    }

    public void OnTriggerExit(Collider other)
    {
        if (PlayerBehavior.Instance.currentInteractable == this) 
        {
            PlayerBehavior.Instance.currentInteractable = null;
        }
    }
}
