using System;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class CollectorMovement : MonoBehaviour
{
    private NavMeshAgent _agent;

    private bool _isArrived;

    public event Action Arrived;

    private void Awake() => _agent = GetComponent<NavMeshAgent>();

    private void Update()
    {
        if (_agent.pathPending)
            return;

        if (_isArrived == false && _agent.remainingDistance <= _agent.stoppingDistance)
        {
            _isArrived = true;
            Arrived?.Invoke();
        }
    }

    public void SetDestination(Vector3 destination)
    {
        _agent.SetDestination(destination);
        _isArrived = false;
    }

    public void Stop() => _agent.isStopped = true;

    public void Resume() => _agent.isStopped = false;
}