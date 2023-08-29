using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    private NavMeshAgent _navMeshAgent;

    private bool _hasSpottedPlayer = false;
    private Vector3 _wanderCenterPos;
    private Vector3 _wanderPoint;
    [SerializeField] private float _wanderRadius = 10.0f;
    [SerializeField] private float _wanderSpeed = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _wanderCenterPos = transform.position;
        _navMeshAgent.speed = _wanderSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if (_hasSpottedPlayer)
        {

        }
        else
        {
            Wander();
        }

       
    }


    void Wander()
    {

        if (Vector3.Distance(transform.position, _wanderPoint) < 2.0f)
            _wanderPoint = GetRandomWanderPoint();
        else
        {
            _navMeshAgent.SetDestination(_wanderPoint);
            Debug.Log("wander");
        }

    }

    Vector3 GetRandomWanderPoint()
    {
        Vector3 randomPoint = _wanderCenterPos + (Random.insideUnitSphere * _wanderRadius);
        NavMeshHit navHit;
        NavMesh.SamplePosition(randomPoint, out navHit, _wanderRadius, -1);

        return new Vector3(navHit.position.x, transform.position.y, navHit.position.z);
    }
}
