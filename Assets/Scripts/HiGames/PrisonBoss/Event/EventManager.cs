using System;
using UnityEngine;

namespace HiGames.PrisonBoss.Event
{
    public class EventManager : MonoBehaviour
    {
        public static EventManager Instance;
        
        public event Action OnPrisonUpgraded;
        public event Action OnCapacityUpgraded;
        public event Action OnPoliceUpgraded;
        public event Action OnMovementChanged;
        public event Action <int, Vector3> OnCashCollected;
        public event Action <int> OnCashSpent;
        public event Action <string, float> OnInfoTextUpdated;

        private void Awake()
        {
            Instance = this;
        }
        
        public void PrisonUpgraded() // BU FONKSIYONU KULLANARAK BAGIRIR !!
        {
            OnPrisonUpgraded?.Invoke();
        }

        public void CapacityUpgraded()
        {
            OnCapacityUpgraded?.Invoke();
        }

        public void PoliceUpgraded()
        {
            OnPoliceUpgraded?.Invoke();
        }
        
        public void MovementChanged()
        {
            OnMovementChanged?.Invoke();
        }
        
        public void CashCollected(int cashAmount, Vector3 fromPos)
        {
            OnCashCollected?.Invoke(cashAmount, fromPos);
        }
        
        public void CashSpent(int cashAmount)
        {
            OnCashSpent?.Invoke(cashAmount);
        }
        
        public void InfoTextUpdated(string info, float animateTime)
        {
            OnInfoTextUpdated?.Invoke(info, animateTime);
        }


    }
}
