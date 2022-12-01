using System;
using MMM.DialogSystem;
using MMM.Music;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace MMM
{
    public class Encounter : MonoBehaviour
    {
        public string EncounterName;
        public AudioClip[] Instruments;
        public string SongName;
        public string KeyLayout;
        public int MissGoal;

        public Color NoteColor;

        public Dialog MeetDialog;
        public Dialog FailDialog;
        public Dialog PassDialog;
        public Dialog HintDialog;

        public Transform ControlList;
        public TMP_Text ControlTextPrefab;

        protected virtual void Start()
        {
            MeetDialog.Name = FailDialog.Name = PassDialog.Name = EncounterName;
        }

        public void OnSongEnd(bool pass)
        {
            if (!pass)
            {
                DialogManager.Instance.QueueDialog(FailDialog);
                OnSongFail();
                return;
            }
            
            EncounterManager.Instance.TransitionFromEncounter();
            DialogManager.Instance.QueueDialog(PassDialog);
            DialogManager.Instance.QueueDialog(HintDialog);
            OnSongPass();
        }

        protected virtual void OnSongPass()
        {
            if (HintDialog.Sentences.Length < 2)
                return;
            
            var text = Instantiate(ControlTextPrefab, ControlList);
            text.text = HintDialog.Sentences[1];
            text.gameObject.SetActive(true);
        }
        
        protected virtual void OnSongFail() { }

        private void OnTriggerEnter2D(Collider2D col)
        {
            GetComponent<Collider2D>().isTrigger = false;
            DialogManager.Instance.QueueDialog(MeetDialog);
        }
    }
}
