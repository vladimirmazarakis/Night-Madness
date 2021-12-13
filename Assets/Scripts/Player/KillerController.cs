using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class KillerController : MonoBehaviour
{
    [SerializeField] private float _attackDelay = 1f;
    [SerializeField] private int _damage = 100;
    private InputMaster _inputMaster;
    private InputAction _attack;
    [SerializeField]private bool _isAttacking = false;
    [SerializeField] private bool _canAttack = true;

    private void Awake()
    {
        _inputMaster = new InputMaster();
    }
    private void OnEnable()
    {
        _attack = _inputMaster.Killer.Attack;
        _attack.performed += Attack_performed;
        _attack.Enable();
    }

    private void OnDisable()
    {
        _attack.Disable();
    }

    public void Attack_performed(InputAction.CallbackContext obj)
    {
        if(_canAttack)
        {
            StartCoroutine(AttackEnumerator());
        }
    }

    IEnumerator AttackEnumerator()
    {
        _isAttacking = true;
        _canAttack = false;
        yield return new WaitForSeconds(_attackDelay);
        _isAttacking = false;
        _canAttack = true;
    }

    /// <summary>
    /// 
    /// </summary>
    public bool IsAttacking 
    { 
        get 
        {
            return _isAttacking;
        } 
    }
}
