using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : BaseManager<InputManager> {
    private bool isCheckInput = true;

    public InputManager(){
        MonoManager.GetInstance().AddUpdateListener(InputUpdate);
    }

    public void SetCheckInput(bool flag)
    {
        isCheckInput = flag;
    }

    private void InputUpdate()
    {
        if (!isCheckInput) return;
        checkInput(KeyCode.W);
        checkInput(KeyCode.S);
        checkInput(KeyCode.A);
        checkInput(KeyCode.D);
        checkInput(KeyCode.Space);
        checkMouse(0);//左键
        checkMouse(1);//右键
    }

    private void checkInput(KeyCode key)
    {
        if (Input.GetKeyDown(key))
        {
            EventManager.GetInstance().EventTrigger(EventManager.EVENT_KEY_DOWN, key);
        }
        if (Input.GetKeyUp(key)) 
        {
            EventManager.GetInstance().EventTrigger(EventManager.EVENT_KEY_UP, key);
        }
    }

    private void checkMouse(int key)
    {
        if (Input.GetMouseButtonDown(key))
        {
            EventManager.GetInstance().EventTrigger(EventManager.EVENT_MOUSE_DOWN, key);
        }
        if (Input.GetMouseButtonUp(key))
        {
            EventManager.GetInstance().EventTrigger(EventManager.EVENT_MOUSE_UP, key);
        }
    }
}
