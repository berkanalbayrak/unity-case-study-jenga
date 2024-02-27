using System;
using ProjectDrive.EventBus;
using UnityEngine;
using UnityEngine.UI;

namespace YourWork.Scripts
{
    public class TestModeButton : MonoBehaviour
    {
        [SerializeField] private Button testModeButton;

        private EventBinding<ActiveStackChangedEvent> onActiveStackChangedEventBinding;
        private EventBinding<TestModeEndEvent> onTestModeEndEventBinding;
        
        private void OnEnable()
        {
            onActiveStackChangedEventBinding = new EventBinding<ActiveStackChangedEvent>(OnActiveStackChanged);
            EventBus<ActiveStackChangedEvent>.Register(onActiveStackChangedEventBinding);
            
            onTestModeEndEventBinding = new EventBinding<TestModeEndEvent>(OnTestModeEnd);
            EventBus<TestModeEndEvent>.Register(onTestModeEndEventBinding);
        }
        
        private void OnDisable()
        {
            EventBus<ActiveStackChangedEvent>.Deregister(onActiveStackChangedEventBinding);
            EventBus<TestModeEndEvent>.Deregister(onTestModeEndEventBinding);
        }
        
        private void Start()
        {
            testModeButton.onClick.AddListener(OnTestModeButtonClicked);
        }

        private void OnTestModeButtonClicked()
        {
            testModeButton.gameObject.SetActive(false);
            EventBus<TestModeStartEvent>.Raise(new TestModeStartEvent());
        }
        
        private void OnActiveStackChanged(ActiveStackChangedEvent @event)
        {
            testModeButton.gameObject.SetActive(true);
        }

        private void OnTestModeEnd()
        {
            Debug.Log("sa");
            testModeButton.gameObject.SetActive(true);
        }
    }
}