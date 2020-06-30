using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Creature {
    private float timer = 2;
    private Creature target;

	// Use this for initialization
	void Awake () {
        render = transform.Find("Player").GetComponent<SkinnedMeshRenderer>();
        target = new Creature();
        target.attack = 100;
    }

    // Update is called once per frame
    protected override void Update () {
        base.Update();
        timer -= Time.deltaTime;
        if(timer <= 0)
        {
            timer = 2;
            TakeDamage(target);
        }

    }
}
