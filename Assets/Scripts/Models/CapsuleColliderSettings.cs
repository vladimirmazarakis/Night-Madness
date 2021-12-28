using Assets.Scripts.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Models
{
    /// <summary>
    /// Capsule Collider settings encapsulation.
    /// </summary>
    public class CapsuleColliderSettings : MonoBehaviour,IColliderSettings
    {
        [SerializeField] private bool _isTrigger;
        [SerializeField] private PhysicMaterial _physicMaterial;
        [SerializeField] private Vector3 _center;
        [SerializeField] private float _radius;
        [SerializeField] private float _height;
        [SerializeField] private CapsuleDirection _direction;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public CapsuleColliderSettings(
            bool isTrigger,
            PhysicMaterial physicMaterial,
            Vector3 center,
            float radius,
            float height,
            CapsuleDirection direction)
        {
            _isTrigger = isTrigger;
            _physicMaterial = physicMaterial;
            _center = center;
            _radius = radius;
            _height = height;
            _direction = direction;
        }

        #region Properties.
        public float Radius 
        {
            get 
            {
                return _radius;
            }
            set
            {
                _radius = value;
            }
        }
        public float Height 
        { 
            get 
            {
                return _height;
            }
            set
            {
                _height = value;
            }
        }
        public CapsuleDirection Direction
        {
            get 
            {
                return _direction;
            }
            set 
            {
                _direction = value;
            }
        }
        #endregion

        #region Interface Properties.
        public bool IsTrigger 
        { 
            get 
            {
                return _isTrigger;
            }
            set 
            {
                _isTrigger = value;
            } 
        }
        public PhysicMaterial PhysicMaterial 
        { 
            get 
            {
                return _physicMaterial;
            }
            set
            {
                _physicMaterial = value;
            }
        }
        public Vector3 Center
        {
            get 
            {
                return _center;
            }
            set 
            {
                _center = value;
            }
        }
        #endregion
    }
    /// <summary>
    /// Enum responsible for the direction of the capsule.
    /// </summary>
    public enum CapsuleDirection
    {
        X = 0,
        Y = 1,
        Z = 2
    }
}
