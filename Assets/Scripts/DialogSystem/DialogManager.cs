using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace MMM.DialogSystem
{
    public class DialogManager : MonoBehaviour
    {
        public static DialogManager Instance;

        public Animator DialogAnimator;
        public TMP_Text NameText;
        public TMP_Text SentenceText;
        public TMP_Text AdvanceText;

        [HideInInspector]
        public bool DialogActive;
    
        private Queue<string> _sentences;
        private Dialog _dialog;

        private string _currentSentence;
        private bool _animatingSentence;

        private Queue<Dialog> _dialogQueue;

        private float _dialogNextTime;

        private void Awake()
        {
            if (!CreateSingleton())
                return;

            _sentences = new Queue<string>();
            _dialogQueue = new Queue<Dialog>();
        }

        private void Update()
        {
            AdvanceText.enabled = Time.time >= _dialogNextTime;
        }

        private bool CreateSingleton()
        {
            if (Instance)
            {
                Destroy(gameObject);
                return false;   
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
            return true;
        }

        public void QueueDialog(Dialog dialog)
        {
            _dialogQueue.Enqueue(dialog);
            if (!DialogActive)
            {
                DialogAnimator.SetBool("FadeIn", true);
                PlayNextDialog();
            }
        }

        public void SetFont(TMP_FontAsset font)
        {
            NameText.font = font;
            SentenceText.font = font;
        }

        private void PlayNextDialog()
        {
            _sentences.Clear();
            _animatingSentence = false;
            _currentSentence = "";

            if (_dialogQueue.Count <= 0)
                return;

            var dialog = _dialogQueue.Dequeue();
            _dialog = dialog;
            DialogActive = true;

            if (NameText)
                NameText.text = dialog.Name;

            foreach (var sentence in dialog.Sentences)
            {
                _sentences.Enqueue(sentence);
            }
        
            ShowNextSentence();
        }

        public void ShowNextSentence()
        {
            if (Time.time < _dialogNextTime)
                return;
            
            StopAllCoroutines();

            if (!DialogActive)
                return;

            if (_animatingSentence)
            {
                SentenceText.text = _currentSentence;
                _animatingSentence = false;
                return;
            }
        
            if (_sentences.Count <= 0)
            {
                NameText.text = "";
                SentenceText.text = "";
                _dialog.OnDialogEnd.Invoke();
                
                if (_dialogQueue.Count > 0)
                {
                    PlayNextDialog();
                    return;
                }
                
                DialogActive = false;
                DialogAnimator.SetBool("FadeIn", false);
                return;
            }
        
            StartCoroutine(ShowNextSentenceRoutine());
            _dialogNextTime = Time.time + _dialog.DelayTime;
        }

        private IEnumerator ShowNextSentenceRoutine()
        {
            if (!SentenceText)
                yield break;

            _animatingSentence = true;
            SentenceText.text = "";
        
            _currentSentence = _sentences.Dequeue();
            var rich = "";
            var endRich = true;
            foreach (var letter in _currentSentence)
            {
                if (letter == '<')
                {
                    rich = "";
                    rich += letter;
                    endRich = false;
                }
                else if (letter == '>')
                {
                    rich += letter;
                    endRich = true;
                }
                else if (!endRich)
                {
                    rich += letter;
                }
                else if (endRich)
                {
                    SentenceText.text += rich + letter;
                    rich = "";
                    yield return new WaitForSeconds(1f / 40);
                }
            }

            _animatingSentence = false;
        }
    }
}