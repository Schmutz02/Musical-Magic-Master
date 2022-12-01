using UnityEngine;

namespace MMM.DialogSystem
{
    public class DialogBox : MonoBehaviour
    {
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                DialogManager.Instance.ShowNextSentence();
            }
        }
    }
}