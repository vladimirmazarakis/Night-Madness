using UnityEngine;
using Mirror;

public class HumanCamera : MonoBehaviour
{
    [SerializeField] private GameObject _freeLook;
    private NetworkIdentity _identity;
    private void Start()
    {
        _identity = this.gameObject.GetComponentInParent<NetworkIdentity>();
        if(_identity is null)
        {
            Debug.LogError("NetworkIdentity is null. Please assign a network identity to the player.");
            return;
        }
        Debug.Log(_identity.hasAuthority);
        if(!_identity.isLocalPlayer)
        {
            Debug.Log("Not local player. Disabling script.");
            this.gameObject.SetActive(false);
            _freeLook.SetActive(false);
            this.enabled = false;
            return;
        }
    }
}
