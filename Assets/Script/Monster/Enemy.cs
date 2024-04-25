using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    NavMeshAgent navMeshAgent;
    public Transform target;

    public void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }
    public void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;

        Observable.EveryUpdate()
            .Subscribe(_ =>
            {
                if (target != null)
                {
                    navMeshAgent.SetDestination(target.position);
                }
            })
            .AddTo(this);
    }
    public void Update()
    {
        
    }
    
}
