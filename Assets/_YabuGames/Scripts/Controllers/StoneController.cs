using System;
using _YabuGames.Scripts.Enums;
using _YabuGames.Scripts.Signals;
using DG.Tweening;
using UnityEngine;

namespace _YabuGames.Scripts.Controllers
{
    public class StoneController : MonoBehaviour
    {
        [SerializeField] private Transform activePosition;

        private int _selectedOre;
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
            LevelSignals.Instance.OnChangeGameState += SetStoneState;
            LevelSignals.Instance.OnSelectOre += SelectOre;
        }

        private void UnSubscribe()
        {
            LevelSignals.Instance.OnChangeGameState -= SetStoneState;
            LevelSignals.Instance.OnSelectOre -= SelectOre;
        }

        private void SetStoneState(GameState state)
        {
            if (state != GameState.CollectingOre) 
                return;
            var stone = transform.GetChild(_selectedOre);
            stone.DOMove(activePosition.position, 1).SetEase(Ease.OutSine).SetDelay(0).OnComplete(BeginMining);
            stone.DORotate(activePosition.rotation.eulerAngles, 1);
        }

        private void BeginMining()
        {
           // LevelSignals.Instance.OnChangeGameState?.Invoke(GameState.CollectingOre);
            LevelSignals.Instance.OnToolChange?.Invoke(0);
        }
        private void SelectOre(int oreID)
        {
            _selectedOre = oreID;
        }
    }
}