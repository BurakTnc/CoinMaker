using System;
using _YabuGames.Scripts.Enums;
using _YabuGames.Scripts.Signals;
using DG.Tweening;
using UnityEngine;

namespace _YabuGames.Scripts.Controllers
{
    public class CoinController : MonoBehaviour
    {
        public static CoinController Instance;

        [SerializeField] private Material[] coinMaterials;

        private GameObject _selectedCoin, _stamp;
        public int _coinIndex;
        private bool _isPlaced;
        private float _emission = 3f;
        private bool _isColored;


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
            Debug.Log("index");
        }

        private void SetCoinState(GameState state)
        {
            switch (state)
            {
                case GameState.Cooling:
                    RaiseTheCoin();
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

        private void RaiseTheCoin()
        {
            transform.DOMoveY(1.5f, 1).SetEase(Ease.OutSine).SetDelay(2f);
        }
        private void CoolTheCoin()
        {
            var coinMat = _selectedCoin.transform.GetChild(0).GetComponent<MeshRenderer>().material;
            var stampMat = _stamp.GetComponent<MeshRenderer>().material;
            var coinRenderer = _selectedCoin.transform.GetChild(0).GetComponent<MeshRenderer>();
            var stampRenderer = _stamp.GetComponent<MeshRenderer>();

            if (!_isColored)
            {
                _isColored = true;
                _emission = -10;
                //  coinMat.SetColor("_EmissionColor", new Color(0,0,0) * -10);
                //  stampMat.SetColor("_EmissionColor", new Color(0,0,0) * -10);
                // coinMat.DOColor(coinMaterials[_coinIndex].color, 2).SetEase(Ease.OutSine);
                // stampMat.DOColor(coinMaterials[_coinIndex].color, 2).SetEase(Ease.OutSine);
                coinRenderer.material = coinMaterials[_coinIndex];
                stampRenderer.material = coinMaterials[_coinIndex];
                
            }

            _emission -= .4f;
            
            _emission = Mathf.Clamp(_emission, -10, 3);
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