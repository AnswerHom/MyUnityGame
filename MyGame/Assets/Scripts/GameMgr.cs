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

public class GameMgr : MonoBehaviour {
    private Vector3 bornPoint1;
    private Vector3 bornPoint2;
    private float timer = 0;
    private List<Enemy> enemyList;
    private Player player;
    /// <summary>
    /// 血条
    /// </summary>
    private List<Scrollbar> barList;
    private GameObject sceneCanvas; 

    void Awake()
    {
        bornPoint1 = GameObject.Find("BornPoint1").transform.position;
        bornPoint2 = GameObject.Find("BornPoint2").transform.position;
        enemyList = new List<Enemy>();
        barList = new List<Scrollbar>();
        sceneCanvas = GameObject.Find("SceneCanvas");
    }

	// Use this for initialization
	void Start () {
        //初始化玩家
        ObjectPool.GetInstance().outPool("Prefabs/Player", OnMainPlayer);
        timer = Random.Range(0.2f, 1.5f);
    }

    private void OnMainPlayer(GameObject playerObj)
    {
        playerObj.transform.position = Vector3.zero;
        player = playerObj.GetComponent<Player>();
        player.init();
        EventManager.GetInstance().EventTrigger(EventManager.EVENT_MAIN_PLAYER);
    }
	
	// Update is called once per frame
	void Update () {
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
        timer -= Time.deltaTime;
        if(timer <= 0)
        {
            timer = Random.Range(1f, 5f);
            RandomEnemy();
        }
    }

    private void RandomEnemy(){
        EnemyType type = 0;
        int randomNum = Random.Range(0, 100);
        if(randomNum >= 0 && randomNum < 50)
        {
            type = EnemyType.ENEMY_BEAR;
        }
        else if(randomNum >= 50 && randomNum < 80)
        {
            type = EnemyType.ENEMY_BUNNY;
        }
        else if(randomNum >= 80 && randomNum <= 100)
        {
            type = EnemyType.ENEMY_HELLEPHANT;
        }
        CreateEnemy(type);
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
            if(random >= 0 && random < 5)
            {
                bornV = bornPoint1;
            }else
            {
                bornV = bornPoint2;
            }
            obj.transform.position = bornV;
            Enemy enemy = obj.GetComponent<Enemy>();
            enemy.init();
            enemyList.Add(enemy);
            GameObject bar = ResManager.GetInstance().LoadRes<GameObject>("Prefabs/HpBar");
            bar.transform.SetParent(sceneCanvas.transform);
            bar.transform.localScale = Vector3.one;
            FollowTarget ft = bar.GetComponent<FollowTarget>();
            ft.SetTarget(obj.transform);
        });
    }
}
