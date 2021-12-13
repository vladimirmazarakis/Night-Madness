using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKillerCamera : MonoBehaviour
{
    private Camera _camera;
    void Start()
    {
        _camera = Camera.main;
    }
    void Update()
    {
        UpdateCamera();
    }
    private void UpdateCamera()
    {
        float yawCamera = _camera.transform.rotation.eulerAngles.y;
        transform.rotation = Quaternion.Euler(0, yawCamera, 0);
    }
}
