using System.Collections.Generic;
using HiGames.PrisonBoss.Event;
using HiGames.PrisonBoss.Game_Manager;
using UnityEngine;

namespace HiGames.PrisonBoss.Upgrade
{
    public class UpgradeManager : MonoBehaviour
    {
        // INSTANCE
        public static UpgradeManager Instance;
        
        [Header("UPGRADABLE")] 
        private bool _isUpgradable;
        public bool IsUpgradable
        {
            get => _isUpgradable;
            set => _isUpgradable = value;
        }

        /*
        [Header("CAPACITY")] 
        [SerializeField] private int _beginningCapacity;

        public int Capacity
        {
            get => PlayerPrefs.GetInt("PrisonCapacity", _beginningCapacity);
            set => PlayerPrefs.SetInt("PrisonCapacity", value);
        }

        [SerializeField] private int _capacityCost;
        public int CapacityCost => _capacityCost;*/

        public int PrisonLevelNo
        {
            get => PlayerPrefs.GetInt("PrisonLevel", 0);
            set => PlayerPrefs.SetInt("PrisonLevel", value);
        }
        
        [SerializeField] private List<int> _prisonCosts = new List<int>();
        public List<int> PrisonCosts => _prisonCosts;
        
        public int PoliceLevelNo
        {
            get => PlayerPrefs.GetInt("PoliceLevel", 0);
            set => PlayerPrefs.SetInt("PoliceLevel", value);
        }
        
        [SerializeField] private List<int> _policeCosts = new List<int>();
        public List<int> PoliceCosts => _policeCosts;

        public int CurrentUpgradeNo { get; set; }

        private void Awake()
        {
            Instance = this;
        }

        public void Upgrade()
        {
            switch (CurrentUpgradeNo)
            {
                case 0:
                    UpgradePrison();
                    break;
                /*
                case 1:
                    UpgradeCapacity();
                    break;*/
                case 2:
                    UpgradePolice();
                    break;
                
            }
        }

        /*
        private void UpgradeCapacity()
        {
            if (_isUpgradable && !GameManager.Instance.CurrentPrison.IsLimitOver)
            {
                EventManager.Instance.CashSpent(_capacityCost);
                Capacity++;
            
                // Capacity levelini değiştir
            
                EventManager.Instance.CapacityUpgraded();
            }
        }*/

        private void UpgradePrison()
        {
            if (_isUpgradable)
            {
                PrisonLevelNo++;
                
                EventManager.Instance.CashSpent(_prisonCosts[PrisonLevelNo]);
            
                // Hapishane levelini değiştir
            
                EventManager.Instance.PrisonUpgraded();
            }
        }

        private void UpgradePolice()
        {
            if (_isUpgradable)
            {
                PoliceLevelNo++;
                
                EventManager.Instance.CashSpent(_policeCosts[PoliceLevelNo]);
            
                // Police levelini değiştir
            
                EventManager.Instance.PoliceUpgraded();
                EventManager.Instance.InfoTextUpdated("YOU HAVE\nA POLICE CAR!", 1f);
            }
            
        }
    }
}
