using DG.Tweening;
using ProjectDrive.EventBus;
using TMPro;
using UnityEngine;
using YourWork.Scripts.Jenga;

namespace YourWork.Scripts.UI
{
    public class JengaPieceDetailsPopup : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI gradeLevelDomainText;
        [SerializeField] private TextMeshProUGUI clusterText;
        [SerializeField] private TextMeshProUGUI standardIdDescText;

        
        [SerializeField] private RectTransform hiddenReferenceRectTransform;
        
        private EventBinding<StackPieceClickedEvent> stackPieceClickedEventBinding;
        private EventBinding<ActiveStackChangedEvent> onActiveStackChangedEventBinding;
        
        private EventBinding<TestModeStartEvent> onTestModeStartEventBinding;
        
        private JengaPiece _clickedPiece;
        private bool isActive = false;

        private Vector2 defaultPosition;
        private void Awake()
        {
            _clickedPiece = null;
            defaultPosition = transform.position;
            transform.position = hiddenReferenceRectTransform.position;
        }

        private void OnEnable()
        {
            stackPieceClickedEventBinding = new EventBinding<StackPieceClickedEvent>(OnStackPieceClicked);
            EventBus<StackPieceClickedEvent>.Register(stackPieceClickedEventBinding);
            
            onActiveStackChangedEventBinding = new EventBinding<ActiveStackChangedEvent>(OnActiveStackChanged);
            EventBus<ActiveStackChangedEvent>.Register(onActiveStackChangedEventBinding);
            
            onTestModeStartEventBinding = new EventBinding<TestModeStartEvent>(OnTestModeStart);
            EventBus<TestModeStartEvent>.Register(onTestModeStartEventBinding);
        }

        private void OnTestModeStart(TestModeStartEvent obj)
        {
            if(isActive) Hide();
        }

        private void OnDisable()
        {
            EventBus<StackPieceClickedEvent>.Deregister(stackPieceClickedEventBinding);
            EventBus<ActiveStackChangedEvent>.Deregister(onActiveStackChangedEventBinding);
        }
        
        private void OnActiveStackChanged(ActiveStackChangedEvent obj)
        {
            if(isActive)  Hide();
        }

        private void OnStackPieceClicked(StackPieceClickedEvent @event)
        {
            _clickedPiece = @event.clickedPiece;
            Show();
        }

        private void Show()
        {
            transform.position = hiddenReferenceRectTransform.transform.position;
            isActive = true;
            
            transform.DOMove(defaultPosition, 0.35f).SetEase(Ease.OutQuart);
            UpdatePopupText();
        }

        private void Hide()
        {
            isActive = false;
            transform.DOMove(hiddenReferenceRectTransform.position, 0.35f).SetEase(Ease.OutSine);
        }
        
        private void UpdatePopupText()
        {
            if (_clickedPiece == null) return;
            var dto = _clickedPiece.StackPieceDTO;

            gradeLevelDomainText.text = $"{dto.Grade} : {dto.Domain}";
            clusterText.text = dto.Cluster;
            standardIdDescText.text = $"{dto.StandardId} : {dto.StandardDescription}";
        }
    }
}