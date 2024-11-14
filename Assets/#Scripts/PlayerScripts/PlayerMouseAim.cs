using GameUtility;
using Player.CameraMovement;
using UnityEngine;
using UnityEngine.UI;


namespace Player
{
    public class PlayerMouseAim : MonoBehaviour, ICameraDependent
    {
        [SerializeField] private float rotationSpeed = 0.25f;
        [SerializeField] private float maxTiltAngle = 1.0f;
        [SerializeField] private float deadZoneRadius = 50.0f;

        [Header("UI Elements")]
        [SerializeField] private RectTransform deadZoneUI;
        [SerializeField] private RectTransform mouseDotUI;
        [SerializeField] private RectTransform aimingReticleUI;
        [SerializeField] private Canvas aimingCanvas;

        [Header("UI Colors")]
        [SerializeField] private Color deadZoneColor = new Color(1, 1, 1, 0.5f);
        [SerializeField] private Color mouseDotColor = new Color(0, 0, 1, 0.7f);
        [SerializeField] private Color aimingReticleColor = new Color(1, 0, 0, 0.7f);

        private Vector3 screenCenter;
        private Vector3 lastMousePosition;
        private Vector2 targetRotation;

        private void Start()
        {
            screenCenter = new Vector3(Screen.width / 2f, Screen.height / 2f, 0);
            Cursor.visible = false;

            deadZoneUI.position = screenCenter;
            deadZoneUI.sizeDelta = Vector2.one * deadZoneRadius * 2f;
            deadZoneUI.GetComponent<Image>().color = deadZoneColor;

            mouseDotUI.GetComponent<Image>().color = mouseDotColor;

            aimingReticleUI.GetComponent<Image>().color = aimingReticleColor;
            CursorUtility.CenterCursor();
            CursorUtility.ReleaseCursor();
        }

        private void FixedUpdate()
        {
            //If the manipulating is not functioning, return
            if (!isManipulating()) return;

            // Calculate mouse offset from the screen center.
            Vector3 mousePosition = Input.mousePosition;

            // Only proceed if the mouse has moved
            if (mousePosition == lastMousePosition && mousePosition == screenCenter)
            {
                transform.Rotate(0, 0, 0);
                return;
            }

            lastMousePosition = mousePosition;
            Vector3 offset = mousePosition - screenCenter;

            // Check if the mouse offset is outside the dead zone.
            float offsetMagnitude = offset.magnitude;

            // Only rotate if the offset is outside the dead zone.
            if (offsetMagnitude > deadZoneRadius)
            {
                // Adjust the offset to exclude the dead zone.
                Vector3 adjustedOffset = (offset - offset.normalized * deadZoneRadius) / (Screen.width / 2f);

                // Calculate rotation angles.
                float pitch = Mathf.Clamp(adjustedOffset.y * rotationSpeed, -maxTiltAngle, maxTiltAngle);
                float yaw = Mathf.Clamp(adjustedOffset.x * rotationSpeed, -maxTiltAngle, maxTiltAngle);

                targetRotation = new Vector2(pitch, yaw);
                transform.Rotate(-targetRotation.x, targetRotation.y, 0);
            }

            UpdateMouseDotPosition(mousePosition);
            UpdateAimingReticlePosition();
        }

        private void UpdateMouseDotPosition(Vector3 mousePosition)
        {
            // Convert mouse position to Canvas coordinates
            Vector2 uiMousePosition;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(aimingCanvas.transform as RectTransform, mousePosition, aimingCanvas.worldCamera, out uiMousePosition);

            // Update mouse dot UI position
            mouseDotUI.localPosition = uiMousePosition;
        }

        private void UpdateAimingReticlePosition()
        {
            // Determine the position in front of the vehicle in world space
            Vector3 aimingPosition = transform.position + transform.forward * 100f; 

            // Convert aiming position to screen coordinates
            Vector3 screenPoint = Camera.main.WorldToScreenPoint(aimingPosition);

            // Convert screen coordinates to canvas coordinates
            Vector2 uiPosition;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(aimingCanvas.transform as RectTransform, screenPoint, aimingCanvas.worldCamera, out uiPosition);

            // Update aiming reticle UI position
            aimingReticleUI.localPosition = uiPosition;
        }
        //Deactivate the aiming canvas
        public void StopManipulating()
        {
            aimingCanvas.enabled = false;
        }
        //Activate the aiming canvas
        public void ResumeManipulating()
        {
            aimingCanvas.enabled = true;
        }
        //manipulation depends on the canvas
        public bool isManipulating()
        {
            return aimingCanvas.enabled;
        }
    }
}
