using System;
using _YabuGames.Scripts.Enums;
using _YabuGames.Scripts.Signals;
using TMPro;
using UnityEngine;

namespace _YabuGames.Scripts.Managers
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance;

        [SerializeField] private GameObject mainPanel, gamePanel, winPanel, stampPanel,orePanel;
        [SerializeField] private TextMeshProUGUI[] moneyText;
        [SerializeField] private GameObject[] tutorialTexts;

        private int _tutorialSeen;


        private void Awake()
        {
            #region Singleton

            if (Instance != this && Instance != null)
            {

                Destroy(this);
                return;
            }
            
            Instance = this;

            #endregion

        }

        private void OnEnable()
        {
            Subscribe();
        }

        private void OnDisable()
        {
            UnSubscribe();
        }

        private void Start()
        {
            SetMoneyTexts();
        }

        #region Subscribtions
        private void Subscribe()
                {
                    CoreGameSignals.Instance.OnLevelWin += LevelWin;
                    CoreGameSignals.Instance.OnLevelFail += LevelLose;
                    CoreGameSignals.Instance.OnGameStart += OnGameStart;
                    LevelSignals.Instance.OnChangeGameState += SetStates;
                    ToolSignals.Instance.TutorialInput += TutorialInput;
                }
        
                private void UnSubscribe()
                {
                    CoreGameSignals.Instance.OnLevelWin -= LevelWin;
                    CoreGameSignals.Instance.OnLevelFail -= LevelLose;
                    CoreGameSignals.Instance.OnGameStart -= OnGameStart;
                    LevelSignals.Instance.OnChangeGameState -= SetStates;
                    ToolSignals.Instance.TutorialInput -= TutorialInput;
                }

        #endregion
        
        private void OnGameStart()
        {
            mainPanel.SetActive(false);
            gamePanel.SetActive(true);
        }
        private void SetMoneyTexts()
        {
            if (moneyText.Length <= 0) return;

            foreach (var t in moneyText)
            {
                if (t)
                {
                    t.text = "$" + GameManager.Instance.GetMoney();
                }
            }
        }

        private void SetStates(GameState state)
        {
            switch (state)
            { 
                case GameState.SelectingOre:
                    orePanel.SetActive(true);
                    break;
                
                case GameState.CollectingOre:
                    orePanel.SetActive(false);
                    tutorialTexts[0].SetActive(true);
                    break;
                
                case GameState.Fixing:
                    tutorialTexts[1].SetActive(true);
                    break;
                
                case GameState.SelectingStamp:
                    stampPanel.SetActive(true);
                    break;
                
                case GameState.Stamping:
                    stampPanel.SetActive(false);
                    tutorialTexts[2].SetActive(true);
                    break;
                
                case GameState.Cooling:
                    tutorialTexts[3].SetActive(true);
                    break;
                default:
                    break;
            }
        }

        private void TutorialInput()
        {
            foreach (var text in tutorialTexts)
            {
                text.SetActive(false);
            }
        }
        private void LevelWin()
        {
            gamePanel.SetActive(false);
            winPanel.SetActive(true);
            HapticManager.Instance.PlaySuccessHaptic();
        }

        private void LevelLose()
        {
            gamePanel.SetActive(false);
            gamePanel.SetActive(true);
            HapticManager.Instance.PlayFailureHaptic();
        }

        public void ChooseStampButton(int stampID)
        {
            LevelSignals.Instance.OnSelectStamp?.Invoke(stampID);
            LevelSignals.Instance.OnChangeGameState?.Invoke(GameState.Stamping);
            LevelSignals.Instance.OnToolChange?.Invoke(2);
            stampPanel.SetActive(false);
            HapticManager.Instance.PlaySelectionHaptic();
        }

        public void ChooseOreButton(int oreID)
        {
            Debug.Log(oreID);
            LevelSignals.Instance.OnSelectCoin?.Invoke(oreID);
            LevelSignals.Instance.OnSelectOre?.Invoke(oreID);
            LevelSignals.Instance.OnChangeGameState?.Invoke(GameState.CollectingOre);
            orePanel.SetActive(false);
            HapticManager.Instance.PlaySelectionHaptic();
        }
        public void PlayButton()
        {
            CoreGameSignals.Instance.OnGameStart?.Invoke();
            LevelSignals.Instance.OnChangeGameState?.Invoke(GameState.Ordering); ///TEST///
            HapticManager.Instance.PlaySelectionHaptic();
        }

        public void MenuButton()
        {
            mainPanel.SetActive(true);
            HapticManager.Instance.PlayLightHaptic();
        }

        public void NextButton()
        {
            CoreGameSignals.Instance.OnLevelLoad?.Invoke();
            HapticManager.Instance.PlaySelectionHaptic();
        }

        public void RetryButton()
        {
            CoreGameSignals.Instance.OnLevelLoad?.Invoke();
            HapticManager.Instance.PlaySelectionHaptic();
        }
    }
}
