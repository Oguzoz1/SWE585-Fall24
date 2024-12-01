using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.CameraControl
{
    using Cinemachine;
    public class CameraManager : MonoBehaviour
    {
        private static CameraManager _instance;
        public static CameraManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<CameraManager>();

                    if (_instance == null)
                    {
                        GameObject obj = new GameObject("CameraManager");
                        _instance = obj.AddComponent<CameraManager>();
                    }
                }
                return _instance;
            }
        }

        [Header("Camera")]
        public CinemachineFreeLook FreeLook;
        public CinemachineVirtualCamera MainVirtualCamera;

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else if (_instance != this)
            {
                Destroy(gameObject);
            }
        }
    }

}
