using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : Creature {
    /// <summary>
    /// 射击类型
    /// </summary>
    private ShootType _shootType = ShootType.SHOOT_NORMAL;
    public ShootType shootType
    {
        get { return _shootType; }
        set { ChangeShootType(value); }
    }

    private TrailRenderer trail;

    // Use this for initialization
    protected override void Awake () {
        base.Awake();
        render = transform.Find("Player").GetComponent<SkinnedMeshRenderer>();
        slider = transform.Find("Canvas/Slider").GetComponent<Slider>();
        trail = transform.Find("Trail").GetComponent<TrailRenderer>();
        trail.enabled = false;
    }

    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update () {
        base.Update();
    }

    public override void init()
    {
        base.init();
        ChangeShootType(ShootType.SHOOT_NORMAL);
    }

    /// <summary>
    /// 改变攻击形态
    /// </summary>
    public void ChangeShootType(ShootType type)
    {
        _shootType = type;
        //影响属性
        switch (type)
        {
            case ShootType.SHOOT_NORMAL://单道距离一般 攻击高 攻速快
                maxAttack = attack = 20;
                attackSpeed = 6;
                attackRange = 10;
                break;
            case ShootType.SHOOT_MUTI://散弹 距离短，攻击高，攻速慢
                maxAttack = attack = 50;
                attackSpeed = 1;
                attackRange = 5;
                break;
            default:
                break;
        }
        //BUFF应用
        for (int i = 0; i < buffList.Count; i++)
        {
            Buff buff = buffList[i];
            if(buff !=null && buff.state == BuffState.STATE_ENABLE)
            {
                buff.AttachBuff(this);
            }
        }
        EventManager.GetInstance().EventTrigger<ShootType>(EventManager.EVENT_MAIN_PLAYER_CHANGE_WEAPON, type);
    }

    protected override void OnAddBuff(Buff buff)
    {
        base.OnAddBuff(buff);
        if(buff.type == BuffType.BUFF_SPEED_UP)
        {
            trail.enabled = true;
        }
    }

    protected override void OnRemoveBuff(Buff buff)
    {
        base.OnRemoveBuff(buff);
        if (buff.type == BuffType.BUFF_SPEED_UP)
        {
            trail.enabled = false;
        }
    }
}
