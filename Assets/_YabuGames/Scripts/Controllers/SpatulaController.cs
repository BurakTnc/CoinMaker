using System;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _YabuGames.Scripts.Controllers
{
    public class SpatulaController : MonoBehaviour
    {
        private Transform _collector;
        
        private void Awake()
        {
            _collector = GameObject.Find("Collector").transform;
        }

        public void CollectOre(Transform ore)
        {
            var randomizedDirection = Random.Range(-.2f, .3f);
            var desiredPosition = _collector.position;
            desiredPosition.x += randomizedDirection;
            desiredPosition.y = .4f;
            //ore.DOMove(desiredPosition, .8f).SetEase(Ease.OutBounce);
            ore.DOJump(desiredPosition, .5f, 1, 1.5f).SetEase(Ease.OutBounce);
        }
    }
}