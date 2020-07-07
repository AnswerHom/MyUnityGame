using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossControl : MonoBehaviour {
    private Boss boss;
    private Transform playerT;
    private Player player;
    private NavMeshAgent agent;
    private Animator anim;
    /// <summary>
    /// 攻击计时器
    /// </summary>
    private float attackTimer = 0;
    private float timer = 0;
    private CapsuleCollider col;
    private Vector3 targetV;
    private Transform firePoint;

    private void Awake()
    {
        boss = GetComponent<Boss>();
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        col = GetComponent<CapsuleCollider>();
        attackTimer = 1.0f / boss.attackSpeed;
        firePoint = transform.Find("FirePoint").transform;
    }

    // Use this for initialization
    void Start () {
        playerT = GameObject.FindGameObjectWithTag("Player").transform;
        player = playerT.GetComponent<Player>();
        //赋值给寻路导航
        agent.speed = boss.speed;
        agent.stoppingDistance = boss.attackRange;
        timer = 2;
    }
	
	// Update is called once per frame
	void Update () {
        if (boss.state == CreatureState.STATE_ALIVE)
        {
            agent.enabled = true;
            col.enabled = true;
            agent.SetDestination(playerT.position);
            anim.SetBool("isAttack", false);
            anim.SetBool("isIdle", true);
            if (agent.velocity == Vector3.zero)
            {
                //准备攻击
                attackTimer -= Time.deltaTime;
                if (attackTimer <= 0)
                {
                    if (player.state == CreatureState.STATE_ALIVE)
                    {
                        anim.SetBool("isAttack", true);
                    }
                    attackTimer = 1.0f / boss.attackSpeed;
                }
            }
            else
            {
                attackTimer = 1.0f / boss.attackSpeed;
            }
        }
        else if (boss.state == CreatureState.STATE_DEAD)
        {
            agent.enabled = false;
            agent.velocity = Vector3.zero;
            anim.SetBool("isDeath", true);
            col.enabled = false;
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                boss.state = CreatureState.STATE_RELIFE;
                timer = 2;
                anim.SetBool("isDeath", false);
                GameMgr.GetInstance().ResetBossTime();
            }
        }
    }

    public void OnBossAttack()
    {
        //再判断下距离
        float distance = Vector3.Distance(playerT.position, boss.transform.position);
        //Debug.Log("distance = " + distance + "    attackRange = " + boss.attackRange);
        if(distance <= boss.attackRange) {
            //面朝
            transform.rotation = Quaternion.LookRotation(playerT.position - transform.position);
            ObjectPool.GetInstance().outPool("Prefabs/BossFire", (GameObject obj) =>
            {
                obj.transform.position = firePoint.position;
                BossFire fire = obj.GetComponent<BossFire>();
                Vector3 dir = playerT.position - obj.transform.position;
                dir.y = 0;
                fire.SetBoss(boss, dir);
                GameMgr.GetInstance().sceneItemList.Add(fire);
            });
        }
    }
}
