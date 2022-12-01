using System;
using UnityEngine;
using UnityEngine.UI;

namespace MMM
{
    public class EasyModeToggle : MonoBehaviour
    {
        public Toggle Toggle;

        private void Start()
        {
            Toggle.onValueChanged.AddListener(OnToggleChanged);
            Toggle.isOn = Settings.HardMode;
        }

        private void OnToggleChanged(bool isOn)
        {
            Settings.HardMode = isOn;
        }
    }
}