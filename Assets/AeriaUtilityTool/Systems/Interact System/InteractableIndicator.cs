using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AeriaUtil.Systems.Interact
{
    public class InteractableIndicator : MonoBehaviour
    {
        [SerializeField]
        private GameObject _interactableUiElement;

        private void Update() => RenderUIElementUponCondition();
            
        private void RenderUIElementUponCondition()
        {
            if (_interactableUiElement == null)
            {
                Debug.LogError("You haven't inject any UI element for indicator.");
                return;
            }

            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject == gameObject)
                {
                    _interactableUiElement.SetActive(true);
                }
                else
                {
                    _interactableUiElement.SetActive(false);
                }
            }
        }
    }
}
