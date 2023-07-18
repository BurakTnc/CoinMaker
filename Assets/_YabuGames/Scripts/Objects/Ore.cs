using System;
using DG.Tweening;
using UnityEngine;

namespace _YabuGames.Scripts.Objects
{
    public class Ore : MonoBehaviour
    {
        private bool _onMine;
        private float _vibrationCooldown = 1f;
        private float _timer;

        private void Update()
        {
            Mine();
        }

        public void OnMine(bool onMine)
        {
            _onMine = onMine;
        }

        private void Mine()
        {

            if (_timer <= 0 && _onMine)
            {
                _vibrationCooldown -= .2f;
                _timer += _vibrationCooldown;
                
                if (_vibrationCooldown <= 0)
                    return;
                
                transform.DOShakeRotation(_vibrationCooldown, Vector3.one * 5, 8, 90, true);
            }

            _timer -= Time.deltaTime;
            _timer = Mathf.Clamp(_timer, 0, _vibrationCooldown);
            _vibrationCooldown = Mathf.Clamp(_vibrationCooldown, 0f, 1);
        }
        
    }
}