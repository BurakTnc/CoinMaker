using _YabuGames.Scripts.Enums;
using _YabuGames.Scripts.Signals;
using DG.Tweening;
using UnityEngine;

namespace _YabuGames.Scripts.Objects
{
    public class Ore : MonoBehaviour
    {
        [SerializeField] private int oreID;
        
        private bool _onMine;
        private float _vibrationCooldown = 1f;
        private float _timer;

        private void OnEnable()
        {
            Subscribe();
        }

        private void OnDisable()
        {
            UnSubscribe();
        }

        private void Subscribe()
        {
            LevelSignals.Instance.OnChangeGameState += SetMeltingStatus;
        }

        private void UnSubscribe()
        {
            LevelSignals.Instance.OnChangeGameState -= SetMeltingStatus;
        }
        
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
        private void SetMeltingStatus(GameState state)
        {
            switch (state)
            {
                case GameState.Melting:
                    StartMelting();
                    break;
                case GameState.Pouring:
                    break;
                default:
                    break;
            }
        }

        private void StartMelting()
        {
            LevelSignals.Instance.OnSelectCoin?.Invoke(oreID);
            transform.DOScale(Vector3.zero, 3.5f).SetEase(Ease.InSine).SetDelay(.2f);
        }
    }
}