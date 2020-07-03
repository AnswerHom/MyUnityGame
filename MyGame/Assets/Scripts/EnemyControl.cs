using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyControl : MonoBehaviour {
    private NavMeshAgent agent;
    private Transform playerT;
    private Player player;
    private Enemy enemy;
    private Animator anim;
    private Vector3 targetV;
    /// <summary>
    /// 释放延迟时间
    /// </summary>
    private float timer = 0;
    private CapsuleCollider col;

    /// <summary>
    /// 攻击计时器
    /// </summary>
    private float attackTimer = 0;

	// Use this for initialization
	void Awake () {
        agent = GetComponent<NavMeshAgent>();
        enemy = GetComponent<Enemy>();
        anim = GetComponent<Animator>();
        targetV = new Vector3(0, -3, 0);
        col = GetComponent<CapsuleCollider>();
        attackTimer = 1.0f / enemy.attackSpeed;
    }

    private void Start()
    {
        playerT = GameObject.FindGameObjectWithTag("Player").transform;
        player = playerT.GetComponent<Player>();
        //赋值给寻路导航
        agent.speed = enemy.speed;
        agent.stoppingDistance = enemy.attackRange;
        timer = 2;
    }

    // Update is called once per frame
    void Update () {
        if (enemy.state == CreatureState.STATE_ALIVE) {
            agent.enabled = true;
            col.enabled = true;
            agent.SetDestination(playerT.position);
            if (agent.velocity == Vector3.zero)
            {
                anim.SetBool("isMove", false);
                //准备攻击
                attackTimer -= Time.deltaTime;
                if (attackTimer <= 0)
                {
                    if(player.state == CreatureState.STATE_ALIVE) {
                        player.TakeDamage(enemy);
                    }
                    attackTimer = 1.0f / enemy.attackSpeed;
                }
            }else
            {
                attackTimer = 1.0f / enemy.attackSpeed;
                anim.SetBool("isMove", true);
            }
        }else if(enemy.state == CreatureState.STATE_DEAD)
        {
            agent.enabled = false;
            agent.velocity = Vector3.zero;
            anim.SetBool("isDeath", true);
            col.enabled = false;
            timer -= Time.deltaTime;
            if(timer <= 0)
            {
                FallDown();
            }
        }
    }

    /// <summary>
    /// 下落
    /// </summary>
    private void FallDown()
    {
        targetV.x = transform.position.x;
        targetV.z = transform.position.z;
        transform.position = Vector3.Lerp(transform.position, targetV, 1 * Time.deltaTime);
        if(Mathf.Abs(transform.position.y-targetV.y) < 0.2)
        {
            //等待释放
            enemy.state = CreatureState.STATE_RELIFE;
            timer = 2;
            anim.SetBool("isDeath", false);
        }
    }
}
