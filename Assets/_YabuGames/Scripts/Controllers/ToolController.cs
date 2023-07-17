using System;
using _YabuGames.Scripts.Signals;
using UnityEngine;

namespace _YabuGames.Scripts.Controllers
{
    public class ToolController : MonoBehaviour
    {
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
            LevelSignals.Instance.OnToolChange += ChangeTool;
        }

        private void UnSubscribe()
        {
            LevelSignals.Instance.OnToolChange -= ChangeTool;
        }

        private void ChangeTool(int toolID)
        {
            for (var i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(i == toolID);
            }
        }
    }
}