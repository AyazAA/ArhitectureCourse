﻿using System;
using System.Collections;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.CodeBase.Infrasstructure
{
    public class SceneLoader
    {
        private readonly ICoroutineRunner _coroutineRunner;

        public SceneLoader(ICoroutineRunner coroutineRunner) => 
            _coroutineRunner = coroutineRunner;

        public void Load(string name, Action onLoaded = null) =>
            _coroutineRunner.StartCoroutine(LoadScene(name, onLoaded));

        private IEnumerator LoadScene(string name, Action onLoaded = null)
        {
            if(SceneManager.GetActiveScene().name == name)
            {
                onLoaded?.Invoke();
                yield break;
            }

            AsyncOperation waitSceneLoad = SceneManager.LoadSceneAsync(name);
            while (!waitSceneLoad.isDone)
                yield return null;

            onLoaded?.Invoke();
        }
    }

}