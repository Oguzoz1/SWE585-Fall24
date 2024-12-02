using AeriaUtil.Systems.GameTemplate;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AeriaUtil.Systems.ObjectPlacement
{
    /// <summary>
    /// A service for rotating an object. It provides methods for the certain movements.
    /// </summary>
    public class ObjectRotator : IPlacementService
    {
        private void RotateObjectOnInputAtTarget(Vector3 rotationAxis, Transform go, float rotationSpeed)
        {
            float rotationAmount = rotationSpeed * Time.deltaTime;

            if (Input.GetKey(PlayerInput.Instance.GetKeycode(KeyInput.ROTATELEFT)))
            {
                RotateObject(go, rotationAxis, -rotationAmount);
            }
            else if (Input.GetKey(PlayerInput.Instance.GetKeycode(KeyInput.ROTATERIGHT)))
            {
                RotateObject(go, rotationAxis, rotationAmount);
            }
        }
        /// <summary>
        /// It rotates on input at the raycasted target.
        /// </summary>
        /// <param name="hit"></param>
        /// <param name="go"></param>
        /// <param name="rotationSpeed"></param>
        public void RotateObjectOnInputAtTarget(RaycastHit hit, Transform go, float rotationSpeed)
        {

            float rotationAmount = rotationSpeed * Time.deltaTime;

            if (Input.GetKey(PlayerInput.Instance.GetKeycode(KeyInput.ROTATELEFT)))
            {
                RotateWithQuaternionAndFaceNormal(hit, go, -rotationAmount);
            }
            else if (Input.GetKey(PlayerInput.Instance.GetKeycode(KeyInput.ROTATERIGHT)))
            {
                RotateWithQuaternionAndFaceNormal(hit, go, rotationAmount);
            }
            else RotateToSurfaceNormal(hit, go);
        }

        /// <summary>
        /// Rotate the transform at given hit location relative to facing direction of the surface.
        /// if hit normal is facing up or down, it will rotate around Y axis. Else, Z axis.
        /// </summary>
        /// <param name="hit"></param>
        /// <param name="go"></param>
        /// <param name="rotationSpeed"></param>
        public void RotateAtTargetRelativeToFacingDir(RaycastHit hit, Transform go, float rotationSpeed)
        {
            Vector3 rotationAxis = (hit.normal == Vector3.down || hit.normal == Vector3.up) ? go.transform.up : go.transform.forward;
            RotateObjectOnInputAtTarget(rotationAxis, go, rotationSpeed);
        }

        private void RotateObject(Transform go, Vector3 rotationAxis, float rotationAmount)
        {
            go.Rotate(rotationAxis, rotationAmount);
        }
        /// <summary>
        /// It rotates by multiplying look rotation of hitNormal and transform.up with angle of rotation axis.
        /// This provides simultaniously updating object to face outward while rotating around intended axis.
        /// </summary>
        /// <param name="hit"></param>
        /// <param name="go"></param>
        /// <param name="rotationAmount"></param>
        private void RotateWithQuaternionAndFaceNormal(RaycastHit hit, Transform go, float rotationAmount)
        {
            Vector3 hitNormal = -hit.normal;

            Quaternion faceNormalRotation = CalculateFaceNormalRotation(hitNormal, go);

            // Calculate the rotation around the object's forward axis
            Quaternion rotationDir = CalculateRotationDirection(hitNormal, rotationAmount, go);

            // Combine the two rotations
            Quaternion finalRotation = faceNormalRotation * rotationDir;

            // Apply the final rotation to the object
            go.rotation = finalRotation;
        }

        private void RotateWithQuaternion(RaycastHit hit, Transform go, float rotationAmount)
        {
            Vector3 hitNormal = -hit.normal;

            // Calculate the rotation around the object's forward axis
            Quaternion rotationDir = CalculateRotationDirection(hitNormal, rotationAmount, go);

            // Apply the final rotation to the object
            go.rotation *= rotationDir;
        }

        public void RotateToSurfaceNormal(RaycastHit hit, Transform go)
        {
            Vector3 hitNormal = -hit.normal;

            Quaternion faceNormalRotation = CalculateFaceNormalRotation(hitNormal, go);

            // Apply the final rotation to the object
            go.rotation = faceNormalRotation;
        }

        private bool IsRotationUpExempted(Vector3 hitNormal,Transform go)
        {
            if (-hitNormal == Vector3.up)
            {
                return true;
            }
            return false; 
        }

        private Quaternion CalculateFaceNormalRotation(Vector3 hitNormal, Transform go)
        {
            if (IsRotationUpExempted(hitNormal, go))
            {
                Debug.Log("A");
                return Quaternion.LookRotation(go.forward, go.up);
            }
            else
            {
                return Quaternion.LookRotation(hitNormal, go.up);
            }
        }

        private Quaternion CalculateRotationDirection(Vector3 hitNormal, float rotationAmount, Transform go)
        {
            if (IsRotationUpExempted(hitNormal,go))
            {
                return Quaternion.AngleAxis(rotationAmount, Vector3.up); 
            }
            else
            {
                return Quaternion.AngleAxis(rotationAmount, Vector3.forward);
            }
        }
    }
}