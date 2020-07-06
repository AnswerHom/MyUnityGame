using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : Creature {
    public GameObject skinObj;

    protected override void Awake()
    {
        base.Awake();
        slider = transform.Find("Canvas/Slider").GetComponent<Slider>();
    }

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
