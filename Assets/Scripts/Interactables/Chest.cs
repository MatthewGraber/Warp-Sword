using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : Interactable
{
    bool used = false;

    enum LootType
    {
        SpeedBoost,
        DamageBoost,
        ManaBoost,
        HealthBoost
    }

    LootType type;

    // Start is called before the first frame update
    void Start()
    {
        type = (LootType) Random.Range(0, LootType.GetValues(typeof(LootType)).Length);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public override void Interact()
    {
        if (used) return;

        switch (type)
        {
            case LootType.SpeedBoost:
                PlayerBehavior.Instance.baseSpeed += 1;
                break;

            case LootType.DamageBoost:
                SwordBehavior.Instance.damageMult += 0.2f;
                break;

            case LootType.ManaBoost:
                PlayerBehavior.Instance.maxMana += 1;
                break;

            case LootType.HealthBoost:
                PlayerBehavior.Instance.maxHealth += 1;
                break;

            default:
                break;
        }
        used = true;
    }
}
