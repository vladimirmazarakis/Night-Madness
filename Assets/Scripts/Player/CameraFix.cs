using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFix : MonoBehaviour
{
    private void Start()
    {
        transform.parent = null;
    }
}
