using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BuffState
{
    /// <summary>
    /// 生效
    /// </summary>
    STATE_ENABLE,
    /// <summary>
    /// 失效
    /// </summary>
    STATE_DISABLE
}

/// <summary>
/// BUFF类型
/// </summary>
public enum BuffType
{
    /// <summary>
    /// 速度提升
    /// </summary>
    BUFF_SPEED_UP,
    /// <summary>
    /// 攻击提升
    /// </summary>
    BUFF_ATTACK_UP,
}

/// <summary>
/// BUFF类
/// </summary>
public class Buff  {
    /// <summary>
    /// BUFF类型
    /// </summary>
    public BuffType type;

    /// <summary>
    /// BUFF状态
    /// </summary>
    public BuffState state = BuffState.STATE_DISABLE;

    /// <summary>
    /// 持续时间
    /// </summary>
    public float maxTime = 0;

    protected Creature attachCreature;

    /// <summary>
    /// 计时器
    /// </summary>
    public float timer;
    /// <summary>
    /// 心跳
    /// </summary>
    public void Update()
    {
        if (state == BuffState.STATE_DISABLE) return;
        timer -= Time.deltaTime;
        if(timer <= 0)
        {
            UnattachBuff(attachCreature);
        }
    }

    /// <summary>
    /// 生物使用BUFF
    /// </summary>
    public virtual void AttachBuff(Creature creature)
    {
        state = BuffState.STATE_ENABLE;
        attachCreature = creature;
    }

    /// <summary>
    /// 刷新BUFF时间
    /// </summary>
    public void RefreshBuff()
    {
        state = BuffState.STATE_ENABLE;
        timer = maxTime;
    }

    /// <summary>
    /// 生物消除BUFF
    /// </summary>
    public virtual void UnattachBuff(Creature creature)
    {
        state = BuffState.STATE_DISABLE;
    }

    public virtual string GetBuffName()
    {
        return "";
    }

    public string GetRemainTime()
    {
        return Mathf.Ceil(timer) + "";
    }
}


/// <summary>
/// 加速BUFF
/// </summary>
public class SpeedUpBuff:Buff
{
    public SpeedUpBuff()
    {
        type = BuffType.BUFF_SPEED_UP;
        maxTime = 10;
    }

    public override void AttachBuff(Creature creature)
    {
        base.AttachBuff(creature);
        creature.speed = 2 * creature.maxSpeed;
    }

    public override void UnattachBuff(Creature creature)
    {
        base.UnattachBuff(creature);
        creature.speed = creature.maxSpeed;
    }

    public override string GetBuffName()
    {
        return "疾行状态(2倍移速)";
    }
}

/// <summary>
/// 增加攻击BUFF
/// </summary>
public class AttackUpBuff:Buff
{
    public AttackUpBuff()
    {
        type = BuffType.BUFF_ATTACK_UP;
        maxTime = 10;
    }

    public override void AttachBuff(Creature creature)
    {
        base.AttachBuff(creature);
        creature.attack = 3 * creature.maxAttack;
    }

    public override void UnattachBuff(Creature creature)
    {
        base.UnattachBuff(creature);
        creature.attack = creature.maxAttack;
    }

    public override string GetBuffName()
    {
        return "疯狂状态(3倍攻击)";
    }
}