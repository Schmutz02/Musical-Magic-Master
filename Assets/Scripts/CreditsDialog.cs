using System;
using System.Collections;
using MMM.DialogSystem;
using TMPro;
using UnityEngine;

namespace MMM
{
    public class CreditsDialog : MonoBehaviour
    {
        public TMP_FontAsset Font;
        
        public Dialog Dialog;

        private void Start()
        {
            DialogManager.Instance.SetFont(Font);
            
            StartCoroutine(EndDialog());
            
            Dialog.OnDialogEnd.AddListener(OnDialogEnd);
        }

        private IEnumerator EndDialog()
        {
            yield return new WaitForSeconds(1f);
            
            DialogManager.Instance.QueueDialog(Dialog);
        }

        private void OnDialogEnd()
        {
            SceneTransition.Instance.TransitionToScene("Title");
        }
    }
}