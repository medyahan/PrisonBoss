using HiGames.PrisonBoss.Event;
using HiGames.PrisonBoss.Game_Manager;
using TMPro;
using UnityEngine;

namespace HiGames.PrisonBoss.UI
{
    public class UIManager : MonoBehaviour
    {
        // INSTANCE
        public static UIManager Instance;

        [Header("START PANEL")] 
        [SerializeField] private GameObject _startPanel;

        [Header("GAME PANEL")] 
        [SerializeField] private GameObject _gamePanel;
        [SerializeField] private TMP_Text _totalCashText;
        
        [Header("PRISONER COUNT")] 
        [SerializeField] private GameObject _prisonerCountGroup;
        [SerializeField] private TMP_Text _prisonerCountText;
        [SerializeField] private GameObject _prisonerCountGroupCar;
        [SerializeField] private TMP_Text _prisonerCountTextCar;
        
        [Header("UPGRADE")] 
        [SerializeField] private CanvasGroup _upgradeButton;
        public CanvasGroup UpgradeButton => _upgradeButton;
        [SerializeField] private TMP_Text _upgradeText;
        public TMP_Text UpgradeText
        {
            get => _upgradeText;
            set => _upgradeText = value;
        }
        [SerializeField] private TMP_Text _upgradeCostText;
        public TMP_Text UpgradeCostText
        {
            get => _upgradeCostText;
            set => _upgradeCostText = value;
        }
        
        [Header("UI ANIMATION CONTROL")] 
        [SerializeField] private UIAnimationController _uiAnimationControl;
        public UIAnimationController UIAnimationControl => _uiAnimationControl;
        private void Awake()
        {
            Instance = this;
        }
    
        private void Start()
        {
            UpdatePrisonerCountText();
            SetTotalCashText();
            
            ResetUI();

            EventManager.Instance.OnMovementChanged += ActivatePrisonerCount;
        }

        private void Update()
        {
            /*
            if(GameManager.Instance.IsRunner)
                _prisonCountGroup.SetActive(true);
            else
            {
                _prisonCountGroup.SetActive(false);
            }*/

        }

        private void ResetUI()
        {
            _startPanel.SetActive(true);
            _gamePanel.SetActive(true);
            _prisonerCountGroup.SetActive(false);
            _prisonerCountGroupCar.SetActive(false);
        }

        public void StartGame()
        {
            _startPanel.SetActive(false);
            _gamePanel.SetActive(true);

            if (GameManager.Instance.IsRunner)
            {
                _prisonerCountGroup.SetActive(true);
                _prisonerCountGroupCar.SetActive(true);
            }
                
        }

        public void UpdatePrisonerCountText()
        {
            _prisonerCountText.text = GameManager.Instance.Prisoners.Count.ToString();
            _prisonerCountTextCar.text = GameManager.Instance.Prisoners.Count.ToString();
        }

        public void SetTotalCashText()
        {
            _totalCashText.text = GameManager.Instance.TotalCash.ToString();
        }

        private void ActivatePrisonerCount()
        {
            if(_prisonerCountGroup.activeInHierarchy)
                _prisonerCountGroup.SetActive(false);
            else 
                _prisonerCountGroup.SetActive(true);
        }
        
        
    }
}
