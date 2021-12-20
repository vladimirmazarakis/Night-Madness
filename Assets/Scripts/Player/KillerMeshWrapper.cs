using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillerMeshWrapper : MonoBehaviour
{
    private KillerController _killerController;

    private void Start()
    {
        _killerController = GetComponentInParent<KillerController>();
    }

    /// <summary>
    /// Performs KillerController's Attack method.
    /// </summary>
    public void Attack()
    {
        Debug.Log("Started attacking...");
        _killerController.Attack();
    }
}
