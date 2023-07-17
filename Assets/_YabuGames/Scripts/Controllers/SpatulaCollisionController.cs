using System;
using UnityEngine;

namespace _YabuGames.Scripts.Controllers
{
    public class SpatulaCollisionController : MonoBehaviour
    {
        private SpatulaController _spatulaController;

        private void Awake()
        {
            _spatulaController = GetComponent<SpatulaController>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Ore"))
            {
                other.tag = "Untagged";
                other.transform.SetParent(null);
                _spatulaController.CollectOre(other.transform);
            }
        }
    }
}