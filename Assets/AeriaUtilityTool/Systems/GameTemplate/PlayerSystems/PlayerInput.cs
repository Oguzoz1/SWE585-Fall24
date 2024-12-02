using AeriaUtil.Pattern;
using AYellowpaper.SerializedCollections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AeriaUtil.Systems
{
    public enum KeyInput
    {
        FORWARD,
        BACKWARD,
        LEFT,
        RIGHT,
        JUMP,
        RUN,
        INTERACT,
        CANCEL,
        DROP,
        ROTATELEFT,
        ROTATERIGHT
    }

    public class PlayerInput : Singleton<PlayerInput>
    {
        public PlayerInputSO InputData
        {
            get
            {
                if (_inputData == null)
                {
                    DebugExt.LogCheckObjExist(_inputData);
                }
                return _inputData;
            }
        }
        [SerializeField]private PlayerInputSO _inputData;
        public KeyCode GetKeycode(KeyInput inputID)
        {
            if (InputData.Keybinds.ContainsKey(inputID))
                return InputData.Keybinds[inputID];
            else
            {
                Debug.LogError($"{inputID} DOES NOT EXIST!");
                return KeyCode.None;
            }
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            _inputData = ScriptableObjectFinder.FindScriptableObject<PlayerInputSO>();
        }
#endif
    }
}
