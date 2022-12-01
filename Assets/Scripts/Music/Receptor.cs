using System;
using System.Collections.Generic;
using UnityEngine;

namespace MMM.Music
{
    public class Receptor : MonoBehaviour
    {
        public GameNote NotePrefab;
        public KeyController Key;
        public ParticleSystem HitEffect;

        private Queue<GameNote> _notes;

        private const int HIT_WINDOW = 150;
        private const int S_POOR = 250;

        private void Awake()
        {
            _notes = new Queue<GameNote>();
        }

        public void AddNote(Note note, Color noteColor)
        {
            var gameNote = Instantiate(NotePrefab, transform);
            gameNote.Init(note, noteColor);
            
            _notes.Enqueue(gameNote);
        }

        public void ClearNotes()
        {
            while(_notes.Count > 0)
                RemoveNote(0);
        }

        private void Update()
        {
            if (!SongPlayer.Playing || _notes.Count <= 0 || Settings.Paused)
                return;

            if (Input.GetKeyDown(Key.Key))
            {
                TryHitNote();
            }
            
            CheckForMissedNote();
        }

        private void CheckForMissedNote()
        {
            if (!_notes.TryPeek(out var note))
                return;
            
            var timeDiff = SongPlayer.SongTime - note.Data.Time;

            if (timeDiff > HIT_WINDOW)
            {
                RemoveNote(timeDiff);
                SongPlayer.MissNote();
            }
        }

        private void TryHitNote()
        {
            if (!_notes.TryPeek(out var note))
                return;
            
            var timeDiff = SongPlayer.SongTime - note.Data.Time;
            var timeDiffAbs = Math.Abs(timeDiff);

            if (timeDiffAbs <= HIT_WINDOW)
            {
                RemoveNote(timeDiff);
                HitEffect.Play();
                return;
            }

            if (timeDiffAbs > S_POOR)
            {
                return;
            }

            RemoveNote(timeDiff);
            SongPlayer.MissNote();
        }

        private void RemoveNote(int timeDiff)
        {
            var note = _notes.Dequeue();
            Destroy(note.gameObject);
        }
    }
}