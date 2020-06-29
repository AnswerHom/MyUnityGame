using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour {
    public float speed = 5;
    // Use this for initialization
    private Rigidbody body;
    private Animator anim;
    private int maskIndex;
    void Start () {
        this.body = this.GetComponent<Rigidbody>();
        this.anim = this.GetComponent<Animator>();
        this.maskIndex = LayerMask.GetMask("Ground");
    }


    private void FixedUpdate()
    {
        this.body.velocity = new Vector3(0, 0, 0);
    }

    // Update is called once per frame
    void Update () {
        //移动
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        this.body.MovePosition(transform.position + new Vector3(x, 0, y) * speed * Time.deltaTime); 
        
        //旋转
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit info;
        if(Physics.Raycast(ray, out info, 100, maskIndex))
        {
            Vector3 temp = info.point;
            temp.y = transform.position.y;
            this.body.rotation = Quaternion.LookRotation(temp - transform.position);
        }

        //动画
        if (x != 0 || y != 0)
        {
            this.anim.SetBool("isMove", true);
        }
        else
        {
            this.anim.SetBool("isMove", false);
        }
    }
}
