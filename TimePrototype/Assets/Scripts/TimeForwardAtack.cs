using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeForwardAtack : MonoBehaviour
{
    private Camera _camera;
    [SerializeField] private LayerMask _enemyLayer;
    [SerializeField] private float _rangedAtackRange = 10.0f;

    // Start is called before the first frame update
    void Start()
    {
        _camera = GetComponentInChildren<Camera>();
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void ForwardAtack()
    {
        Ray ray = new Ray(_camera.transform.position, _camera.transform.forward);
        RaycastHit hitInfo;
        Physics.Raycast(ray, out hitInfo, _rangedAtackRange, _enemyLayer);

        if (hitInfo.collider == null)
            return;

        EnemyAI enemy = hitInfo.collider.gameObject.GetComponent<EnemyAI>();
        if (enemy != null)
            enemy.DealDamage(20);

    }
}
