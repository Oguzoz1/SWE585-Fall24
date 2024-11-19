using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    public class VehicleRotator : MonoBehaviour
    {
        public float rotationSpeed = 10f;  // Speed of rotation
        private Vector3 previousMousePosition;  // Store the previous mouse position
        private bool isRotating = false;  // Check if mouse button is held down

        void Update()
        {
            // Check if the left mouse button is held down
            if (Input.GetMouseButton(0)) // 0 is for left-click
            {
                // Hide the cursor and lock it to the center of the screen while rotating
                Cursor.visible = false;

                if (isRotating)
                {
                    // Get the current mouse position and calculate how much the mouse moved
                    Vector3 currentMousePosition = Input.mousePosition;
                    float mouseDeltaX = currentMousePosition.x - previousMousePosition.x;

                    // Rotate the vehicle around the Y axis (horizontal)
                    transform.Rotate(Vector3.up, mouseDeltaX * rotationSpeed * Time.deltaTime);

                    // Update the previous mouse position
                    previousMousePosition = currentMousePosition;
                }
            }
            else
            {
                // Show the cursor when the mouse button is released
                Cursor.visible = true;
            }

            // When the user clicks, set the origin point and start rotation
            if (Input.GetMouseButtonDown(0))
            {
                // Capture the initial mouse position when the user clicks
                previousMousePosition = Input.mousePosition;
                isRotating = true;
            }

            // When the user releases the mouse button, stop rotating
            if (Input.GetMouseButtonUp(0))
            {
                isRotating = false;
            }
        }
    }
}
