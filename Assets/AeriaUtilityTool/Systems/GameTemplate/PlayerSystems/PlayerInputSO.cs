using AYellowpaper.SerializedCollections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AeriaUtil.Systems
{
    [CreateAssetMenu(fileName = "PlayerInput",
menuName = "ScriptableObjects/Input/PlayerInput")]
    public class PlayerInputSO : ScriptableObject
    {
        public SerializedDictionary<KeyInput, KeyCode> Keybinds;
    }
}
