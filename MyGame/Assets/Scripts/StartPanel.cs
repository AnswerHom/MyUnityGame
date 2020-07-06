using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPanel : BasePanel {

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void OnClick(string name)
    {
        switch (name)
        {
            case "ButtonStart":
                GameMgr.GetInstance().StartGame();
                UIManager.GetInstance().HidePanel("PanelStart");
                break;
            default:
                break;
        }
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
