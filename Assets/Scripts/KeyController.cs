using System;
using System.Collections;
using System.Collections.Generic;
using MMM.DialogSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MMM
{
    public class KeyController : MonoBehaviour
    {
        public Image KeyImage;
        public TMP_Text KeyText;
        public AudioSource KeySound;

        private Color _keyUpColor;
        public Color KeyDownColor;

        [HideInInspector]
        public string Key;

        private void Start()
        {
            _keyUpColor = KeyImage.color;
        }

        public void SetKey(string key)
        {
            KeyText.text = key;
            Key = key;
        }

        private void Update()
        {
            if (DialogManager.Instance.DialogActive || Settings.Paused)
                return;
            
            if (Input.GetKeyDown(Key))
            {
                KeySound.PlayOneShot(KeySound.clip);
                KeyImage.color = KeyDownColor;
            }

            if (Input.GetKeyUp(Key))
            {
                KeyImage.color = _keyUpColor;
            }
        }
    }
}
