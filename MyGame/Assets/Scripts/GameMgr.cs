using System.Collections;
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

public enum GameState
{
    /// <summary>
    /// 游戏开始
    /// </summary>
    GAME_STATE_START,
    /// <summary>
    /// 游戏结束
    /// </summary>
    GAME_STATE_END
}

public class GameMgr : BaseManager<GameMgr>
{
    public GameState state;
    private Vector3 bornPoint1;
    private Vector3 bornPoint2;
    private float timer = 0;
    private float itemTimer = 0;
    /// <summary>
    /// Boss来袭倒计时
    /// </summary>
    private float bossTimer = 0;
    public List<SceneItem> sceneItemList;
    private List<Enemy> enemyList;
    /// <summary>
    /// 蓝电池BUFF
    /// </summary>
    private BlueBattery blue = null;
    /// <summary>
    /// 黄电池BUFF
    /// </summary>
    private YellowBattery yellow = null;
    /// <summary>
    /// 主玩家
    /// </summary>
    private Player player;
    public Player GetMainPlayer()
    {
        return player;
    }
    private GameObject sceneCanvas;
    private bool hasBoss = false;

    public GameMgr()
    {
        bornPoint1 = GameObject.Find("BornPoint1").transform.position;
        bornPoint2 = GameObject.Find("BornPoint2").transform.position;
        enemyList = new List<Enemy>();
        sceneItemList = new List<SceneItem>();
        sceneCanvas = GameObject.Find("SceneCanvas");
        MonoManager.GetInstance().AddUpdateListener(Update);
        state = GameState.GAME_STATE_END;
    }

    /// <summary>
    /// 游戏开始
    /// </summary>
    public void StartGame()
    {
        Debug.Log("=======> 游戏开始");
        if (state == GameState.GAME_STATE_START) return;
        //初始化玩家
        ObjectPool.GetInstance().outPool("Prefabs/Player", OnMainPlayer);
        timer = Random.Range(0.2f, 1.5f);
        itemTimer = 1;
        state = GameState.GAME_STATE_START;
        UIManager.GetInstance().ShowPanel<HudPanel>("PanelHud", UILayer.MIDDLE);
        ResetBossTime();
    }

    private void OnMainPlayer(GameObject playerObj)
    {
        playerObj.transform.position = Vector3.zero;
        player = playerObj.GetComponent<Player>();
        player.init();
        EventManager.GetInstance().EventTrigger(EventManager.EVENT_MAIN_PLAYER);
    }

    /// <summary>
    /// 游戏结束
    /// </summary>
    public void EndGame()
    {
        Debug.Log("=======> 游戏结束");
        if (state == GameState.GAME_STATE_END) return;
        state = GameState.GAME_STATE_END;
        //回收主玩家
        ObjectPool.GetInstance().intoPool(player.gameObject);
        //回收敌人
        while (enemyList.Count > 0)
        {
            Enemy enemy = enemyList[0];
            ObjectPool.GetInstance().intoPool(enemy.gameObject);
            enemyList.Remove(enemy);
        }
        //回收物品
        if (blue != null)
        {
            ObjectPool.GetInstance().intoPool(blue.gameObject);
            blue = null;
        }
        if (yellow != null)
        {
            ObjectPool.GetInstance().intoPool(yellow.gameObject);
            yellow = null;
        }
        while (sceneItemList.Count > 0)
        {
            SceneItem item = sceneItemList[0];
            ObjectPool.GetInstance().intoPool(item.gameObject);
            sceneItemList.Remove(item);
        }
        UIManager.GetInstance().HidePanel("PanelHud");
        UIManager.GetInstance().ShowPanel<StartPanel>("PanelStart",UILayer.MIDDLE);
    }

    // Update is called once per frame
    void Update()
    {
        if (state == GameState.GAME_STATE_END) return;
        //检查主玩家状态
        if (player != null && player.state == CreatureState.STATE_RELIFE)
        {
            EndGame();
        }
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
        for (int i = 0; i < sceneItemList.Count; i++)
        {
            SceneItem item = sceneItemList[i];
            if (item && item.state == SceneItemState.STATE_INACTIVE)
            {
                ObjectPool.GetInstance().intoPool(item.gameObject);
                sceneItemList.Remove(item);
                i--;
            }
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
        //BOSS
        bossTimer -= Time.deltaTime;
        if(!hasBoss && bossTimer <= 0)
        {
            CreateBoss();
            hasBoss = true;
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

    private void CreateBoss()
    {
        ObjectPool.GetInstance().outPool("Prefabs/Boss", (GameObject obj) =>
        {
            Vector3 bornV = new Vector3(14.5f,0,7);
            obj.transform.position = bornV;
            Boss boss = obj.GetComponent<Boss>();
            boss.init();
            enemyList.Add(boss);
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

    public void ResetBossTime()
    {
        bossTimer = 30;
        hasBoss = false;
    }

    public float GetBossTime()
    {
        return bossTimer;
    }
}
