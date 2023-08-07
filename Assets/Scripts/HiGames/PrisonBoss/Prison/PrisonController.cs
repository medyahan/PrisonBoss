using System;
using System.Collections.Generic;
using HiGames.PrisonBoss.Event;
using HiGames.PrisonBoss.Game_Manager;
using HiGames.PrisonBoss.Upgrade;
using UnityEngine;

namespace HiGames.PrisonBoss.Prison
{
    public class PrisonController : MonoBehaviour
    {
        [Header("PRISON LEVELS")] 
        [SerializeField] private List<GameObject> _prisonLevels = new List<GameObject>();
        
        [Header("PRISON CELLS")] 
        [SerializeField] private List<PrisonLoc> prisonerLocs = new List<PrisonLoc>();
        public List<PrisonLoc> PrisonerLocs => prisonerLocs;

        private bool _areCapacityFulled;

        private void Start()
        {
            EventManager.Instance.OnPrisonUpgraded += UpdatePrison;
            EventManager.Instance.OnMovementChanged += ResetPrison;
            //EventManager.Instance.OnCapacityUpgraded += UpdateCapacity;
            
            UpdatePrison();
        }

        private void Update()
        {
            if (!_areCapacityFulled && GameManager.Instance.PrisonersInPrison.Count == prisonerLocs.Count)
            {
                EventManager.Instance.InfoTextUpdated("PRISON CAPACITY\nIS FULL!", 1.5f);
                _areCapacityFulled = true;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                GameManager.Instance.CurrentPrison = this;
            }
        }


        private void UpdatePrison()
        {
            foreach (var prisonLevel in _prisonLevels)
            {
                prisonLevel.SetActive(false);
            }
            _prisonLevels[UpgradeManager.Instance.PrisonLevelNo].SetActive(true);

            UpdatePrisonerLocs();
            //UpdateCapacity();
        }

        private void UpdatePrisonerLocs()
        {
            GameObject prison = _prisonLevels[UpgradeManager.Instance.PrisonLevelNo];
            
            List<PrisonLoc> locs = new List<PrisonLoc>();

            for (int i = 0; i < prison.transform.childCount; i++)
            {
                if(prison.transform.GetChild(i).GetComponent<PrisonLoc>() != null)
                    locs.Add(prison.transform.GetChild(i).GetComponent<PrisonLoc>());
            }
            prisonerLocs = locs;
        }

        private void ResetPrison()
        {
            if(GameManager.Instance.IsRunner)
                GameManager.Instance.PrisonersInPrison.Clear();
        }
        
    }
}
