using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    public GameObject target;
    public float smoothingSpeed;

	// Use this for initialization
	void Awake () {
        EventManager.GetInstance().AddEventListener(EventManager.EVENT_MAIN_PLAYER, OnMainPlayer);
	}

    private void OnMainPlayer()
    {
        target = GameObject.FindGameObjectWithTag("Player");
    }
	
	// Update is called once per frame
	void Update () {
        if (target) {
            Vector3 targetPos = target.transform.position + new Vector3(0, 6, -6);
            transform.position = Vector3.Lerp(transform.position, targetPos, smoothingSpeed * Time.deltaTime);
        }
    }
}
