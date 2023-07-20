using UnityEngine;
using UnityEngine.UI;

namespace _YabuGames.Scripts.ScriptableObjects
{
    [CreateAssetMenu(fileName = "Customer Data-", menuName = "New Customer Data", order = 0)]
    public class CustomerData : ScriptableObject
    {
        public Sprite coinImage;
        public int oreID;
        public int stampID;
    }
}