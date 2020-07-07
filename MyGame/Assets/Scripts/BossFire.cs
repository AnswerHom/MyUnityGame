using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFire : SceneItem {
    public float speed = 15;

    private float timer = 0;
    private Vector3 dir = new Vector3(0,0,0);
    private Boss boss;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.position += dir * speed * Time.deltaTime;
        timer -= Time.deltaTime;
        if(timer <= 0)
        {
            state = SceneItemState.STATE_INACTIVE;
        }
    }

    public void SetBoss(Boss b,Vector3 d)
    {
        base.init();
        timer = 8;
        boss = b;
        dir = d.normalized;
    }

    protected override void OnPlayerEnter(GameObject player)
    {
        base.OnPlayerEnter(player);
        Player p = player.GetComponent<Player>();
        if(boss)
            p.TakeDamage(boss);
        state = SceneItemState.STATE_INACTIVE;
    }

    protected override void OnOtherEnter(GameObject other)
    {
        base.OnOtherEnter(other);
        state = SceneItemState.STATE_INACTIVE;
    }
}
