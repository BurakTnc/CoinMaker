using System;
using _YabuGames.Scripts.Enums;
using UnityEngine;
using UnityEngine.Events;

namespace _YabuGames.Scripts.Signals
{
    public class LevelSignals : MonoBehaviour
    {
        public static LevelSignals Instance;

        public UnityAction<GameState> OnChangeGameState = delegate { };
        public UnityAction<int> OnToolChange = delegate { };

        private void Awake()
        {
            if (Instance != this && Instance != null) 
            {
                Destroy(this);
                return;
            }

            Instance = this;
        }
    }
}