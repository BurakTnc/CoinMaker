using System;
using _YabuGames.Scripts.Enums;
using _YabuGames.Scripts.Interfaces;
using _YabuGames.Scripts.Signals;
using DG.Tweening;
using UnityEngine;

namespace _YabuGames.Scripts.Controllers
{
    public class WaterController : MonoBehaviour,ITool
    {
        [SerializeField] private Transform activePosition, disabledPosition;

        private GameObject _coin;
        private bool _isActive;
        private Camera _camera;
        private Vector3 _offset;
        private Vector3 _coinDefaultPos;
        private bool _isDragging;
        private float _timer;
        private float _delayer;
        private bool _canCool;
        private bool _tutorialSeen;
        private bool _isCooling;
        private float _coolingTimer, _coolingCooldown=.03f, _coolingDelayer;
        public AudioClip clip;

        private void Awake()
        {
            _coin = GameObject.Find("CoinRoot");
            _coinDefaultPos = _coin.transform.position;
            _camera=Camera.main;
        }

        private void Start()
        {
            disabledPosition.SetPositionAndRotation(transform.position,transform.rotation);
        }

        private void Update()
        {
            BeginDrag();
            Drag();
            CheckWaterContact();
        }

        private void CoolEffect()
        {
            if(!_isCooling)
                return;
            if (_coolingTimer <= 0)
            {
                if(_coolingTimer>1)
                    return;
                _coolingCooldown += _coolingDelayer;
                _coolingDelayer += .5f;
                AudioSource.PlayClipAtPoint(clip,_camera.transform.position);
            }
        }
        private void BeginDrag()
        {
            if(!_isActive)
                return;
            if (Input.GetMouseButtonDown(0))
            {
                _isDragging = true;
                _offset = Input.mousePosition - _camera.WorldToScreenPoint(_coin.transform.position);
            }
        }
        private void Drag()
        {
            if (!_isActive || !_isDragging) 
                return;

            if (Input.GetMouseButton(0))
            {
                _canCool = true;
                var calculatedPos = _camera.ScreenToWorldPoint(Input.mousePosition - _offset);
                var desiredPos = new Vector3(_coinDefaultPos.x, calculatedPos.y, _coinDefaultPos.z);
                _coin.transform.position = desiredPos;
            }
        }

        private void CheckWaterContact()
        {
            if(!_isActive || !_canCool)
                return;
            
            if (_coin.transform.position.y <= 1.2f) 
            {
                if(_timer>0)
                    return;
                _timer += _delayer;
                _isActive = false;
                ToolSignals.Instance.CoolHit?.Invoke();
                LevelSignals.Instance.OnChangeGameState?.Invoke(GameState.Extraction);
                if (_tutorialSeen)
                    return;
                _tutorialSeen = true;
                ToolSignals.Instance.TutorialInput?.Invoke();
            }

            _timer -= Time.deltaTime;
            _timer = Mathf.Clamp(_timer, 0, _delayer);
        }

        private void PlaceTheWater()
        {
            var desiredRotation = activePosition.rotation.eulerAngles;

            transform.DOMove(activePosition.position, 1).SetEase(Ease.OutSine).SetDelay(2);
            transform.DORotate(desiredRotation, 1).SetEase(Ease.OutSine).SetDelay(2);
        }
        

        private void ResetTheWater()
        {
            var desiredRotation = disabledPosition.rotation.eulerAngles;

            transform.DOMove(disabledPosition.position, 1).SetEase(Ease.InSine);
            transform.DORotate(desiredRotation, 1).SetEase(Ease.InSine);
        }

        public void Activate()
        {
            PlaceTheWater();
            _isActive = true;
        }

        public void Disable()
        {
            ResetTheWater();
            _isActive = false;
        }
    }
}