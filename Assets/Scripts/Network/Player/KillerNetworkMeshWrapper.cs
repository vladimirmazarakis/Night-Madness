using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillerNetworkMeshWrapper : MonoBehaviour
{
    private KillerNetworkController _killerController;

    private void Start()
    {
        _killerController = GetComponentInParent<KillerNetworkController>();
    }

    /// <summary>
    /// Performs KillerController's Attack method.
    /// </summary>
    public void Attack()
    {
        _killerController.Attack();
    }
}
