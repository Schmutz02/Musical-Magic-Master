using System;
using UnityEngine;
using UnityEngine.UI;

namespace MMM
{
    public class PauseMenuController : MonoBehaviour
    {
        public GameObject PauseMenu;
        public Toggle EasyModeToggle;

        private void Start()
        {
            EasyModeToggle.onValueChanged.AddListener(ToggleEasyMode);
        }

        private void ToggleEasyMode(bool isOn)
        {
            Settings.HardMode = isOn;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (Settings.Paused)
                    UnPause();
                else if (!Settings.Paused)
                    Pause();

                Settings.Paused = !Settings.Paused;
            }
        }

        private void Pause()
        {
            Time.timeScale = 0;
            PauseMenu.SetActive(true);
            EasyModeToggle.isOn = Settings.HardMode;
        }

        private void UnPause()
        {
            Time.timeScale = 1;
            PauseMenu.SetActive(false);
        }
    }
}
