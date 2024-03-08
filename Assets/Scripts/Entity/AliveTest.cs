using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AliveTest : EntityAlive
{
    public override void Init()
    {
        base.Init();
        maxHealth = 10;
        healthbarOffset = new Vector2(0, 1);
        healthbarScale = 1;
    }

    public override void WhileAlive()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            TakeDamage(1);
        }

    }
}
