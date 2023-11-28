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
        string message = "You found a ";

        switch (type)
        {
            case LootType.SpeedBoost:
                PlayerBehavior.Instance.baseSpeed += 1;
                message += "speed boost! + 1 speed";
                break;

            case LootType.DamageBoost:
                SwordBehavior.Instance.damageMult += 0.2f;
                message += "damage boost! + 20% damage!";
                break;

            case LootType.ManaBoost:
                PlayerBehavior.Instance.maxMana += 1;
                PlayerBehavior.Instance.MP += 1;
                message += "mana boost! + 1 max mana!";
                break;

            case LootType.HealthBoost:
                PlayerBehavior.Instance.maxHealth += 1;
                PlayerBehavior.Instance.HP += 1;
                message += "health boost! + 1 max health";
                break;

            default:
                break;
        }
        GameManager.Instance.Message(message, 2.0f);
        used = true;
    }
}
