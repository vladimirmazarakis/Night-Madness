using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Interfaces
{
    /// <summary>
    /// Colliders' shared properties interface.
    /// </summary>
    public interface IColliderSettings
    {
        /// <summary>
        /// Defines if the Collider is trigger or not.
        /// </summary>
        public bool IsTrigger { get; set; }
        /// <summary>
        /// Defines the Collider's physic material.
        /// </summary>
        public PhysicMaterial PhysicMaterial { get; set; }
        /// <summary>
        /// The center of the collider.
        /// </summary>
        public Vector3 Center { get; set; }
    }
}
