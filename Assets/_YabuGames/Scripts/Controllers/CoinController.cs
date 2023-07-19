using System;
using _YabuGames.Scripts.Enums;
using _YabuGames.Scripts.Signals;
using UnityEngine;

namespace _YabuGames.Scripts.Controllers
{
    public class CoinController : MonoBehaviour
    {
        public static CoinController Instance;

        [SerializeField] private Material[] coinMaterials;

        private GameObject _selectedCoin, _stamp;
        private int _coinIndex;
        private bool _isPlaced;
        private int _stampCount;
        


        private void Awake()
        {
            if (Instance != this && Instance != null) 
            {
                Destroy(this);
                return;
            }

            Instance = this;
        }

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
        }

        private void UnSubscribe()
        {
            LevelSignals.Instance.OnChangeGameState -= SetCoinState;
            LevelSignals.Instance.OnSelectCoin -= SetRawCoin;
            LevelSignals.Instance.OnSelectStamp -= SetStampType;
            ToolSignals.Instance.HammerHit -= PlaceTheCoin;
            ToolSignals.Instance.StampHit -= StampTheCoin;
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
                    CoolTheCoin();
                    break;
            }
        }

        private void PlaceTheCoin()
        {
            if(_isPlaced)
                return;
            
            _isPlaced = true;
            _selectedCoin.SetActive(true);
        }
        private void CoolTheCoin()
        {
            
        }

        private void StampTheCoin()
        {
            if (_stampCount >= 10)
            {
                LevelSignals.Instance.OnChangeGameState?.Invoke(GameState.Pouring);
                return;
            }

            _stampCount++;
            _stamp.SetActive(true);
            _stamp.transform.localScale += Vector3.forward*1.75f;
        }

        private void SetStampType(int stampID)
        {
            _stamp = _selectedCoin.transform.GetChild(1).GetChild(stampID).gameObject;
        }
        
        
    }
}