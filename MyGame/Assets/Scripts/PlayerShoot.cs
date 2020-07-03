using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ShootType
{
    SHOOT_NORMAL,//单道
    SHOOT_MUTI,//散弹
}

public class PlayerShoot : MonoBehaviour {
    private float timer = 0;
    private Light shootLight;
    private ParticleSystem shootPart;
    private bool isMouseDown = false;
    private LineRenderer lineRender;
    private Player player;

	// Use this for initialization
	void Awake () {
        shootLight = GetComponent<Light>();
        shootLight.enabled = false;
        shootPart = GetComponentInChildren<ParticleSystem>();
        lineRender = GetComponent<LineRenderer>();
        lineRender.enabled = false;
        player = GetComponentInParent<Player>();
        InputManager.GetInstance();
        EventManager.GetInstance().AddEventListener<int>(EventManager.EVENT_MOUSE_DOWN, OnMouseDownEvent);
        EventManager.GetInstance().AddEventListener<int>(EventManager.EVENT_MOUSE_UP, OnMouseUpEvent);
    }

    private void OnDestroy()
    {
        EventManager.GetInstance().RemoveEventListener<int>(EventManager.EVENT_MOUSE_DOWN, OnMouseDownEvent);
        EventManager.GetInstance().RemoveEventListener<int>(EventManager.EVENT_MOUSE_UP, OnMouseUpEvent);
    }

    private void OnMouseDownEvent(int key)
    {
        if(key == 0)
        {
            isMouseDown = true;
        }
    }

    private void OnMouseUpEvent(int key)
    {
        if(key == 0)
        {
            isMouseDown = false;
        }
    }

    // Update is called once per frame
    void Update () {
        timer += Time.deltaTime;
        if(isMouseDown && (timer >= 1.0f / player.attackSpeed))
        {
            //可以发射
            timer = 0;
            Shoot();
        }
	}

    void Shoot()
    {
        if (player.state != CreatureState.STATE_ALIVE) return;
        shootLight.enabled = true;
        //粒子特效播放
        shootPart.Play();
        lineRender.enabled = true;
        switch (player.shootType)
        {
            case ShootType.SHOOT_NORMAL:
                lineRender.positionCount = 2;
                DrawShootLine(0, 0);
                break;
            case ShootType.SHOOT_MUTI:
                int idx = 0;
                lineRender.positionCount = 10;
                for (int i = -2; i <= 2; i++)
                {
                    DrawShootLine(idx++, i * 10);
                }
                break;
            default:
                break;
        }
        
        Invoke("EndShoot", 0.05f);
    }
    
    /// <summary>
    /// 绘制弹道
    /// </summary>
    private void DrawShootLine(int idx , float angle)
    {
        Vector3 tempV = Quaternion.AngleAxis(angle, transform.up) * transform.forward;
        tempV = tempV.normalized;
        //起始位置
        lineRender.SetPosition(2* idx, transform.position);
        //发射射线
        Ray ray = new Ray(transform.position, tempV);
        RaycastHit info;
        if (Physics.Raycast(ray, out info, player.attackRange))
        {
            //有障碍物
            lineRender.SetPosition(2 * idx+1, info.point);
            if (info.transform.tag == "Enemy")
            {
                Enemy obj = info.transform.GetComponent<Enemy>();
                obj.TakeDamage(player);
            }
        }
        else
        {
            //无障碍最大发射距离
            lineRender.SetPosition(2 * idx+1, transform.position + tempV * player.attackRange);
        }
    }

    void EndShoot()
    {
        lineRender.enabled = false;
        shootLight.enabled = false;
    }
}
