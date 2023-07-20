using System;
using _YabuGames.Scripts.Signals;
using UnityEngine;
using UnityEngine.UI;

namespace _YabuGames.Scripts.Controllers
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private Slider camDistance, camRotation, gameSpeed;

        private Transform _player;
        private Vector3 _debuggedPosition, _debuggedRotation;
        private Vector3 _defaultPosition, _defaultRotation;
        private bool _isDebug;

        private void Start()
        {
            _defaultPosition = transform.position;
            _defaultRotation = transform.rotation.eulerAngles;

            _debuggedPosition = _defaultPosition;
            _debuggedRotation = _defaultRotation;
        }

        void Update()
        {
            Debug();
        }

        public void DebugModeOn()
        {
            _isDebug = true;
        }

        public void DebugModeOff()
        {
            _isDebug = false;
            transform.position = _defaultPosition;
            transform.rotation = Quaternion.Euler(_defaultRotation);
        }

        private void Debug()
        {
            if (_isDebug)
            {
                _debuggedPosition.z = camDistance.value;
                _debuggedRotation.x = camRotation.value;
                
                transform.position = _debuggedPosition;
                transform.rotation = Quaternion.Euler(_debuggedRotation);

                Time.timeScale = gameSpeed.value;
            }
        }
    }
}
