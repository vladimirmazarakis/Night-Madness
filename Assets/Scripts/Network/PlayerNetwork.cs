using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNetwork : MonoBehaviour
{
    [SerializeField] private PlayerType _playerType;
    private enum PlayerType
    {
        Killer,
        Human
    }
    void Start()
    {
        if(_playerType == PlayerType.Human)
        {
            var killer = GameObject.FindGameObjectWithTag("Killer");
            killer.GetComponent<KillerController>().DisableAllSelfComponents();
        }
        else if(_playerType == PlayerType.Killer)
        {
            var humen = GameObject.FindGameObjectsWithTag("Human");
            foreach(var human in humen)
            {
                human.GetComponent<HumanController>().DisableAllSelfComponents();
            }
        }   
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
