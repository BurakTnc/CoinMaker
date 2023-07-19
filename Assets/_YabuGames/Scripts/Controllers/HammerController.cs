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
        [SerializeField] private float pushMultiplier;
        
        private bool _onAnimation;
        private bool _isSelected;
        private bool _isReset;
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
                _isReset = false;
                var currentRotation = transform.rotation.eulerAngles;
                if (currentRotation.z <= 270)
                {
                    Hit();
                    return;
                }

                currentRotation.z -= rotationIncreaseValue * Time.deltaTime;
                transform.rotation= Quaternion.Euler(currentRotation);
            }

            if (Input.GetMouseButtonUp(0))
            {
                if(_isReset)
                    return;
                ResetHammer();
            }
        }
        private void Hit()
        {

            _onAnimation = true;
            transform.DOPunchRotation(Vector3.forward * 20, .4f, 10, 1f).OnComplete(ResetHammer);
            ShakeManager.Instance.ShakeCamera(false);
            liquid.position += Vector3.down * pushMultiplier;
            
        }

        private void ResetHammer()
        {
            var desiredRotation = activePosition.rotation.eulerAngles;

            _isReset = true;
            transform.DORotate(desiredRotation, .7f).SetEase(Ease.InSine)
                .OnComplete(() => _onAnimation = false);
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