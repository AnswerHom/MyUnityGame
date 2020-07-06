using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStart : MonoBehaviour {

	// Use this for initialization
	void Start () {
        UIManager.GetInstance().ShowPanel<StartPanel>("PanelStart", UILayer.MIDDLE);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
