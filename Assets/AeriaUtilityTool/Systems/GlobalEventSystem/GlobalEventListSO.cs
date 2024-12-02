using UnityEngine;


namespace AeriaUtil.Systems.GlobalEvent
{
    [CreateAssetMenu(fileName = "GlobalEventList",
    menuName = "ScriptableObjects/Global/GlobalEventList")]
    public class GlobalEventListSO : ScriptableObject
    {
        public GlobalEvent[] GlobalEvents;
    }

}
