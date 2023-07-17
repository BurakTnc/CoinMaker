using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace _YabuGames.Scripts.Controllers
{
    public class DragNDropController : MonoBehaviour
    {
        private Camera _camera;
        private Vector3 _offset;
        private bool _isDragging;
        private void Awake()
        {
            _camera = Camera.main;
        }

        private void StartDrag()
        {
            if (!Input.GetMouseButtonDown(0)) 
                return;
            _isDragging = true;
            _offset = Input.mousePosition - _camera.WorldToScreenPoint(transform.position);
        }

        private void EndDrag()
        {
            if (!Input.GetMouseButtonUp(0))
                return;
            
            _isDragging = false;
        }

        private void Drag()
        {
            if (!Input.GetMouseButton(0)) 
                return;
            
            var calculatedPos = _camera.ScreenToWorldPoint(Input.mousePosition - _offset);
            var desiredPos = new Vector3(calculatedPos.x, .5f, calculatedPos.z);
            transform.position = desiredPos;
        }

        private void Update()
        {
            StartDrag();
            Drag();
            EndDrag();
        }
    }
}