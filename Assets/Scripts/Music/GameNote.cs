using System;
using UnityEngine;
using UnityEngine.UI;

namespace MMM.Music
{
    public class GameNote : MonoBehaviour
    {
        public int NoteSpeed;
        
        public Note Data;

        public void Init(Note data, Color color)
        {
            Data = data;
            GetComponent<Image>().color = color;

            var pos = ((RectTransform) transform).anchoredPosition;
            pos.y = NoteSpeed * (data.Time + SongPlayer.SongOffset);
            ((RectTransform)transform).anchoredPosition = pos;
        }

        private void Update()
        {
            if (!SongPlayer.Playing)
                return;
            
            var dist = -NoteSpeed * TimeConverter.DeltaTime;
            var pos = ((RectTransform) transform).anchoredPosition;
            pos.y += dist;
            ((RectTransform)transform).anchoredPosition = pos;
        }
    }
}