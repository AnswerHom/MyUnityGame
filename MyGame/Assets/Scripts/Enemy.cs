using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Creature {
    public GameObject skinObj;

	// Use this for initialization
	protected override void Start () {
        base.Start();
        render = skinObj.GetComponent<SkinnedMeshRenderer>();
    }
	
	// Update is called once per frame
	protected override void Update () {
        base.Update();
	}
}
