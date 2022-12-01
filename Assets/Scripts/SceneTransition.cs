using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MMM
{
    public class SceneTransition : MonoBehaviour
    {
        public static SceneTransition Instance;

        public Animator Animator;

        private void Awake()
        {
            if (Instance)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
            Animator.speed = 0.5f;
        }

        public void TransitionToScene(string sceneName)
        {
            StartCoroutine(Transition(sceneName));
        }

        private IEnumerator Transition(string sceneName)
        {
            Animator.SetBool("FadeIn", true);
            
            yield return new WaitForSeconds(0.5f);
            
            var sceneLoad = SceneManager.LoadSceneAsync(sceneName);

            yield return new WaitUntil(() => sceneLoad.isDone);
            
            Animator.SetBool("FadeIn", false);
        }
    }
}