using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HudPanel : BasePanel {

    private Text txtWeapon;
    private Text txtBuff;
    private Text txtBoss;
    private Player player;

    protected override void Awake()
    {
        base.Awake();
        txtWeapon = transform.Find("TextWeapon").GetComponent<Text>();
        txtBuff = transform.Find("TextBuff").GetComponent<Text>();
        txtBoss = transform.Find("TextBoss").GetComponent<Text>();
        txtBoss.text = "";
        EventManager.GetInstance().AddEventListener(EventManager.EVENT_MAIN_PLAYER, OnMainPlayer);
        EventManager.GetInstance().AddEventListener<ShootType>(EventManager.EVENT_MAIN_PLAYER_CHANGE_WEAPON, OnMainPlayerChangeWeapon);
    }

    // Use this for initialization
    void Start () {
        OnMainPlayer();
    }

    /// <summary>
    /// 主玩家下来
    /// </summary>
    private void OnMainPlayer()
    {
        Debug.Log("======> 主玩家诞生");
        player = GameMgr.GetInstance().GetMainPlayer();
        OnMainPlayerChangeWeapon(player.shootType);
    }

    /// <summary>
    /// 玩家武器变化
    /// </summary>
    /// <param name="type"></param>
    private void OnMainPlayerChangeWeapon(ShootType type)
    {
        switch (type)
        {
            case ShootType.SHOOT_NORMAL:
                txtWeapon.text = "当前武器：步枪";
                break;
            case ShootType.SHOOT_MUTI:
                txtWeapon.text = "当前武器：散弹枪";
                break;
            default:
                break;
        }
    }

    // Update is called once per frame
    void Update () {
        txtBuff.text = "";
        txtBoss.text = "";
        if (player.buffList.Count > 0)
        {
            for (int i = 0; i < player.buffList.Count; i++)
            {
                Buff buff = player.buffList[i];
                if(buff.state == BuffState.STATE_ENABLE)
                {
                    txtBuff.text += buff.GetBuffName() + "剩余：" + buff.GetRemainTime() + "s\n"; 
                }
            }
        }
        float bossTime = GameMgr.GetInstance().GetBossTime();
        if (bossTime > 0)
        {
            txtBoss.text = "BOSS来袭倒计时：" + Mathf.Ceil(bossTime);
        }else
        {
            txtBoss.text = "BOSS来袭...";
        }
	}
}
