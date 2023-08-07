using DG.Tweening;
using HiGames.PrisonBoss.Event;
using HiGames.PrisonBoss.Game_Manager;
using TMPro;
using UnityEngine;

namespace HiGames.PrisonBoss.UI
{
    public class UIAnimationController : MonoBehaviour
    {
        [Header("FOR CASH COLLECTING AND SPENDING")] 
        
        [SerializeField] private Camera _cam;
        [SerializeField] private GameObject _cash2D;
        [SerializeField] private GameObject _cashCanvasGroup;
        [SerializeField] private float _moveAnimateTime;
        [SerializeField] private Vector3 _shakeVector;
        [SerializeField] private float _shakeAnimateTime;
        
        [SerializeField] private TMP_Text _spendCashText;
        [SerializeField] private Transform _textSpawnPoint;
        
        [Header("FOR INFO")]
        [SerializeField] private TMP_Text _infoText;

        private void Start()
        {
            EventManager.Instance.OnCashCollected += AnimateCashCollecting;
            EventManager.Instance.OnCashSpent += AnimateCashSpending;
            EventManager.Instance.OnInfoTextUpdated += AnimateInfoText;
            
            _infoText.transform.localScale = Vector3.zero;
            _infoText.gameObject.SetActive(false);
        }

        private void AnimateCashCollecting(int cashAmount, Vector3 fromPos)
        {
            var spamPos = _cam.WorldToScreenPoint(fromPos); // Spawn noktasi belirleme
            var cash = Instantiate(_cash2D, spamPos + new Vector3(0,170, 0), Quaternion.identity, _cashCanvasGroup.transform);
            
            var targetPos = _cashCanvasGroup.transform.position; // Hedef noktasi belirleme
            
            cash.transform.DOMove(targetPos, _moveAnimateTime).SetEase(Ease.InSine).OnComplete(() =>
            {
                cash.gameObject.SetActive(false);
                _cashCanvasGroup.transform.DORewind();             
                _cashCanvasGroup.transform.DOPunchScale(_shakeVector, _shakeAnimateTime);
                GameManager.Instance.AddCash(cashAmount);
            });

        }

        private void AnimateCashSpending(int cashAmount)
        {
            var spawnPoint = _textSpawnPoint.position;
            
            _cashCanvasGroup.transform.DORewind();
            _cashCanvasGroup.transform.DOPunchScale(_shakeVector, _shakeAnimateTime).OnComplete(() =>
            {
                var spendAmount = Instantiate(_spendCashText, spawnPoint, Quaternion.identity, _cashCanvasGroup.transform);
                spendAmount.text = "- " + cashAmount;
                
                Sequence sequence = DOTween.Sequence();
                sequence.Append(
                        spendAmount.transform.DOMove(new Vector3(spawnPoint.x, spawnPoint.y - 90, spawnPoint.z),
                            0.5f))
                    .Insert(0,
                        spendAmount.transform.DOScale(1f, 0.5f).SetEase(Ease.InSine)
                            .OnComplete(() => Destroy(spendAmount)));
                
                GameManager.Instance.SpendCash(cashAmount);
            });
            

        }

        private void AnimateInfoText(string info, float animateTime)
        {
            _infoText.text = info;
            _infoText.gameObject.SetActive(true);
            
            _infoText.transform.DOScale(new Vector3(1, 1, 1), 0.3f).OnComplete((() =>
            {
                _infoText.transform.DOScale(new Vector3(0, 0, 0), 0.3f).SetDelay(animateTime).OnComplete((() => _infoText.gameObject.SetActive(false)));
            }));

        }
    }
}
