using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System;

public class Movement : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private NavMeshAgent _agent;

    private float _waitTime = 2;

    public void Stop()
    {
        StopAllCoroutines();
        _rigidbody.velocity = Vector3.zero;
        _agent.ResetPath();
    }

    public void SetDestination(Vector3 destination)
    {
        StartCoroutine(WaitForStartMove(destination));
    }

    private IEnumerator WaitForStartMove(Vector3 destination)
    {
        yield return new WaitForSeconds(_waitTime);

        _agent.enabled = true;
        _agent.SetDestination(destination);
    }
}
