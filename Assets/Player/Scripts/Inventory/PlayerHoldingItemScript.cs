using Item;
using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace Player
{
    [RequireComponent(typeof(PlayerMovement))]
    public class PlayerHoldingItemScript : MonoBehaviour
    {
        private PlayerMovement playerMovement;
        [Header("Правая рука")]
        [SerializeField] private TwoBoneIKConstraint rightHandConstraint;
        [SerializeField] private Transform rightHandTip;
        [SerializeField] private Transform rightHand;
        [Header("Левая рука")]
        [SerializeField] private TwoBoneIKConstraint leftHandConstraint;
        [SerializeField] private Transform leftHandTip;
        [SerializeField] private Transform leftHand;

        [SerializeField] private Transform body;
        [SerializeField] private Vector3 delta;
        private PlayerHoldingManager holdingManager;
        [SerializeField] private ItemBehaviour currentHoldingItem;
        private void Awake()
        {
            playerMovement = GetComponent<PlayerMovement>();
            holdingManager = new PlayerHoldingManager
                (
                new HandManager(rightHandTip, rightHand, rightHandConstraint),
                new HandManager(leftHandTip, leftHand, leftHandConstraint),
                body, playerMovement
                );
        }

        public void Update()
        {
            if(currentHoldingItem)
            {
                currentHoldingItem.UpdateHoldingPos(holdingManager);
            }
            holdingManager.Update(0.05f);
            //Vector3 forward = delta;
            //Vector3 qrot = playerMovement.CurrentCameraEulerAngles;
            //qrot.x /= 1.2f;
            //qrot.y = Mathf.DeltaAngle(playerMovement.CurrentBodyXRotation, qrot.y) / 1.5f + playerMovement.CurrentBodyXRotation;
            //Quaternion rot = Quaternion.Euler(qrot);
            //rightHandTip.position = body.position + rot * forward;
            //rightHandTip.rotation = rot;
            //float weight = rightHandConstraint.weight;
            //LerpFunctions.DampByDeltaTime(ref weight, playerMovement.CurrentCameraEulerAngles.x > 80 ? 0 : 1, 0.01f);
            //rightHandConstraint.weight = weight;

        }
        public void LateUpdate()
        {
            if (currentHoldingItem)
            {
                currentHoldingItem.AttractItemToHands(holdingManager);
            }
        }
    }
}