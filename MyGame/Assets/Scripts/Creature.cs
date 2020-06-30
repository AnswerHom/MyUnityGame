using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState
{
    /// <summary>
    /// 活着
    /// </summary>
    STATE_ALIVE,
    /// <summary>
    /// 死亡
    /// </summary>
    STATE_DEAD,
    /// <summary>
    /// 复活
    /// </summary>
    STATE_RELIFE
}

/// <summary>
/// 生物基类
/// </summary>
public class Creature : MonoBehaviour {
    /// <summary>
    /// 血量
    /// </summary>
    public float hp = 0;
    /// <summary>
    /// 防御
    /// </summary>
    public float defence = 0;
    /// <summary>
    /// 攻击
    /// </summary>
    public float attack = 0;
    /// <summary>
    /// 状态
    /// </summary>
    public PlayerState state = PlayerState.STATE_ALIVE;

    /// <summary>
    /// 皮肤
    /// </summary>
    protected SkinnedMeshRenderer render;
    /// <summary>
    /// 变色速度
    /// </summary>
    private float colorSpeed = 5;


    // Use this for initialization
    protected virtual void Start () {
        state = PlayerState.STATE_ALIVE;
    }
	
	// Update is called once per frame
	protected virtual void Update () {
        if (render)
        {
            render.material.color = Color.Lerp(render.material.color, Color.white, colorSpeed * Time.deltaTime);
        }
        //判断状态
        if(hp > 0)
        {
            state = PlayerState.STATE_ALIVE;
        }else
        {
            state = PlayerState.STATE_DEAD;
        }
	}

    public virtual void TakeDamage(Creature target)
    {
        hp -= target.attack + defence;
        if (render)
        {
            //受伤变红
            render.material.color = Color.red;
        }
    }
}
