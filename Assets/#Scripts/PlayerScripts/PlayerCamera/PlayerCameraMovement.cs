using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using GameUtility;

namespace Player.CameraMovement
{
    public class PlayerCameraMovement : MonoBehaviour
    {
        [Header("FreeLook Settings")]
        [SerializeField] private Cinemachine.CinemachineFreeLook _freeLookCam;
        [SerializeField] private KeyCode _freeLookKey = KeyCode.LeftAlt;

        //All manipulators that is going to be stopped during freelook.
        private ICameraDependent[] _cameraDependents;

        private bool _isResumed = false;
        private void Start() => Init();
        private void Update() => FreeLook();
        private void Init()
        {
            _cameraDependents = FindObjectsOfType<MonoBehaviour>()
                .OfType<ICameraDependent>()
                .ToArray();
        }

        private bool SetManipulators(bool shouldDisable)
        {
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
            foreach (var dependent in _cameraDependents)
            {
                dependent.ResumeManipulating();
            }
        }


        //Responsible of looking around the space ship
        private void FreeLook()
        {
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
