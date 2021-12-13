using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanController : MonoBehaviour
{
    private int _health = 100;
    private void Update()
    {
        HealthCheck();
    }
    private void HealthCheck()
    {
        if(_health <= 0)
        {
            Destroy(transform.parent.gameObject);
        }
    }

    /// <summary>
    /// Subtracts from the health the given amount.
    /// </summary>
    /// <param name="DamageAmount">The amount to subtract from the health.</param>
    public void GiveDamage(int DamageAmount)
    {
        _health -= DamageAmount;
    }
    /// <summary>
    /// User health (Only-get).
    /// </summary>
    public int Health 
    { 
        get 
        {
            return _health; 
        } 
    }
}
