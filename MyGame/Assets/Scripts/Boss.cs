using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss : Enemy {
    private float bornTimer;

    protected override void Awake()
    {
        base.Awake();
    }

    public override void init()
    {
        base.init();
        state = CreatureState.STATE_BORN;
        bornTimer = 0.5f;
    }

    protected override void Update()
    {
        if(bornTimer > 0)
        {
            bornTimer -= Time.deltaTime;
            state = CreatureState.STATE_BORN;
        }else {
            base.Update();
        }
    }
}
