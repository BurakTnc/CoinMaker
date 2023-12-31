using System;
using _YabuGames.Scripts.Enums;
using _YabuGames.Scripts.Interfaces;
using _YabuGames.Scripts.Managers;
using _YabuGames.Scripts.Signals;
using DG.Tweening;
using UnityEngine;

namespace _YabuGames.Scripts.Controllers
{
    public class HammerController : MonoBehaviour,ITool
    {
        [SerializeField] private Transform activePosition;
        [SerializeField] private float rotationIncreaseValue;
        [SerializeField] private Transform liquid;
        [SerializeField] private float pushMultiplier;
        [SerializeField] private int desiredHitCount;
        [SerializeField] private GameObject hitParticle;
        [SerializeField] private Transform particlePosition;
        
        private bool _onAnimation;
        private bool _isSelected;
        private bool _isReset;
        private float _timer;
        private float _delayer;
        private int _hitCount;
        private bool _tutorialSeen;
        private bool _isActive;
        private Vector3 _disabledPosition;
        private Vector3 _disabledRotation;

        private void Start()
        {
            _disabledPosition = transform.position;
            _disabledRotation = transform.rotation.eulerAngles;
        }

        private void Update()
        {
            BeginHit();
        }
        
        private void BeginHit()
        {
            if(_onAnimation || !_isSelected || !_isActive)
                return;
            
            if (Input.GetMouseButton(0))
            {
                _isReset = false;
                var currentRotation = transform.localRotation.eulerAngles;
                if (currentRotation.z <= 270)
                {
                    Hit();
                    ToolSignals.Instance.HammerHit?.Invoke();
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
            _hitCount++;
            _onAnimation = true;
            transform.DOPunchRotation(Vector3.forward * 20, .4f, 10, 1f).OnComplete(ResetHammer);
            ShakeManager.Instance.ShakeCamera(false);
            HapticManager.Instance.PlayHeavyHaptic();
            Instantiate(hitParticle, particlePosition.position,Quaternion.identity);
            liquid.position += Vector3.down * pushMultiplier;

            if (!_tutorialSeen)
            {
                _tutorialSeen = true;
                ToolSignals.Instance.TutorialInput?.Invoke();
            }
            
            if (_hitCount < desiredHitCount) 
                return;
            _isActive = false;
            liquid.gameObject.SetActive(false);
           // LevelSignals.Instance.OnSelectStamp?.Invoke(3);//////-TEST-/////
            LevelSignals.Instance.OnChangeGameState?.Invoke(GameState.SelectingStamp);


        }

        private void ResetHammer()
        {
            var desiredRotation = activePosition.rotation.eulerAngles;

            _isReset = true;
            transform.DOLocalRotate(desiredRotation, .7f).SetEase(Ease.InSine)
                .OnComplete(() => _onAnimation = false);
        }
        
        public void Activate()
        {
            _isActive = true;
            var desiredRotation = activePosition.rotation.eulerAngles;

            _isSelected = true;
            _onAnimation = true;
            transform.DOMove(activePosition.position, 1).SetEase(Ease.OutSine).OnComplete(ReadyToUse).SetDelay(2);
            transform.DORotate(desiredRotation, 1).SetEase(Ease.InSine).SetDelay(2);

            void ReadyToUse()
            {
                _onAnimation = false;
            }
        }

        public void Disable()
        {
            _isActive = false;
            var desiredRotation = activePosition.rotation.eulerAngles;

            _isReset = true;
            transform.DORotate(desiredRotation, .7f).SetEase(Ease.InSine)
                .OnComplete(GoToIdlePosition);

            void GoToIdlePosition()
            {
                _isSelected = false;
                transform.DOMove(_disabledPosition, 1).SetEase(Ease.OutSine);
                transform.DORotate(_disabledRotation, 1).SetEase(Ease.InSine);
            }
        }
    }
}