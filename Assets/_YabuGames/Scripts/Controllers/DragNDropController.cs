using System;
using System.Collections;
using _YabuGames.Scripts.Managers;
using DG.Tweening;
using UnityEngine;

namespace _YabuGames.Scripts.Controllers
{
    public class DragNDropController : MonoBehaviour
    {
        private Camera _camera;
        private Vector3 _offset;
        private bool _isMining;
        private float _startingPosZ;
        private void Awake()
        {
            _camera = Camera.main;
            _startingPosZ = transform.position.z;
        }

        private void OnMouseDown()
        {
            _offset = Input.mousePosition - _camera.WorldToScreenPoint(transform.position);
            HapticManager.Instance.PlayLightHaptic();
        }

        private void OnMouseUp()
        {
            _isMining = false;
        }

        private void OnMouseDrag()
        {
            if (_isMining) 
                return;
            
            var calculatedPos = _camera.ScreenToWorldPoint(Input.mousePosition - _offset);
            var desiredPos = new Vector3(calculatedPos.x, calculatedPos.y, _startingPosZ);
            transform.position = desiredPos;
        }

        public void OnMine()
        {
            _isMining = true;
        }

        public void OffMine()
        {
            _isMining = false;
        }
    }
}