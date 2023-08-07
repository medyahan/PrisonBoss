using System;
using DG.Tweening;
using HiGames.PrisonBoss.Game_Manager;
using HiGames.PrisonBoss.UI;
using UnityEngine;

namespace HiGames.PrisonBoss.Upgrade
{
    public class UpgradeArea : MonoBehaviour
    {
        [Header("TYPE")] 
        [SerializeField] private UpgradeType _upgradeType;

        private CanvasGroup _upgradeButton;

        enum UpgradeType
        {
            Prison = 0,
            Capacity = 1,
            Police = 2
        }

        private void Start()
        {
            _upgradeButton = UIManager.Instance.UpgradeButton;
            _upgradeButton.gameObject.SetActive(false);
            HideButton();
        }

        private void ShowButton()
        {
            SetButton();
            _upgradeButton.gameObject.SetActive(true);
            _upgradeButton.gameObject.transform.DOScale(new Vector3(1, 1, 1), 0.3f);
        }
        
        private void HideButton()
        {
            _upgradeButton.gameObject.transform.DOScale(new Vector3(0, 0, 0), 0.3f).OnComplete((() => _upgradeButton.gameObject.SetActive(false)));
        }
        

        private void OnTriggerStay(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                ShowButton();
            }
        }


        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                HideButton();
                UpgradeManager.Instance.IsUpgradable = false;
            }
        }

        private void SetButton()
        {
            switch (_upgradeType)
            {
                
                case UpgradeType.Prison:
                {
                    UpgradeManager.Instance.CurrentUpgradeNo = (int) UpgradeType.Prison;
                    
                    CheckButtonActivity(UpgradeManager.Instance.PrisonCosts[UpgradeManager.Instance.PrisonLevelNo + 1]);
                    
                    UIManager.Instance.UpgradeText.text = "Upgrade!";
                    UIManager.Instance.UpgradeCostText.text = UpgradeManager.Instance.PrisonCosts[UpgradeManager.Instance.PrisonLevelNo + 1].ToString();
                    break;
                }/*
                case UpgradeType.Capacity:
                {
                    UpgradeManager.Instance.CurrentUpgradeNo = (int) UpgradeType.Capacity;
                    
                    CheckCapacityButtonActivity(UpgradeManager.Instance.CapacityCost);
                    
                    UIManager.Instance.UpgradeText.text = "+1 Capacity";
                    UIManager.Instance.UpgradeCostText.text = UpgradeManager.Instance.CapacityCost.ToString();
                    break;
                }*/
                case UpgradeType.Police:
                {
                    UpgradeManager.Instance.CurrentUpgradeNo = (int) UpgradeType.Police;
                    
                    CheckButtonActivity(UpgradeManager.Instance.PoliceCosts[UpgradeManager.Instance.PoliceLevelNo + 1]);
                    
                    UIManager.Instance.UpgradeText.text = "Upgrade!";
                    UIManager.Instance.UpgradeCostText.text = UpgradeManager.Instance.PoliceCosts[UpgradeManager.Instance.PoliceLevelNo + 1].ToString();
                    break;
                }
            }
        }

        private void CheckButtonActivity(int upgradeCost)
        {
            if (upgradeCost <= GameManager.Instance.TotalCash)
            {
                UpgradeManager.Instance.IsUpgradable = true;
                _upgradeButton.alpha = 1;
            }
            else
            {
                UpgradeManager.Instance.IsUpgradable = false;
                _upgradeButton.alpha = 0.3f;
            }
        }

        /*
        private void CheckCapacityButtonActivity(int upgradeCost)
        {
            if (upgradeCost <= GameManager.Instance.TotalCash && !GameManager.Instance.CurrentPrison.IsLimitOver)
            {
                UpgradeManager.Instance.IsUpgradable = true;
                _upgradeButton.alpha = 1;
            }
            else
            {
                UpgradeManager.Instance.IsUpgradable = false;
                _upgradeButton.alpha = 0.3f;
            }
        }*/
    }
}
