﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 敌人类型
/// </summary>
public enum EnemyType
{
    ENEMY_BEAR,
    ENEMY_BUNNY,
    ENEMY_HELLEPHANT
}

public class GameMgr : MonoBehaviour
{
    private Vector3 bornPoint1;
    private Vector3 bornPoint2;
    private float timer = 0;
    private float itemTimer = 0;
    private List<Enemy> enemyList;
    /// <summary>
    /// 蓝电池BUFF
    /// </summary>
    private BlueBattery blue = null;
    /// <summary>
    /// 黄电池BUFF
    /// </summary>
    private YellowBattery yellow = null;
    private Player player;
    private GameObject sceneCanvas;

    void Awake()
    {
        bornPoint1 = GameObject.Find("BornPoint1").transform.position;
        bornPoint2 = GameObject.Find("BornPoint2").transform.position;
        enemyList = new List<Enemy>();
        sceneCanvas = GameObject.Find("SceneCanvas");
    }

    // Use this for initialization
    void Start()
    {
        //初始化玩家
        ObjectPool.GetInstance().outPool("Prefabs/Player", OnMainPlayer);
        timer = Random.Range(0.2f, 1.5f);
        itemTimer = 1;
    }

    private void OnMainPlayer(GameObject playerObj)
    {
        playerObj.transform.position = Vector3.zero;
        player = playerObj.GetComponent<Player>();
        player.init();
        EventManager.GetInstance().EventTrigger(EventManager.EVENT_MAIN_PLAYER);
    }

    // Update is called once per frame
    void Update()
    {
        //检查死亡的敌人，进行回收
        for (int i = 0; i < enemyList.Count; i++)
        {
            Enemy enemy = enemyList[i];
            if (enemy && enemy.state == CreatureState.STATE_RELIFE)
            {
                ObjectPool.GetInstance().intoPool(enemy.gameObject);
                enemyList.Remove(enemy);
                i--;
            }
        }
        //检查无效的物品
        if(blue && blue.state == SceneItemState.STATE_INACTIVE)
        {
            ObjectPool.GetInstance().intoPool(blue.gameObject);
            blue = null;
        }
        if (yellow && yellow.state == SceneItemState.STATE_INACTIVE)
        {
            ObjectPool.GetInstance().intoPool(yellow.gameObject);
            yellow = null;
        }
        //敌人刷新
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            timer = Random.Range(1f, 5f);
            RandomEnemy();
        }
        //道具刷新
        itemTimer -= Time.deltaTime;
        if(itemTimer <= 0)
        {
            itemTimer = Random.Range(5f, 10f);
            RandomSceneItem();
        }
    }

    private void RandomEnemy()
    {
        EnemyType type = 0;
        int randomNum = Random.Range(0, 100);
        if (randomNum >= 0 && randomNum < 50)
        {
            type = EnemyType.ENEMY_BEAR;
        }
        else if (randomNum >= 50 && randomNum < 80)
        {
            type = EnemyType.ENEMY_BUNNY;
        }
        else if (randomNum >= 80 && randomNum <= 100)
        {
            type = EnemyType.ENEMY_HELLEPHANT;
        }
        CreateEnemy(type);
    }

    private void RandomSceneItem()
    {
        BuffType type = BuffType.BUFF_ATTACK_UP;
        int randomNum = Random.Range(0, 100);
        if (randomNum >= 0 && randomNum < 50)
        {
            type = BuffType.BUFF_ATTACK_UP;
            if (yellow != null) return;
        }
        else if (randomNum >= 50 && randomNum <= 100)
        {
            type = BuffType.BUFF_SPEED_UP;
            if (blue != null) return;
        }
        CreateSceneItem(type);
    }

    private void CreateEnemy(EnemyType type)
    {
        string path = "";
        switch (type)
        {
            case EnemyType.ENEMY_BEAR:
                path = "Prefabs/ZomBear";
                break;
            case EnemyType.ENEMY_BUNNY:
                path = "Prefabs/Zombunny";
                break;
            case EnemyType.ENEMY_HELLEPHANT:
                path = "Prefabs/Hellephant";
                break;
            default:
                path = "Prefabs/ZomBear";
                break;
        }
        ObjectPool.GetInstance().outPool(path, (GameObject obj) =>
        {
            int random = Random.Range(0, 10);
            Vector3 bornV = Vector3.zero;
            if (random >= 0 && random < 5)
            {
                bornV = bornPoint1;
            }
            else
            {
                bornV = bornPoint2;
            }
            obj.transform.position = bornV;
            Enemy enemy = obj.GetComponent<Enemy>();
            enemy.init();
            enemyList.Add(enemy);
        });
    }


    private void CreateSceneItem(BuffType type)
    {
        string path = "";
        Vector3 pos = new Vector3();
        switch (type)
        {
            case BuffType.BUFF_SPEED_UP:
                path = "Prefabs/Battery_blue";
                pos.x = -6f;
                pos.y = 0.5f;
                pos.z = 5.5f;
                break;
            case BuffType.BUFF_ATTACK_UP:
                path = "Prefabs/Battery_yellow";
                pos.x = -4f;
                pos.y = 0.5f;
                pos.z = -10f;
                break;
            default:
                break;
        }
        ObjectPool.GetInstance().outPool(path, (GameObject obj) =>
        {
            obj.transform.position = pos;
            SceneItem item = obj.GetComponent<SceneItem>();
            item.init();
            switch (type)
            {
                case BuffType.BUFF_SPEED_UP:
                    blue = item as BlueBattery;
                    break;
                case BuffType.BUFF_ATTACK_UP:
                    yellow = item as YellowBattery;
                    break;
            }
        });
    }
}
