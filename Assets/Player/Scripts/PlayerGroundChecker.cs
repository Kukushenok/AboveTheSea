using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Player
{
    public abstract class GroundChecker: MonoBehaviour
    {
        public abstract bool IsOnGround();
    }
    public class PlayerGroundChecker : GroundChecker
    {
        [SerializeField] private float radius;
        [SerializeField] private LayerMask layerMask;
        public override bool IsOnGround()
        {
            return Physics.CheckSphere(transform.position, radius, layerMask, QueryTriggerInteraction.Ignore);
        }
        public void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(transform.position, radius);
        }
    }
}