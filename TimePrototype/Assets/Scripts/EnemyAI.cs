using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    enum State
    {
        Wander,
        Chase,
        Atack,
        Frozen
    }

    

    private State _currState = State.Wander;

    private NavMeshAgent _navMeshAgent;


    private Vector3 _wanderCenterPos;
    private Vector3 _wanderPoint;


    [SerializeField] private float _wanderRadius = 10.0f;
    [SerializeField] private float _wanderSpeed = 0.5f;

    /*[SerializeField] private*/ public float _fovRadius = 10.0f;
    /*[SerializeField] private*/ public float _fovAngle = 90.0f;
    [SerializeField] private LayerMask _obstructionMask;
    private bool _hasSpottedPlayer = false;
    [SerializeField] private float _loseInterestRadius = 12.0f;
    [SerializeField] private float _chaseSpeed = 5.0f;

    [SerializeField] private LayerMask _playerLayer;
    [SerializeField] private float _meleeAtackRange = 3.0f;
    [SerializeField] private float _meleeAtackDamage = 5.0f;
    [SerializeField] private Transform _atackCenter;

    private bool _isFrozen = false;

    [SerializeField] private bool _isRanged = false;
    [SerializeField] private float _rangedAtackRange;
    [SerializeField] private float _rangedAtackDamage;

    [SerializeField] private GameObject _enemyProjectilePrefab;
    [SerializeField] private Transform _projectileShootPos;

    // Start is called before the first frame update

    private GameObject _player;
    void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _wanderCenterPos = transform.position;
        _navMeshAgent.speed = _wanderSpeed;
        //_navMeshAgent.stoppingDistance = _atackRange - 0.5f;
        _player =  GameObject.FindGameObjectWithTag("Player");
        _wanderPoint = GetRandomWanderPoint();

    }

    // Update is called once per frame
    void Update()
    {
       



        if (_isFrozen)
            return;

        switch (_currState)
        {
            case State.Wander:
                Wander();
                break;
            case State.Chase:
                Chase();
                break;
            case State.Atack:
                Atack();
                break;
          

        }

    }


    void Wander()
    {

        if (Vector3.Distance(transform.position, _wanderPoint) < 2.0f)
        {
            _wanderPoint = GetRandomWanderPoint();
        }
        else
        {
            _navMeshAgent.SetDestination(_wanderPoint);
            Debug.Log("wander");
        }

        FieldOfViewCheck();
        if (_hasSpottedPlayer)
        {
            _hasSpottedPlayer = false;
            _navMeshAgent.speed = _chaseSpeed;
            _currState = State.Chase;

        }

        //TODO: Maybe add hearing radius

    }

    Vector3 GetRandomWanderPoint()
    {
        Vector3 randomPoint = _wanderCenterPos + (Random.insideUnitSphere * _wanderRadius);
        NavMeshHit navHit;
        NavMesh.SamplePosition(randomPoint, out navHit, _wanderRadius, -1);

        return new Vector3(navHit.position.x, transform.position.y, navHit.position.z);
    }

    private void FieldOfViewCheck()
    {
    


        Collider[] hitCheck = Physics.OverlapSphere(transform.position, _fovRadius, _playerLayer);
        //Check if player is in range
        if (hitCheck.Length != 0)
        {
            Transform target = hitCheck[0].transform;
            Vector3 directionToTarget = (target.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, directionToTarget) < _fovAngle / 2) //Check if player is in fov angle
            {
                float distanceToTarget = Vector3.Distance(transform.position, target.position);

                if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, _obstructionMask)) //Check if something obstructs the vision
                {
                    _hasSpottedPlayer = true;
                    
                }
                else
                {
                    _hasSpottedPlayer = false;
                }
            }
            else
            {
                _hasSpottedPlayer = false;
            }
        }
      
    }

    void Chase()
    {

        float distToPlayer = Vector3.Distance(transform.position, _player.transform.position);

        if (_isRanged)
        {
            if (distToPlayer < (_rangedAtackRange - 3.0f)) // in atack range
            {
                //rotate to face player and set state to atack

                _navMeshAgent.SetDestination(transform.position);
                _currState = State.Atack;
            }
            else if (distToPlayer > _loseInterestRadius) //Player out of interest range
            {
                _wanderCenterPos = transform.position;
                _navMeshAgent.speed = _wanderSpeed;
                _currState = State.Wander;
            }
            else //Move towards player
            {
                NavMeshHit navHit;
                NavMesh.SamplePosition(_player.transform.position, out navHit, _wanderRadius, -1);
                _navMeshAgent.SetDestination(new Vector3(navHit.position.x, transform.position.y, navHit.position.z));

            }
        }
        else
        {

            if (distToPlayer < (_meleeAtackRange - 1.0f)) // in atack range
            {
                //rotate to face player and set state to atack

                _navMeshAgent.SetDestination(transform.position);
                _currState = State.Atack;
            }
            else if (distToPlayer > _loseInterestRadius) //Player out of interest range
            {
                _wanderCenterPos = transform.position;
                _navMeshAgent.speed = _wanderSpeed;
                _currState = State.Wander;
            }
            else //Move towards player
            {
                NavMeshHit navHit;
                NavMesh.SamplePosition(_player.transform.position, out navHit, _wanderRadius, -1);
                _navMeshAgent.SetDestination(new Vector3(navHit.position.x, transform.position.y, navHit.position.z));

            }
        }
    }

    void Atack()
    {
        float distToPlayer = Vector3.Distance(transform.position, _player.transform.position);

        if (_isRanged)
        {
            if (distToPlayer > _rangedAtackRange) //if out of atack range -> chase player again
            {
                _currState = State.Chase;
            }
            else   //TODO: add cooldown depending on animation
            {
                Vector3 direction = _player.transform.position - transform.position;
                Vector3 normalizedDirection = direction.normalized;

                transform.LookAt(_player.transform);

                GameObject projectile = GameObject.Instantiate(_enemyProjectilePrefab, _projectileShootPos.position, _projectileShootPos.rotation);
                projectile.transform.parent = null;

            }
        }
        else
        { 


            if (distToPlayer > _meleeAtackRange) //if out of atack range -> chase player again
            {
                _currState = State.Chase;
            }
            else   //TODO: add cooldown depending on animation
            {
                Vector3 direction = _player.transform.position - transform.position;
                Vector3 normalizedDirection = direction.normalized;


                transform.LookAt(_player.transform);

                Debug.Log("Enemy atack");

                Ray ray = new Ray(_atackCenter.position, _atackCenter.forward);
                RaycastHit hitInfo;
                Physics.Raycast(ray, out hitInfo, _meleeAtackRange, _playerLayer);

                if (hitInfo.collider == null)
                    return;

                //Deal dmg
                Debug.Log("Player hit");
            }


        }
    }


    public void StopTime()
    {
        _isFrozen = true;
        _navMeshAgent.SetDestination(transform.position);

    }

    public void RestartTime()
    {
        _isFrozen = false;
    }

   

}
