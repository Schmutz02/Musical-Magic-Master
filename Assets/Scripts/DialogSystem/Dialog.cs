using System;
using UnityEngine;
using UnityEngine.Events;

namespace MMM.DialogSystem
{
    [Serializable]
    public class Dialog
    {
        public string Name;
        
        [TextArea(minLines: 3, maxLines: 10)]
        public string[] Sentences;

        public float DelayTime;

        public UnityEvent OnDialogEnd;
    }
}