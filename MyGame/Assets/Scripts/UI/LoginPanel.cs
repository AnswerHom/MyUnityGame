using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginPanel : BasePanel {

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void OnClick(string name)
    {
        switch (name)
        {
            case "btn_login":
                Debug.Log("===========> 登陆成功！");
                break;
            case "btn_out":
                Debug.Log("===========> 退出成功！");
                break;
        }
    }

    public override void Show()
    {
        Debug.Log("========> Show");
    }

    public override void Hide()
    {
        Debug.Log("========> Hide");
    }
}
