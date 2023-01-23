using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

namespace SceneChanger{
    public class FadeSceneChanger : SingletonMonoBehaviour<FadeSceneChanger>{

        [Tooltip("CanvasGroup")]
        private CanvasGroup fadeCanvasGroup = null;

        [Tooltip("PrefabLoadObject")]
        private GameObject prefabObject = null;

        private static float expectedFadeTime = 0f;

        public static float expectedFadetime{
            get{
                return expectedFadeTime;
            }
        }

        private Sequence sequence;

        private void Awake() {
            GameObject prefab = (GameObject)Resources.Load("Prefabs/FadeCanvas");
            prefabObject = Instantiate(prefab);

            fadeCanvasGroup = prefabObject.GetComponent<CanvasGroup>();

            DontDestroyOnLoad(fadeCanvasGroup);
            DontDestroyOnLoad(fadeCanvasGroup.GetComponentInChildren<Image>());

            SceneManager.activeSceneChanged += OnActiveSceneChanged;
        }

        /// <summary>
        /// Function to transition the scene while fading in and out.
        /// </summary>
        /// <param name="sceneName">Name of next scene.</param>
        /// <param name="fadeIoTime">Time to fade.</param>
        /// <param name="fadeWaitTime">Additional time to wait in the dark after completion of scene transition.</param>
        public void ChangeSceneWithFade(string sceneName, float fadeIoTime, float fadeWaitTime){
            prefabObject.SetActive(true);

            // Start loading the next scene
            AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
            // Block scene transitions
            operation.allowSceneActivation = false;

            expectedFadeTime = fadeIoTime + fadeWaitTime;

            sequence = DOTween.Sequence();

            // Fade animation processing
            sequence
                .Append(fadeCanvasGroup.DOFade(1f, fadeIoTime)) // Fade out
                .AppendCallback(() => {
                    sequence.Pause(); // Stop until the scene changes
                    operation.allowSceneActivation = true; // Allow scene changes
                })
                .AppendInterval(fadeWaitTime) // Stops during darkening
                .Append(fadeCanvasGroup.DOFade(0f, fadeIoTime)); // Fade in
                

            sequence.Play().OnComplete(() => prefabObject.SetActive(false));  // TODO:FIX
        }

        private void OnActiveSceneChanged(Scene preScene, Scene nextScene){
            sequence.Play();
        }
    }
}

