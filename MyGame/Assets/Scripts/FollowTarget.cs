using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour {
    private Transform target;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (target)
        {
            transform.position = target.position;
        }
	}

    public void SetTarget(Transform t)
    {
        target = t;
    }
}
