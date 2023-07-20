using System;
using _YabuGames.Scripts.Enums;
using _YabuGames.Scripts.Signals;
using DG.Tweening;
using UnityEngine;

namespace _YabuGames.Scripts.Controllers
{
    public class CoinController : MonoBehaviour
    {
        [SerializeField] private Material[] coinMaterials;

        private GameObject _selectedCoin, _stamp;
        private int _coinIndex;
        private bool _isPlaced;
        private bool _isColored;

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
            LevelSignals.Instance.OnChangeGameState += SetCoinState;
            LevelSignals.Instance.OnSelectCoin += SetRawCoin;
            LevelSignals.Instance.OnSelectStamp += SetStampType;
            ToolSignals.Instance.HammerHit += PlaceTheCoin;
            ToolSignals.Instance.StampHit += StampTheCoin;
            ToolSignals.Instance.CoolHit += CoolTheCoin;
        }

        private void UnSubscribe()
        {
            LevelSignals.Instance.OnChangeGameState -= SetCoinState;
            LevelSignals.Instance.OnSelectCoin -= SetRawCoin;
            LevelSignals.Instance.OnSelectStamp -= SetStampType;
            ToolSignals.Instance.HammerHit -= PlaceTheCoin;
            ToolSignals.Instance.StampHit -= StampTheCoin;
            ToolSignals.Instance.CoolHit -= CoolTheCoin;
        }

        private void SetRawCoin(int coinID)
        {
            _selectedCoin = transform.GetChild(coinID).gameObject;
            _coinIndex = coinID;
        }

        private void SetCoinState(GameState state)
        {
            switch (state)
            {
                case GameState.Cooling:
                    RaiseTheCoin();
                    break;
                case GameState.Extraction:
                    ShowTheCoin();
                    break;
            }
        }

        private void ShowTheCoin()
        {
            transform.DOMoveY(2.5f, 1).SetEase(Ease.OutBack).OnComplete(Rotate).SetDelay(4f);

            void Rotate()
            {
                transform.DORotate(new Vector3(-60, 0, 0), .5f).SetEase(Ease.InBack).OnComplete(Shake);
            }

            void Shake()
            {
                transform.DOShakeRotation(.2f, new Vector3(0, 20, 0), 10, 100).SetLoops(4, LoopType.Yoyo)
                    .OnComplete(() => CoreGameSignals.Instance.OnLevelWin?.Invoke()).SetDelay(.2f);
                
            }
        }
        private void PlaceTheCoin()
        {
            if(_isPlaced)
                return;
            
            _isPlaced = true;
            _selectedCoin.SetActive(true);
        }

        private void RaiseTheCoin()
        {
            transform.DOMoveY(1.5f, 1).SetEase(Ease.OutSine).SetDelay(2f);
        }
        private void CoolTheCoin()
        {
            var coinRenderer = _selectedCoin.transform.GetChild(0).GetComponent<MeshRenderer>();
            var stampRenderer = _stamp.GetComponent<MeshRenderer>();

            if (!_isColored)
            {
                _isColored = true;
                coinRenderer.material = coinMaterials[_coinIndex];
                stampRenderer.material = coinMaterials[_coinIndex];
                
            }
            
        }

        private void StampTheCoin()
        {
            _stamp.SetActive(true);
            _stamp.transform.localScale += Vector3.forward*1.75f;
        }

        private void SetStampType(int stampID)
        {
            _stamp = _selectedCoin.transform.GetChild(1).GetChild(stampID).gameObject;
        }
        
        
    }
}