using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SceneItemState
{
    STATE_ACTIVE,
    STATE_INACTIVE
}

/// <summary>
/// 场景道具基类
/// </summary>
public class SceneItem : MonoBehaviour {
    public SceneItemState state = SceneItemState.STATE_ACTIVE;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public virtual void init()
    {
        state = SceneItemState.STATE_ACTIVE;
    }

    /// <summary>
    /// 触发
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            OnPlayerEnter(other.gameObject);
        }else
        {
            OnOtherEnter(other.gameObject);
        }
    }

    protected virtual void OnPlayerEnter(GameObject player)
    {

    }

    protected virtual void OnOtherEnter(GameObject other)
    {

    }
}
