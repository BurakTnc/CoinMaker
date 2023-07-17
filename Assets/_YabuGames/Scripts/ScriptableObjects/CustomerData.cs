using UnityEngine;
using UnityEngine.UI;

namespace _YabuGames.Scripts.ScriptableObjects
{
    [CreateAssetMenu(fileName = "Customer Data-", menuName = "New Customer Data", order = 0)]
    public class CustomerData : ScriptableObject
    {
        public Image coinImage;
        public string customerText;
        public int coinID;
    }
}