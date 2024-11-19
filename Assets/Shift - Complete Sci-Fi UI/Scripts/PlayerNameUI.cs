using Game.Player;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Game.UI
{
    public interface IPlayerNameHolder { public void SetName(string playerName); };
    public class PlayerNameUI : MonoBehaviour, IPlayerNameHolder
    {
        [SerializeField] private TextMeshProUGUI _text;

        public void SetName(string playerName)
        {
            _text.text = playerName;
        }
    }

}
