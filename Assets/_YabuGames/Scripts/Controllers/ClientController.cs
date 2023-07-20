using System;
using _YabuGames.Scripts.Enums;
using _YabuGames.Scripts.ScriptableObjects;
using _YabuGames.Scripts.Signals;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _YabuGames.Scripts.Controllers
{
    public class ClientController : MonoBehaviour
    {
        [SerializeField] private CustomerData[] customerData;
        [SerializeField] private Transform orderingPosition;

        private Vector3 _startPosition;
        private Sprite _coinImage;
        private Transform _chosenClient;
        private Transform _chatBubble;
        private Vector3 _chatBubbleScale;

        private void Start()
        {
            _startPosition = transform.GetChild(0).position;
            ChooseClient();
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
            LevelSignals.Instance.OnChangeGameState += OrderCoin;
        }

        private void UnSubscribe()
        {
            LevelSignals.Instance.OnChangeGameState -= OrderCoin;
        }

        private void ChooseClient()
        {
            var r = Random.Range(0, 2);
            _chosenClient = transform.GetChild(r);
            ChooseRandomCoin();
            SetClientBubble();
        }

        private void ChooseRandomCoin()
        {
            var r = Random.Range(0, customerData.Length);

            _coinImage = customerData[r].coinImage;
        }

        private void SetClientBubble()
        {
            _chatBubble = _chosenClient.GetChild(0);
            _chatBubbleScale = _chatBubble.localScale;
            var sRenderer = _chosenClient.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>();

            sRenderer.sprite = _coinImage;
        }

        private void OrderCoin(GameState state)
        {
            if (state != GameState.Ordering) 
                return;
            
            _chosenClient.gameObject.SetActive(true);
            _chatBubble.transform.localScale=Vector3.zero;
            _chosenClient.DOMove(orderingPosition.position, 2).SetEase(Ease.OutSine).OnComplete(OpenChatBubble);

            void OpenChatBubble()
            {
                _chatBubble.gameObject.SetActive(true);
                _chatBubble.transform.DOScale(_chatBubbleScale, 1).SetEase(Ease.OutBack).OnComplete(EndOrder);
            }
        }

        private void EndOrder()
        {
            _chatBubble.transform.DOScale(Vector3.zero, 1).SetEase(Ease.InBack).SetDelay(3).OnComplete(Leave);

            void Leave()
            {
                _chosenClient.DORotate(Vector3.zero,.5f).SetEase(Ease.OutSine);
                _chosenClient.DOMove(_startPosition, 2).SetEase(Ease.InSine)
                    .OnComplete(() => _chosenClient.gameObject.SetActive(true));
                LevelSignals.Instance.OnChangeGameState?.Invoke(GameState.SelectingOre);
            }
        }
    }
}