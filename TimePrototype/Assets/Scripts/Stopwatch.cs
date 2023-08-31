using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Stopwatch : MonoBehaviour
{
    [SerializeField] private float _stopwatchDuration = 5;
    [SerializeField] private LayerMask _enemyLayer;
    [SerializeField] private LayerMask _projectileLayer;

    private List<EnemyAI> _enemies = new List<EnemyAI>();

    // Start is called before the first frame update
    void Start()
    {
        //Invoke the restarttime after x sec
        Invoke("RestartTime", _stopwatchDuration);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
     
       // Debug.Log(other.gameObject.layer);


        //Get enemies
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            Debug.Log(other);
            EnemyAI enemy = other.GetComponent<EnemyAI>();
            if (enemy == null)
                return;

            _enemies.Add(enemy);
    
            enemy.StopTime();


        }
        else if (other.gameObject.layer == _projectileLayer)
        {

            //get projectile comp
            //proj script should have something that saves the dir en when it ends should be put back
        }

        //Get projectiles

    }

    void RestartTime()
    {
        foreach(EnemyAI enemy in _enemies)
        {
            enemy.RestartTime();
        }

        Destroy(this.gameObject);
    }

}
