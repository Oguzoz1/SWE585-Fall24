
using AYellowpaper.SerializedCollections;
using UnityEngine;

namespace AeriaUtil.Systems.GameTemplate
{
    public class StatDataSO : ScriptableObject
    {
        [SerializedDictionary("Stat Name", "Stat Value"), SerializeField]
        public SerializedDictionary<string, Stat> StatsDictionary = new SerializedDictionary<string, Stat>();
    }

}
