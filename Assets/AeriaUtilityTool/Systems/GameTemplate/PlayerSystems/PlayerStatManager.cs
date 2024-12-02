using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AeriaUtil.Systems.GameTemplate
{
    public class PlayerStatManager : StatManager
    {
#if UNITY_EDITOR

        private void OnValidate()
        {
            StatDataSO = ScriptableObjectFinder.FindScriptableObject<PlayerStatDataSO>();

        }
#endif
    }

}
