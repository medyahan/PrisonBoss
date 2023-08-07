using System.Collections;
using System.Collections.Generic;
using HiGames.PrisonBoss.Event;
using HiGames.PrisonBoss.Prison;
using HiGames.PrisonBoss.Prisoner;
using HiGames.PrisonBoss.UI;
using HiGames.PrisonBoss.Upgrade;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace HiGames.PrisonBoss.Game_Manager
{
    public class GameManager : MonoBehaviour
    {
        // INSTANCE
        public static GameManager Instance;

        [Header("LEVELS")]
        [SerializeField] private List<GameObject> _levels = new List<GameObject>();
        public int LevelNo
        {
            get => PlayerPrefs.GetInt("LevelID", 1);
            set => PlayerPrefs.SetInt("LevelID", value);
        }

        [Header("GAME")]
        public static bool IsGameStarted;
        public static bool IsCompleted;
        public static bool IsFailed;
        private bool _isRunner = true;
        public bool IsRunner
        {
            get => _isRunner;
            set => _isRunner = value;
        }
        
        [Header("PRISONERS IN PRISON")]
        [SerializeField] private List<PrisonerController> _prisonersInPrison = new List<PrisonerController>();
        public List<PrisonerController> PrisonersInPrison
        {
            get => _prisonersInPrison;
            set => _prisonersInPrison = value;
        }

        [Header("PRISONER COLLECTING")] 
        [SerializeField] private List<PrisonerController> _prisoners = new List<PrisonerController>();
        public List<PrisonerController> Prisoners
        {
            get => _prisoners;
            set => _prisoners = value;
        }
        [SerializeField] private float _prisonerDistance;
        public float PrisonerDistance => _prisonerDistance;
        [SerializeField] private float _distanceToPlayer;
        public float DistanceToPlayer
        {
            get => _distanceToPlayer;
            set => _distanceToPlayer = value;
        }
        [SerializeField] private float _collectSpeed;
        public float CollectSpeed => _collectSpeed;
        [SerializeField] private float _curlingSmoothness;
        public float CurlingSmoothness => _curlingSmoothness;

        // SCORE
        
        public int TotalCash
        {
            get => PlayerPrefs.GetInt("TotalCash", 300);
            set => PlayerPrefs.SetInt("TotalCash", value);
        }

        private PrisonController _currentPrison;
        public PrisonController CurrentPrison
        {
            get => _currentPrison;
            set => _currentPrison = value;
        }

        private void Awake()
        {
            Instance = this;

            IsGameStarted = false;
            IsFailed = false;
            IsCompleted = false;

            //LoadLevel(LevelNo);
        }
        private void Start()
        {
            
            
        
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0) && !IsGameStarted)
            {
                IsGameStarted = true;
                UIManager.Instance.StartGame();
            }

        }

        public void OnLevelCompleted()
        {
            if (IsGameStarted)
            {

                IsGameStarted = false;
            }
        }

        public void OnLevelFailed()
        {
            if (IsGameStarted)
            {
                IsFailed = true;

                IsGameStarted = false;
            }
        }
    
        private void LoadLevel(int levelCounter)
        {
            foreach (var level in _levels)
            {
                level.SetActive(false);
            }
            _levels[levelCounter - 1].SetActive(true);
        }

        public void NextLevel()
        {
            if (LevelNo == _levels.Count) // son levelda ise
            {
                LevelNo = 1; // ilk level numarasi
            }
            else
            {
                LevelNo++;
            }
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void Restart()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void AddCash(int cash)
        {
            TotalCash += cash;
            
            UIManager.Instance.SetTotalCashText();
        }
        
        public void SpendCash(int cash)
        {
            TotalCash -= cash;
            
            UIManager.Instance.SetTotalCashText();
        }

        IEnumerator RefreshPrisonerIndexRoutine()
        {
            for (int i = 0; i < Prisoners.Count; i++)
            {
                Prisoners[i].PrisonerIndex = i;

                yield return new WaitForSeconds(0.1f);
            }
            
        }

        public void RefreshPrisonerIndex()
        {
            StartCoroutine(RefreshPrisonerIndexRoutine());
        }
        

    }
}
