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
        public UnityAction<int> OnSelectCoin = delegate { };
        public UnityAction<int> OnSelectStamp = delegate { };
        public UnityAction<int> OnSelectOre = delegate { };


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