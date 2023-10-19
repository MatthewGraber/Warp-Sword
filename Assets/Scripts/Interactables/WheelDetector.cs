using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelDetector : Interactable
{
    public ShipBehavior ship;

    // Start is called before the first frame update
    void Start()
    {
        ship = GetComponentInParent<ShipBehavior>();   
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Interact()
    {
        if (ship.active)
        {
            ship.active = false;
            PlayerBehavior.Instance.state = PlayerState.active;
        }
        else
        {
            ship.active = true;
            PlayerBehavior.Instance.state = PlayerState.sailing;
        }
    }
}
