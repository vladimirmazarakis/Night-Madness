using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Mirror;

public class PlayerNetwork : NetworkBehaviour
{
    public PlayerType playerType;

    private CustomNetworkManager _networkManager;
    private bool _hasCheckExecuted = false;
    public enum PlayerType
    {
        Killer,
        Human
    }
    void Start()
    {
        _networkManager = FindObjectOfType<CustomNetworkManager>();
    }
    void Update()
    {
        if (!_hasCheckExecuted)
        {
            Check();
        }
    }

    private void Check()
    {
        if (hasAuthority && _networkManager.AreAllPlayersLoaded)
        {
            if (playerType == PlayerType.Human)
            {
                var killer = FindObjectsOfType<PlayerNetwork>().FirstOrDefault(pType => pType.playerType == PlayerType.Killer);
                if(killer != null)
                {
                    Debug.Log($"Was killer found: {((killer != null) ? true : false)}");
                    killer.GetComponent<KillerController>().DisableAllSelfComponents();
                }
            }
            else if (playerType == PlayerType.Killer)
            {
                var humen = FindObjectsOfType<PlayerNetwork>().Where(pType => pType.playerType == PlayerType.Human);
                if (humen != null)
                {
                    Debug.Log($"Found humen on server: {humen.Count()}");
                    foreach (var human in humen)
                    {
                        human.GetComponent<HumanController>().DisableAllSelfComponents();
                    }
                } 
            }
            _hasCheckExecuted = true;
        }
    }
}
