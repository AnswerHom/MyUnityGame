using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZomControl : MonoBehaviour {
    public GameObject target;
    public int index;
    public int maxIndex;
    public float stopRadius;
    public float moveSpeed;
    public float rotationSpeed;

    private Rigidbody body;
    private bool isMove = false;
	// Use this for initialization
	void Start () {
        this.body = this.GetComponent<Rigidbody>();
	}

    // Update is called once per frame
    void Update() {
        if (target)
        {
            float dis = Vector3.Distance(target.transform.position, transform.position);
            if(dis <= stopRadius) { 
                // 位置展开
                float angle = 2 * Mathf.PI * index / maxIndex;
                Vector3 temp = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle));
                temp = Vector3.Normalize(temp);
                transform.position = Vector3.Lerp(transform.position, target.transform.position + temp * stopRadius, moveSpeed * Time.deltaTime);
                //旋转
                transform.forward = Vector3.Lerp(transform.forward, transform.position - target.transform.position, rotationSpeed * Time.deltaTime);
            } else
            {
                //移动
                Vector3 temp = target.transform.position - transform.position;
                temp = Vector3.Normalize(temp);
                transform.position = Vector3.Lerp(transform.position, target.transform.position + temp * stopRadius, moveSpeed * Time.deltaTime);
                //旋转
                transform.forward = Vector3.Lerp(transform.forward, target.transform.position - transform.position, rotationSpeed * Time.deltaTime);
            }
        }
	}
}
