using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour {
    public int shootRate = 2;//1秒2发子弹
    public float shootRange = 30;
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
        if(isMouseDown && (timer >= 1.0f / shootRate))
        {
            //可以发射
            timer = 0;
            Shoot();
        }
	}

    void Shoot()
    {
        if (player.state != PlayerState.STATE_ALIVE) return;
        shootLight.enabled = true;
        //粒子特效播放
        shootPart.Play();
        lineRender.enabled = true;
        //起始位置
        lineRender.SetPosition(0, transform.position);
        //发射射线
        Ray ray = new Ray(transform.position,transform.forward);
        RaycastHit info;
        if(Physics.Raycast(ray,out info, shootRange))
        {
            //有障碍物
            lineRender.SetPosition(1, info.point);
        }else
        {
            //无障碍最大发射距离
            lineRender.SetPosition(1, transform.position + transform.forward * shootRange);
        }
        Invoke("EndShoot", 0.05f);
    }

    void EndShoot()
    {
        lineRender.enabled = false;
        shootLight.enabled = false;
    }
}
