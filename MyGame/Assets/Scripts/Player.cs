using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    // Use this for initialization
    void Awake () {
        render = transform.Find("Player").GetComponent<SkinnedMeshRenderer>();
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
                attack = 20;
                attackSpeed = 6;
                attackRange = 10;
                break;
            case ShootType.SHOOT_MUTI://散弹 距离短，攻击高，攻速慢
                attack = 50;
                attackSpeed = 1;
                attackRange = 5;
                break;
            default:
                break;
        }
    }
}
