using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : Creature {
    public GameObject skinObj;

    protected override void Awake()
    {
        base.Awake();
        Transform sliderObj = transform.Find("Canvas/Slider");
        if(sliderObj)
            slider = sliderObj.GetComponent<Slider>();
        render = skinObj.GetComponent<SkinnedMeshRenderer>();
    }

    // Use this for initialization
    protected override void Start () {
        base.Start();
    }
	
	// Update is called once per frame
	protected override void Update () {
        base.Update();
	}
}
