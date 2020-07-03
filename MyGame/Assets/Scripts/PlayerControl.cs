using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour {
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
        EventManager.GetInstance().AddEventListener<KeyCode>(EventManager.EVENT_KEY_DOWN, OnKeyDown);
    }

    private void OnKeyDown(KeyCode key)
    {
        if(key == KeyCode.Space)
        {
            if (player.shootType == ShootType.SHOOT_NORMAL)
                player.shootType = ShootType.SHOOT_MUTI;
            else
                player.shootType = ShootType.SHOOT_NORMAL;
        }
    }

    // Update is called once per frame
    void Update () {
        if(player.state == CreatureState.STATE_ALIVE) {
            //移动
            float x = Input.GetAxis("Horizontal");
            float y = Input.GetAxis("Vertical");
            body.MovePosition(transform.position + new Vector3(x, 0, y) * player.speed * Time.deltaTime);

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
        }else if(player.state == CreatureState.STATE_DEAD)
        {
            anim.SetBool("isDeath", true);
        }
    }
}
