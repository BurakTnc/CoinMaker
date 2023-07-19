using System;
using _YabuGames.Scripts.Enums;
using _YabuGames.Scripts.Interfaces;
using _YabuGames.Scripts.Managers;
using _YabuGames.Scripts.Objects;
using _YabuGames.Scripts.Signals;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

namespace _YabuGames.Scripts.Controllers
{
    public class SpatulaController : MonoBehaviour,ITool
    {
        [SerializeField] private Transform activePosition, disabledPosition;

        private Transform _collector;
        private DragNDropController _dragNDrop;
        private Transform _currentOre;
        private float _vibrationCoolDown = .8f;
        private float _timer;
        private int _collectedOre;
        private void Awake()
        {
            _collector = GameObject.Find("Melter").transform;
            _dragNDrop = GetComponent<DragNDropController>();
        }

        private void Update()
        {
            MineProcess();
        }

        private void MineProcess()
        {
            if (_currentOre && _timer <= 0)
            {
                _vibrationCoolDown -= .2f;
                _timer += _vibrationCoolDown;

                if (_vibrationCoolDown <= 0)
                {
                    CollectOre();
                    return;
                }

                transform.DOShakeRotation(_vibrationCoolDown, Vector3.one * 2, 8, 90, true);
            }

            _timer -= Time.deltaTime;
            _timer = Mathf.Clamp(_timer, 0, _vibrationCoolDown);
            _vibrationCoolDown = Mathf.Clamp(_vibrationCoolDown, 0f, 1);
        }
        private void CollectOre()
        {
            if(!_currentOre)
                return;
            
            var randomizedDirection = Random.Range(-.05f, .06f);
            var desiredPosition = _collector.position;
  
            ShakeManager.Instance.ShakeCamera(true);
            _currentOre.SetParent(null);
            desiredPosition.x += randomizedDirection;
            desiredPosition.y = .9f;
            //ore.DOMove(desiredPosition, .8f).SetEase(Ease.OutBounce);
            _currentOre.DOJump(desiredPosition, .3f, 1, .8f).SetEase(Ease.OutBack);
            _currentOre = null;
            _collectedOre++;
            _vibrationCoolDown = 1;
            _timer = 0;
            
            if (_collectedOre >= 5) 
            {
                LevelSignals.Instance.OnChangeGameState?.Invoke(GameState.Melting);
            }
        }

        public void MineOre(Transform ore)
        {
            _currentOre = ore;
            _dragNDrop.OnMine();
            if (ore.TryGetComponent(out Ore oreController))
            {
                oreController.OnMine(true);
            }
        }

        public void EndMining(Transform ore)
        {
            _currentOre = null;
            _dragNDrop.OffMine();
            if (ore.TryGetComponent(out Ore oreController))
            {
                oreController.OnMine(false);
            }
        }

        public void Activate()
        {
            var desiredRotation = activePosition.rotation.eulerAngles;
            
            transform.DOMove(activePosition.position, 1).SetEase(Ease.OutSine).OnComplete(ReadyToUse).SetDelay(1);
            transform.DORotate(desiredRotation, 1).SetEase(Ease.InSine).SetDelay(1);
            
            void ReadyToUse()
            {
                _dragNDrop.enabled = true;
            }
        }

        public void Disable()
        {
            var desiredRotation = disabledPosition.rotation.eulerAngles;
            
            _dragNDrop.enabled = false;
            transform.DOMove(disabledPosition.position, 1).SetEase(Ease.OutSine);
            transform.DORotate(desiredRotation, 1).SetEase(Ease.InSine);
            
        }
    }
}