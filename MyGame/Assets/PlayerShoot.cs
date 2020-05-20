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

	// Use this for initialization
	void Start () {
        shootLight = this.GetComponent<Light>();
        shootLight.enabled = false;
        shootPart = this.GetComponentInChildren<ParticleSystem>();
        lineRender = this.GetComponent<LineRenderer>();
        lineRender.enabled = false;
    }

    // Update is called once per frame
    void Update () {
        timer += Time.deltaTime;
        if (Input.GetMouseButtonDown(0))
        {
            isMouseDown = true;
        }
        if (Input.GetMouseButtonUp(0))
        {
            isMouseDown = false;
        }
        if(isMouseDown && timer >= 1 / shootRate)
        {
            //可以发射
            timer = 0;
            Shoot();
        }
	}

    void Shoot()
    {
        shootLight.enabled = true;
        shootPart.Play();
        lineRender.enabled = true;
        //发射射线
        lineRender.SetPosition(0, transform.position);
        Ray ray = new Ray(transform.position,transform.forward);
        RaycastHit info;
        if(Physics.Raycast(ray,out info, shootRange))
        {
            lineRender.SetPosition(1, info.point);
        }else
        {
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
