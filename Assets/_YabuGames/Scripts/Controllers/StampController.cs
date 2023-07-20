using System;
using _YabuGames.Scripts.Enums;
using _YabuGames.Scripts.Interfaces;
using _YabuGames.Scripts.Managers;
using _YabuGames.Scripts.Signals;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

namespace _YabuGames.Scripts.Controllers
{
    public class StampController : MonoBehaviour,ITool
    {
        [SerializeField] private Transform activePosition;
        [SerializeField] private float pushMultiplier;
        
        private bool _onAnimation;
        private bool _isSelected;
        private bool _isReset;
        private bool _isStamping;
        private float _stampingCooldown = .5f;
        private float _timer;
        private float _delayer = .25f;
        private bool _tutorialSeen;
        public AudioClip _clip;
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
            Stamp();
        }
        
        private void BeginHit()
        {
            if(_onAnimation || !_isSelected)
                return;
            
            if (Input.GetMouseButton(0))
            {
                _isReset = false;
                _isStamping = false;
                var currentPosition = transform.position;
                if (currentPosition.y <= .51f)
                {
                    Hit();
                    return;
                }

                currentPosition.y -= pushMultiplier * Time.deltaTime;
                transform.position = currentPosition;
            }

            if (Input.GetMouseButtonUp(0))
            {
                if(_isReset)
                    return;
                ResetStamp();
            }
        }

        private void Stamp()
        {
            if(!_isStamping)
                return;
            
            if (_timer <= 0)
            {
                _stampingCooldown -= _delayer;
                _delayer -= .5f;
                _delayer = Mathf.Clamp(_delayer, 0.03f, 1);

                if (_stampingCooldown <= 0)
                {
                    LevelSignals.Instance.OnChangeGameState?.Invoke(GameState.Cooling);
                    LevelSignals.Instance.OnToolChange?.Invoke(3);
                    return;
                }
                ToolSignals.Instance.StampHit?.Invoke();
                ShakeManager.Instance.ShakeCamera(true);
                AudioSource.PlayClipAtPoint(_clip,Camera.main.transform.position);
                HapticManager.Instance.PlayRigidHaptic();
                _timer += _stampingCooldown;
            }

            _timer -= Time.deltaTime;
            _timer = Mathf.Clamp(_timer, 0, _stampingCooldown);
        }
        private void Hit()
        {
           // transform.DOPunchRotation(Vector3.forward * 5, .5f, 10, 0f);
           _isStamping = true;
           
           if (_tutorialSeen)
               return;
           _tutorialSeen = true;
           ToolSignals.Instance.TutorialInput?.Invoke();


        }

        private void ResetStamp()
        {
            var desiredPosition = activePosition.position;

            _isReset = true;
            transform.DOMove(desiredPosition, .7f).SetEase(Ease.InSine)
                .OnComplete(() => _onAnimation = false);
        }
        
        public void Activate()
        {
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
            var desiredPosition = activePosition.position;

            _isReset = true;
            transform.DOMove(desiredPosition, .7f).SetEase(Ease.InSine)
                .OnComplete(GoToIdle);
            

            void GoToIdle()
            {
                _isSelected = false;
                transform.DOMove(_disabledPosition, 1).SetEase(Ease.OutSine).SetDelay(1);
                transform.DORotate(_disabledRotation, 1).SetEase(Ease.InSine).SetDelay(1);
            }
        }
    }
}