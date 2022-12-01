using System;
using MMM.Music;
using UnityEngine;
using UnityEngine.Events;

namespace MMM
{
    public class EncounterManager : MonoBehaviour
    {
        public static EncounterManager Instance;

        public PlayerMovement PlayerMovement;
        public AudioSource BackgroundMusic;
        
        public Animator OverlayAnimator;
        public Animator UIAnimator;
        public Animator PPAnimator;

        public SongPlayer SongPlayer;

        private Encounter _encounter;

        private void Awake()
        {
            if (!CreateSingleton())
                return;
        }

        private bool CreateSingleton()
        {
            if (Instance)
            {
                Destroy(gameObject);
                return false;   
            }

            Instance = this;
            return true;
        }

        private static readonly int FadeIn = Animator.StringToHash("FadeIn");
        public void TransitionToEncounter(Encounter encounter)
        {
            _encounter = encounter;

            PlayerMovement.enabled = false;
            BackgroundMusic.Pause();

            OverlayAnimator.SetBool(FadeIn, true);
            UIAnimator.SetBool(FadeIn, true);
            PPAnimator.SetBool(FadeIn, true);
            
            var song = new Song(encounter.Instruments, encounter.SongName, encounter.KeyLayout);
            SongPlayer.SetSong(song, encounter.NoteColor, encounter.MissGoal);
            SongPlayer.PlaySong();
            SongPlayer.OnSongEnd += encounter.OnSongEnd;
        }

        public void TransitionFromEncounter()
        {
            OverlayAnimator.SetBool(FadeIn, false);
            UIAnimator.SetBool(FadeIn, false);
            PPAnimator.SetBool(FadeIn, false);

            SongPlayer.OnSongEnd -= _encounter.OnSongEnd;
            PlayerMovement.enabled = true;
            BackgroundMusic.Play();
            foreach (Transform child in SongPlayer.transform)
            {
                Destroy(child.gameObject);
            }
        }
    }
}