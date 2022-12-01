using System;
using System.Collections.Generic;
using UnityEngine;

namespace MMM.Music
{
    public class Song
    {
        public int BPM;
        public int Lanes;
        public int TimeSig;
        public AudioClip[] Instruments;
        public Note[] Notes;
        public string KeyLayout;

        private int MSPB => (int)(1f / BPM * 60 * 1000);

        public Song(AudioClip[] instruments, string songName, string keyLayout)
        {
            Instruments = instruments;
            KeyLayout = keyLayout;
            var fileText = Resources.Load<TextAsset>($"Songs/{songName}").text;
            var notes = new List<Note>();
            var lines = fileText.Split("\r\n");
            BPM = int.Parse(lines[0]);
            Lanes = int.Parse(lines[1]);
            TimeSig = int.Parse(lines[2]);
            for (var i = 3; i < lines.Length; i++)
            {
                var noteMS = (int) (((float) (i - 3) / TimeSig) * MSPB);
                foreach (var lane in lines[i])
                {
                    if (lane == '0')
                        break;

                    var note = new Note(noteMS, int.Parse(lane.ToString()));
                    notes.Add(note);
                }
            }

            Notes = notes.ToArray();
        }
    }
}