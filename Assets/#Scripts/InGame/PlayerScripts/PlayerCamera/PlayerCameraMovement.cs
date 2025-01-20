using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using GameUtility;
using Cinemachine;
using Game.CameraControl;
using Mirror;

namespace Player.ShipCamera
{
    public class PlayerCameraMovement : MonoBehaviour
    {
        [Header("FreeLook Settings")]
        [SerializeField] private KeyCode _freeLookKey = KeyCode.LeftAlt;

        //All manipulators that is going to be stopped during freelook.
        private ICameraDependent[] _cameraDependents;
        private CinemachineFreeLook _freeLookCam;
        private CinemachineVirtualCamera _virtualCam;
        private NetworkIdentity networkIdentity;

        private bool _isResumed = false;
        private void Start() => Init();
        private void Update() => FreeLook();
        private void Init()
        {
            networkIdentity = GetComponent<NetworkIdentity>();

            if (!networkIdentity.isLocalPlayer) return;

            _cameraDependents = FindObjectsOfType<MonoBehaviour>()
                .OfType<ICameraDependent>()
                .ToArray();

            // Set Cameras for player
            _freeLookCam = CameraManager.Instance.FreeLook;
            _virtualCam = CameraManager.Instance.MainVirtualCamera;

            _freeLookCam.Follow = transform;
            _freeLookCam.LookAt = transform;
            _virtualCam.Follow = transform;
            _virtualCam.LookAt = transform;
        }

        private bool SetManipulators(bool shouldDisable)
        {
            if (shouldDisable && (_cameraDependents == null || _cameraDependents.Length == 0)) return true;

            if (!shouldDisable && (_cameraDependents == null || _cameraDependents.Length == 0)) return false;

            //When the button is pressed.
            if (shouldDisable)
            {
                if (!_cameraDependents[0].isManipulating()) return true;

                foreach(var dependent in _cameraDependents)
                {
                    dependent.StopManipulating();
                }
                _isResumed = false;
                CursorUtility.CenterCursor();
                return true;
            }
            //When the button is released
            else
            {
                //Check if the first dependent is false, then skip rest.
                if (_cameraDependents[0].isManipulating()) return false;
                if (!_isResumed) StartCoroutine(ResumeDependents());
                CursorUtility.ReleaseCursor();
                return false;
            }
        }

        private IEnumerator ResumeDependents()
        {
            _isResumed = true;
            yield return new WaitForSeconds(1.25f);
            if (_cameraDependents == null) yield return null;
            foreach (var dependent in _cameraDependents)
            {
                dependent.ResumeManipulating();
            }
        }


        //Responsible of looking around the space ship
        private void FreeLook()
        {
            if (!networkIdentity.isLocalPlayer) return;


            if (SetManipulators(Input.GetKey(_freeLookKey)))
            {
                _freeLookCam.Priority = 11;
            }
            else
            {
                _freeLookCam.Priority = 9;
            }
        }

    }

}
