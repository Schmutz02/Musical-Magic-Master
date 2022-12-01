using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MMM
{
    public class PlayButton : MonoBehaviour
    {
        public Button Button;

        private void Start()
        {
            Button.onClick.AddListener(OnButtonClick);
        }

        private void OnButtonClick()
        {
            SceneTransition.Instance.TransitionToScene("Main");
        }
    }
}
