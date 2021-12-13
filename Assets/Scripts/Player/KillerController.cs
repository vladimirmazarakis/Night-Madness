using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class KillerController : MonoBehaviour
{
    [Header("Attack Settings")]
    [SerializeField] private LayerMask _attackRayLayerMask;
    [SerializeField] private Transform _attackRayOrigin;
    [SerializeField] private float _attackDelay = 1f;
    [SerializeField] private float _attackRadius = 0.5f;
    [SerializeField] private int _damage = 100;
    private InputMaster _inputMaster;
    private InputAction _attack;
    private bool _isAttacking = false;
    private bool _canAttack = true;
    #region Assignings.
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
    #endregion
    #region Attack
    private void Attack_performed(InputAction.CallbackContext obj)
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
    /// Performs the attack. (Ray check + health taking)
    /// </summary>
    public void Attack()
    {
        Vector3 origin = _attackRayOrigin.position;
        Vector3 direction = transform.forward;
        RaycastHit rayHit;
        Collider[] attackColliders = Physics.OverlapSphere(origin, _attackRadius, _attackRayLayerMask);
        if (attackColliders != null)
        {
            foreach(var ac in attackColliders)
            {
                if(ac.gameObject.tag == "Player")
                {
                    HumanController hitHuman;
                    bool isHumanControllerAvailable = ac.transform.gameObject.TryGetComponent<HumanController>(out hitHuman);
                    if (!isHumanControllerAvailable) return;
                    hitHuman.GiveDamage(_damage);
                    Debug.Log("Human was found, and damage was taken.");
                    break;
                }
            }
            return;
        }
        else
        {
            Debug.Log("Human was not found!");
            return;
        }
    }
    /// <summary>
    /// IsAttack property. (Only-get)
    /// </summary>
    public bool IsAttacking
    {
        get
        {
            return _isAttacking;
        }
    }
    #endregion

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_attackRayOrigin.position, _attackRadius);
    }
}
