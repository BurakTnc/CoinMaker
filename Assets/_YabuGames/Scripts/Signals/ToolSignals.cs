using System;
using UnityEngine;
using UnityEngine.Events;

namespace _YabuGames.Scripts.Signals
{
    public class ToolSignals : MonoBehaviour
    {
        public static ToolSignals Instance;

        public UnityAction TutorialInput = delegate { };
        public UnityAction HammerHit = delegate { };
        public UnityAction StampHit = delegate { };
        public UnityAction CoolHit = delegate { };

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