using System;
using MMM.DialogSystem;
using TMPro;
using UnityEngine;

namespace MMM
{
    public class StartDialog : MonoBehaviour
    {
        public TMP_FontAsset Font;
        
        public Dialog Dialog;
        
        private void Start()
        {
            DialogManager.Instance.SetFont(Font);
            DialogManager.Instance.QueueDialog(Dialog);
        }
    }
}