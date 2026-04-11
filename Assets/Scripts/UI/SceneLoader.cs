using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class SceneLoader : MonoBehaviour, IService
    {
        public event Action OnLoad;
        
        public void Init()
        {
            SceneManager.sceneLoaded += (scene, screen) =>  OnLoad?.Invoke();
        }
    }
}