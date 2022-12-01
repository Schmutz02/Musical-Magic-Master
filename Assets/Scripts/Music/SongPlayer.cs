using System;
using TMPro;
using UnityEngine;

namespace MMM.Music
{
    public class SongPlayer : MonoBehaviour
    {
        private const int StartOffsetMs = 5000;
        
        public static bool Playing;
        public static int SongTime;

        private static int _missedNotes;
        
        public Transform LaneParent;
        public GameObject LanePrefab;
        public TMP_Text MissText;

        public Action<bool> OnSongEnd;

        private Receptor[] _receptors;
        private int _songEndTime;

        private Song _song;
        private Color _noteColor;
        private int _missGoal;
        private int _ogMissGoal;

        public static int SongOffset;
        
        public void SetSong(Song song, Color noteColor, int missGoal)
        {
            _song = song;
            _noteColor = noteColor;
            _ogMissGoal = _missGoal = missGoal;
            _receptors = new Receptor[song.Lanes];
            for (var i = 0; i < song.Lanes; i++)
            {
                var lane = Instantiate(LanePrefab, LaneParent);
                _receptors[i] = lane.GetComponentInChildren<Receptor>();
                var key = lane.GetComponentInChildren<KeyController>();
                key.SetKey(song.KeyLayout[i].ToString());
                key.KeySound.clip = song.Instruments[i];
            }

            SongOffset = StartOffsetMs;
        }

        private void ResetSong()
        {
            Playing = false;
            SongTime = 0;
            _missedNotes = 0;

            
            _missGoal = 50;
            if (Settings.HardMode)
                _missGoal = _ogMissGoal;

            foreach (var receptor in _receptors)
            {
                receptor.ClearNotes();
            }
            
            foreach (var note in _song.Notes)
            {
                _receptors[note.Lane - 1].AddNote(note, _noteColor);
            }
        }

        public void PlaySong()
        {
            ResetSong();
            
            Playing = true;
            SongTime = -SongOffset;
            _songEndTime = _song.Notes[^1].Time + 1000;
        }

        public static void MissNote()
        {
            _missedNotes++;
        }

        private void Update()
        {
            MissText.text = $"Misses: {_missedNotes}/{_missGoal}";
            
            if (!Playing)
                return;

#if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.F12))
            {
                EndPlaying(true);
                return;
            }
            
            if (Input.GetKeyDown(KeyCode.F11))
            {
                EndPlaying(false);
                return;
            }
#endif
            SongTime += TimeConverter.DeltaTime;

            if (SongTime >= _songEndTime)
            {
                EndPlaying(_missedNotes <= _missGoal);
            }
        }

        private void EndPlaying(bool pass)
        {
            Playing = false;
            SongOffset = 3000;
            OnSongEnd?.Invoke(pass);
        }
    }
}