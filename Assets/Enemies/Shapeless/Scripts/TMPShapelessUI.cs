using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Enemies.Shapeless
{

    public class TMPShapelessUI : MonoBehaviour
    {
        public Toggle toggle;
        public Animator shapelessAnimator;
        public Transform trackingTransform;
        public void Awake()
        {
            toggle.onValueChanged.AddListener(OnToggleChanged);
        }
        public void OnToggleChanged(bool value)
        {
            shapelessAnimator.SetBool("angry", value);
            toggle.interactable = false;
            Invoke(nameof(MakeToggleInteractableAgain), 3);
        }

        public void MakeToggleInteractableAgain()
        {
            toggle.interactable = true;
        }
        public void Update()
        {
            if (Input.GetMouseButton(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit raycastHitInfo))
                {
                    trackingTransform.transform.position = raycastHitInfo.point + Vector3.up; 
                }
            }
            trackingTransform.LookAt(shapelessAnimator.transform.position);
        }
    }
}