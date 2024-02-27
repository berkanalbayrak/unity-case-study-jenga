using ProjectDrive.EventBus;
using UnityEngine;
using YourWork.Scripts.Jenga;

namespace YourWork.Scripts.Core.Input
{
    public class PlayerClickToSelect : MonoBehaviour
    {
        private EventBinding<TestModeStartEvent> testModeStartEventBinding;
        private EventBinding<TestModeEndEvent> testModeEndEventBinding;

        private bool _inputEnabled = true;
        private void OnEnable()
        {
            testModeStartEventBinding = new EventBinding<TestModeStartEvent>(OnTestModeStart);
            EventBus<TestModeStartEvent>.Register(testModeStartEventBinding);
            
            testModeEndEventBinding = new EventBinding<TestModeEndEvent>(OnTestModeEnd);
            EventBus<TestModeEndEvent>.Register(testModeEndEventBinding);
        }

        private void OnTestModeStart(TestModeStartEvent @event) => _inputEnabled = false;

        private void OnTestModeEnd(TestModeEndEvent @event) => _inputEnabled = true;
        
        private void Update()
        {
            if (UnityEngine.Input.GetMouseButtonDown(0) && _inputEnabled)
            {
                var ray = UnityEngine.Camera.main.ScreenPointToRay(UnityEngine.Input.mousePosition);
                if (Physics.Raycast(ray, out var hit))
                {
                    if (hit.collider.gameObject.CompareTag("JengaPiece"))
                    {
                        var dto = hit.collider.GetComponent<JengaPiece>();
                        EventBus<StackPieceClickedEvent>.Raise(new StackPieceClickedEvent
                        {
                            clickedPiece = dto
                        });
                    }
                }
            }
        }
    }
}