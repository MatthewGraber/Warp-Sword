using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Axeman : BasicEnemy
{

    [SerializeField] GameObject axe;
    private AxeBehavior axeBehavior;

    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        axeBehavior = axe.GetComponent<AxeBehavior>();
    }

    // Update is called once per frame
    protected void Update()
    {
        base.Update();
    }


    protected void FixedUpdate()
    {
        base.FixedUpdate();
        if (DistanceFromPlayer() < 3 && State == EnemyState.Active) {
            axeBehavior.Attack();
        }
    }


}
