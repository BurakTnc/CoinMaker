using System;
using _YabuGames.Scripts.Interfaces;
using _YabuGames.Scripts.Signals;
using UnityEngine;

namespace _YabuGames.Scripts.Controllers
{
    public class ToolController : MonoBehaviour
    {
        private int _prevTool;
        private bool _hasPrevTool;

        private void Start()
        {
            
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
            LevelSignals.Instance.OnToolChange += ChangeTool;
        }

        private void UnSubscribe()
        {
            LevelSignals.Instance.OnToolChange -= ChangeTool;
        }

        private void ChangeTool(int toolID)
        {
            if (_hasPrevTool)
            {
                transform.GetChild(_prevTool).GetComponent<ITool>().Disable();
            }

            transform.GetChild(toolID).GetComponent<ITool>().Activate();
            _prevTool = toolID;
            _hasPrevTool = true;
        }
    }
}