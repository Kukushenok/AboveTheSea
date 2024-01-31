using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerFootIKPlanter : MonoBehaviour
{
    [SerializeField] private MultiParentConstraint parentConstraint;
    [SerializeField] private Transform IKReference;
    [SerializeField] private TwoBoneIKConstraint twoBoneIKConstraint;
    [SerializeField] private Transform footTarget;
    [SerializeField] private float rayYOffset = 1;
    [SerializeField] private float rayDistance = 0.1f;
    [SerializeField] private float plantedYOffset = 0.1f;
    [SerializeField] private LayerMask mask;
    [SerializeField, Range(0, 1)] private float footIKDamp; 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        parentConstraint.weight = 1f;
        float targetWeight = 0f;
        if (Physics.Raycast(footTarget.position + Vector3.up * rayYOffset, Vector3.down, out RaycastHit hit, rayDistance, mask, QueryTriggerInteraction.Ignore))
        {
            Vector3 plantPos = hit.point + hit.normal * plantedYOffset;
            if (plantPos.y > footTarget.position.y)
            {
                IKReference.position = plantPos;// Vector3.Lerp(IKReference.position, plantPos, 1 - Mathf.Pow(footIKDamp, Time.deltaTime)); ;
                IKReference.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal) * footTarget.rotation;
                targetWeight = 1f;
            }
            Debug.DrawRay(plantPos, Vector3.down * rayDistance, Color.red);
        }
        twoBoneIKConstraint.weight = targetWeight;//Mathf.Lerp(twoBoneIKConstraint.weight, targetWeight, 1 - Mathf.Pow(footIKDamp, Time.deltaTime));
    }
}
