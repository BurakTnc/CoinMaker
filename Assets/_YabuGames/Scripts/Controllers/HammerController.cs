using System;
using _YabuGames.Scripts.Interfaces;
using _YabuGames.Scripts.Managers;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

namespace _YabuGames.Scripts.Controllers
{
    public class HammerController : MonoBehaviour,ITool
    {
        [SerializeField] private Transform activePosition, disabledPosition;
        [SerializeField] private float rotationIncreaseValue;
        [SerializeField] private Transform liquid;
        private bool _onAnimation;
        private bool _isSelected;
        private float _timer;
        private float _delayer;


        private void Update()
        {
            BeginHit();
        }

        private void BeginHit()
        {
            if(_onAnimation || !_isSelected)
                return;
            
            if (Input.GetMouseButton(0))
            {
                var currentRotation = transform.rotation.eulerAngles;
                if (currentRotation.z <= 270)
                {
                    Hit();
                    return;
                }

                currentRotation.z -= rotationIncreaseValue * Time.deltaTime;
                transform.rotation= Quaternion.Euler(currentRotation);
            }
        }
        private void Hit()
        {
            _onAnimation = true;
            var desiredRotation = transform.rotation.eulerAngles;
            desiredRotation.z = -5;
            transform.DOPunchRotation(Vector3.forward * 20, .4f, 10, 1f).OnComplete(Reset);
            ShakeManager.Instance.ShakeCamera(false);

            void Reset()
            {
                transform.DORotate(desiredRotation, 1).SetEase(Ease.InSine)
                    .OnComplete(() => _onAnimation = false);
            }
        }
        
        public void Activate()
        {
            var desiredRotation = activePosition.rotation.eulerAngles;

            _isSelected = true;
            _onAnimation = true;
            transform.DOMove(activePosition.position, 1).SetEase(Ease.OutSine).OnComplete(ReadyToUse).SetDelay(1);
            transform.DORotate(desiredRotation, 1).SetEase(Ease.InSine).SetDelay(1);

            void ReadyToUse()
            {
                _onAnimation = false;
            }
        }

        public void Disable()
        {
            var desiredRotation = disabledPosition.rotation.eulerAngles;

            _isSelected = false;
            transform.DOMove(disabledPosition.position, 1).SetEase(Ease.OutSine);
            transform.DORotate(desiredRotation, 1).SetEase(Ease.InSine);
        }
    }
}