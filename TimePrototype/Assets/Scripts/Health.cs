using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    

     private float _maxHealth = 3.0f;
    private float _currHealth;

    private Slider _healthBar;

    [SerializeField] private float _depletionSpeed = 0.3f;



    // Start is called before the first frame update
    void Start()
    {
        _healthBar = GameObject.FindGameObjectWithTag("HealthBar").GetComponent<Slider>();

        _currHealth = _maxHealth;
        _healthBar.maxValue = _maxHealth;
        _healthBar.minValue = 0;
        _healthBar.value = _healthBar.maxValue;
       
    }


    // Update is called once per frame
    void Update()
    {
        _currHealth -= Time.deltaTime * _depletionSpeed;

        if(_currHealth < 0)
        {
            //LOSE
        }

        _healthBar.value = _currHealth;
    }


    void Heal(float health)
    {
        _currHealth += health;
    }

    void TakeDamage(float damage)
    {
        _currHealth -= damage;
    }

}
