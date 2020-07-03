using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CreatureState
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
    /// 等待复活
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
    public float maxHp = 0;
    /// <summary>
    /// 当前血量
    /// </summary>
    public float hp = 0;

    /// <summary>
    /// 防御
    /// </summary>
    public float maxDefence = 0;
    /// <summary>
    /// 当前防御
    /// </summary>
    public float defence = 0;

    /// <summary>
    /// 攻击
    /// </summary>
    public float maxAttack = 0;
    /// <summary>
    /// 当前攻击
    /// </summary>
    public float attack = 0;

    /// <summary>
    /// 移动速度
    /// </summary>
    public float maxSpeed = 0;
    /// <summary>
    /// 当前速度
    /// </summary>
    public float speed = 0;

    /// <summary>
    /// 攻速
    /// </summary>
    public int attackSpeed = 1;
    /// <summary>
    /// 攻击范围
    /// </summary>
    public float attackRange = 0;

    /// <summary>
    /// 状态
    /// </summary>
    public CreatureState state = CreatureState.STATE_ALIVE;

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
        state = CreatureState.STATE_ALIVE;
    }
	
	// Update is called once per frame
	protected virtual void Update () {
        if (render)
        {
            render.material.color = Color.Lerp(render.material.color, Color.white, colorSpeed * Time.deltaTime);
        }
        //判断状态
        if(state != CreatureState.STATE_RELIFE) { 
            if(hp > 0)
            {
                state = CreatureState.STATE_ALIVE;
            }else
            {
                state = CreatureState.STATE_DEAD;
            }
        }
    }

    public virtual void TakeDamage(Creature target)
    {
        hp -= target.attack + defence;
        if (render)
        {
            //受伤变红
            render.materials[0].color = Color.red;
        }
    }

    /// <summary>
    /// 对象池出池初始化
    /// </summary>
    public virtual void init()
    {
        //初始化数值
        state = CreatureState.STATE_ALIVE;
        hp = maxHp;
        attack = maxAttack;
        speed = maxSpeed;
        defence = maxDefence;
    }
}
