using System;
using _YabuGames.Scripts.Objects;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _YabuGames.Scripts.Controllers
{
    public class SpatulaController : MonoBehaviour
    {
        private Transform _collector;
        private DragNDropController _dragNDrop;
        private Transform _currentOre;
        public float _vibrationCoolDown = 1f;
        private float _timer;

        private void Awake()
        {
            _collector = GameObject.Find("Collector").transform;
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
            
            var randomizedDirection = Random.Range(-.2f, .3f);
            var desiredPosition = _collector.position;
            
            desiredPosition.x += randomizedDirection;
            desiredPosition.y = .4f;
            //ore.DOMove(desiredPosition, .8f).SetEase(Ease.OutBounce);
            _currentOre.DOJump(desiredPosition, .5f, 1, .8f).SetEase(Ease.OutBounce);
            _currentOre = null;

            _vibrationCoolDown = 1;
            _timer = 0;
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
    }
}