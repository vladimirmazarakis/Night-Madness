using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHumanCamera : MonoBehaviour
{
    [SerializeField] private float _playerMeshDissapearRadius = 5f;
    [SerializeField] private GameObject _playerMesh;
    [SerializeField] private Transform _playerMeshHighestPoint;
    [SerializeField] private Transform _playerMeshLowestPoint;
    [SerializeField] private Camera _camera;
    private void Update()
    {
        PlayerMeshRadiusCheck();
    }
    private void PlayerMeshRadiusCheck()
    {
        if(_camera.transform.position.y <= _playerMeshHighestPoint.position.y && _camera.transform.position.y >= _playerMeshLowestPoint.position.y)
        {
            Vector3 cameraPos = _camera.transform.position;
            cameraPos.y = 0;
            Vector3 playerMeshPos = _playerMesh.transform.position;
            playerMeshPos.y = 0;
            if (Vector3.Distance(playerMeshPos, cameraPos) <= _playerMeshDissapearRadius)
            {
                TogglePlayerMesh(false);
            }
            else
            {
                TogglePlayerMesh(true);
            }
        }
        else
        {
            TogglePlayerMesh(true);
        }
    }

    private void TogglePlayerMesh(bool IsActive)
    {
        _playerMesh.SetActive(IsActive);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(_playerMesh.transform.position, _playerMeshDissapearRadius);
    }
}
