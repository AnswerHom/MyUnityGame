using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 场景道具--黄色电池
/// </summary>
public class YellowBattery : SceneItem
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    protected override void OnPlayerEnter(GameObject player)
    {
        base.OnPlayerEnter(player);
        Player p = player.GetComponent<Player>();
        p.AddBuff(BuffType.BUFF_ATTACK_UP);
        state = SceneItemState.STATE_INACTIVE;
    }
}
