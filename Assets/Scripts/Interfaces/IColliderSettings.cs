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
        public bool IsTrigger { get; set; }
        public PhysicMaterial PhysicMaterial { get; set; }
        public Vector3 Center { get; set; }
    }
}
