using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour {
    /// <summary>
    /// 移动速度
    /// </summary>
    public float speed = 5;
    private Rigidbody body;
    private Animator anim;
    private int maskIndex;
    /// <summary>
    /// 玩家状态类
    /// </summary>
    private Player player;

    void Start () {
        body = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        maskIndex = LayerMask.GetMask("Ground");
        player = GetComponent<Player>();
    }

    // Update is called once per frame
    void Update () {
        if(player.state == PlayerState.STATE_ALIVE) {
            //移动
            float x = Input.GetAxis("Horizontal");
            float y = Input.GetAxis("Vertical");
            this.body.MovePosition(transform.position + new Vector3(x, 0, y) * speed * Time.deltaTime);

            //旋转
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit info;
            if (Physics.Raycast(ray, out info, 100, maskIndex))
            {
                Vector3 temp = info.point;
                temp.y = transform.position.y;
                this.body.rotation = Quaternion.LookRotation(temp - transform.position);
            }
            //动画
            if (x != 0 || y != 0)
            {
                anim.SetBool("isMove", true);
            }
            else
            {
                anim.SetBool("isMove", false);
            }
        }else if(player.state == PlayerState.STATE_DEAD)
        {
            anim.SetBool("isDeath", true);
        }
    }
}
