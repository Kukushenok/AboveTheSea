using Player;
using UnityEngine;

namespace Item
{
    public class PlayerHoldingData
    {
        public HandRawData leftHand;
        public HandRawData rightHand;
        public Vector3 bodyPosition => body.position;
        public Vector3 cameraRotation => movement.CurrentCameraEulerAngles;
        public float bodyXRotation => movement.CurrentBodyXRotation;
        protected Transform body;
        protected PlayerMovement movement;
        public PlayerHoldingData(HandRawData leftHand, HandRawData rightHand, Transform body, PlayerMovement movement)
        {
            this.leftHand = leftHand;
            this.rightHand = rightHand;
            this.body = body;
            this.movement = movement;
        }
        protected PlayerHoldingData()
        {

        }
    }
    public class PlayerHoldingManager: PlayerHoldingData
    {
        public void UpdateByLerpCoeff(float lCoeff)
        {
            ((HandManager)leftHand)?.UpdateByLerpCoeff(lCoeff);
            ((HandManager)rightHand)?.UpdateByLerpCoeff(lCoeff);
        }
        public void Update(float dampCoeff)
        {
            UpdateByLerpCoeff(LerpFunctions.LerpTDeltaTime(dampCoeff));
        }
        public PlayerHoldingManager(HandManager leftHand, HandManager rightHand, Transform body, PlayerMovement movement)
        {
            this.leftHand = leftHand;
            this.rightHand = rightHand;
            this.body = body;
            this.movement = movement;
        }
    }
}