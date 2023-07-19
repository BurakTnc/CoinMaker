using System;
using _YabuGames.Scripts.Enums;
using _YabuGames.Scripts.Signals;
using DG.Tweening;
using UnityEngine;

namespace _YabuGames.Scripts.Objects
{
    public class Melter : MonoBehaviour
    {
        [SerializeField] private GameObject fire, liquid, pouringEffect;
        [SerializeField] private Transform pouringPosition, pouredLiquid;
        [SerializeField] private Vector3 pouringRotation;

        private Vector3 _startPosition;

        #region Subscribtions
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
            LevelSignals.Instance.OnChangeGameState += SetMelterStatus;
        }

        private void UnSubscribe()
        {
            LevelSignals.Instance.OnChangeGameState -= SetMelterStatus;
        }
        

        #endregion

        private void SetMelterStatus(GameState state)
        {
            switch (state)
            {
                case GameState.Melting:
                    StartMelting();
                    break;
                case GameState.Pouring:
                    EndMelting();
                    StartPouring();
                    break;
                default:
                    break;
            }
        }

        private void StartMelting()
        {
            _startPosition = transform.position;
            fire.SetActive(true);
            liquid.transform.DOScaleY(1, 3.5f).SetEase(Ease.OutSine).SetDelay(.5f).OnComplete(() =>
                LevelSignals.Instance.OnChangeGameState?.Invoke(GameState.Pouring));
        }

        private void EndMelting()
        {
            fire.SetActive(false);
        }

        private void StartPouring()
        {
            transform.DOMove(pouringPosition.position, 2).SetDelay(1).SetEase(Ease.OutSine).OnComplete(Pour);
            transform.DORotate(pouringRotation, 2).SetDelay(1.5f).SetEase(Ease.OutSine);
        }

        private void Pour()
        {
            pouringEffect.SetActive(true);
            liquid.transform.DOScaleX(0, 4).SetEase(Ease.InSine)
                .OnComplete(GoToPreviousPosition);
            liquid.transform.DOScaleZ(0, .5f).SetDelay(3.7f);
            pouredLiquid.gameObject.SetActive(true);
        }

        private void GoToPreviousPosition()
        {
            transform.DOMove(_startPosition, 2).SetEase(Ease.OutSine).OnComplete(() =>
                LevelSignals.Instance.OnChangeGameState?.Invoke(GameState.Fixing));
            transform.DORotate(Vector3.zero, 2);
            LevelSignals.Instance.OnToolChange?.Invoke(1);
        }
    }
}