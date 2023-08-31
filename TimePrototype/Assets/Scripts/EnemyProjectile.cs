using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    private Rigidbody _rb;
    [SerializeField] private float _projectileSpeed;
    private Vector3 _oldVelocity = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.velocity = transform.forward * _projectileSpeed;

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StopTime()
    {
        _oldVelocity = _rb.velocity;
        _rb.velocity = Vector3.zero;
    }

    public void RestartTime()
    {
       _rb.velocity = _oldVelocity;
    }
}
