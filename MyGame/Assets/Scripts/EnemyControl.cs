using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyControl : MonoBehaviour {
    private NavMeshAgent agent;
    private Transform player;

	// Use this for initialization
	void Awake () {
        agent = GetComponent<NavMeshAgent>();
	}

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update () {
        agent.SetDestination(player.position);
    }
}
