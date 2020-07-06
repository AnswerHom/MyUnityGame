using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    /// <summary>
    /// 血条
    /// </summary>
    protected Slider slider;
    /// <summary>
    /// BUFF
    /// </summary>
    protected List<Buff> buffList;

    protected virtual void Awake()
    {
        buffList = new List<Buff>();
    }

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
                if (slider)
                {
                    slider.transform.rotation = Quaternion.LookRotation(transform.position - Camera.main.transform.position);
                }
                //BUFF检测
                for (int i = 0; i < buffList.Count; i++)
                {
                    Buff buff = buffList[i];
                    if (buff == null) continue;
                    buff.Update();
                    if(buff.state == BuffState.STATE_DISABLE)
                    {
                        buffList.Remove(buff);
                        OnRemoveBuff(buff);
                        i--;
                    }
                }
            }
            else
            {
                state = CreatureState.STATE_DEAD;
                if (slider)
                {
                    slider.gameObject.SetActive(false);
                }
                buffList.Clear();
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
        if (slider)
        {
            slider.value = hp / maxHp;
        }
    }

    /// <summary>
    /// 增加BUFF
    /// </summary>
    /// <param name="buff"></param>
    public virtual void AddBuff(BuffType type)
    {
        Buff buff = null;
        for (int i = 0; i < buffList.Count; i++)
        {
            Buff temp = buffList[i];
            if(temp.type == type)
            {
                buff = temp;
                break;
            }
        }
        if (buff != null)
        {
            //刷新BUFF时间即可
            buff.RefreshBuff();
        }else {
            //新加BUFF
            switch (type)
            {
                case BuffType.BUFF_SPEED_UP:
                    buff = new SpeedUpBuff();
                    break;
                case BuffType.BUFF_ATTACK_UP:
                    buff = new AttackUpBuff();
                    break;
                default:
                    break;
            }
            if (buff != null)
            {
                buff.AttachBuff(this);
                buff.RefreshBuff();
                buffList.Add(buff);
                OnAddBuff(buff);
            }
        }
    }

    protected virtual void OnAddBuff(Buff buff)
    {

    }

    protected virtual void OnRemoveBuff(Buff buff)
    {

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
        buffList.Clear();
        if (slider)
        {
            slider.gameObject.SetActive(true);
            slider.value = hp / maxHp;
        }
    }
}
