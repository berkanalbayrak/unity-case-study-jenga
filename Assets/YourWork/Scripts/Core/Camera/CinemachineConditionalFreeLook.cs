using Cinemachine;
using ProjectDrive.EventBus;
using UnityEngine;
using UnityEngine.EventSystems;
using YourWork.Scripts.Jenga;

namespace YourWork.Scripts.Core.Camera
{
    [RequireComponent(typeof(CinemachineFreeLook))]
    public class CinemachineConditionalFreeLook : MonoBehaviour
    {
        [SerializeField] private Transform orbitTransform;
        [SerializeField] private float rotationSpeed = 500f;
        [SerializeField] private Vector3 cameraOffset;
    
        private Vector3 camPreviousPosition;
        
        private const float groundHeight = 0.5f;

        private UnityEngine.Camera _mainCamera;
        
        private EventBinding<StackPieceClickedEvent> stackPieceClickedEventBinding;

        public JengaStack _orbitStack;
        private bool IsOrbitChanging = false;
        
        private CinemachineFreeLook _freeLookCamera;
        private float _xInputAxisDefaultSpeed;
        private float _yInputAxisDefaultSpeed;

        private void Awake()
        {
            _freeLookCamera = GetComponent<CinemachineFreeLook>();
            
            _xInputAxisDefaultSpeed = _freeLookCamera.m_XAxis.m_MaxSpeed;
            _yInputAxisDefaultSpeed = _freeLookCamera.m_YAxis.m_MaxSpeed;
        }
        
        private void OnEnable()
        {
            stackPieceClickedEventBinding = new EventBinding<StackPieceClickedEvent>(OnStackPieceClicked);
            EventBus<StackPieceClickedEvent>.Register(stackPieceClickedEventBinding);
        }

        private void OnDisable()
        {
            EventBus<StackPieceClickedEvent>.Deregister(stackPieceClickedEventBinding);
        }

        private void Update()
        {
            var enableFreeLook = (UnityEngine.Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject());
            
            _freeLookCamera.m_XAxis.m_MaxSpeed = enableFreeLook ? _xInputAxisDefaultSpeed : 0;
            _freeLookCamera.m_YAxis.m_MaxSpeed = enableFreeLook ? _yInputAxisDefaultSpeed : 0;
        }
        
        private void OnStackPieceClicked(StackPieceClickedEvent @event)
        {
            if (@event.clickedPiece == null || @event.clickedPiece.parentStack == null ||
                @event.clickedPiece.parentStack == _orbitStack || IsOrbitChanging) return;
            
            ChangeOrbitedStack(@event.clickedPiece.parentStack);
        }

        private void ChangeOrbitedStack(JengaStack stack)
        {
            if(_orbitStack != null)
                _orbitStack.isActive = false;
            
            _orbitStack = stack;
            orbitTransform = _orbitStack.transform;
            _freeLookCamera.Follow = orbitTransform;
            _freeLookCamera.LookAt = orbitTransform;
            _orbitStack.isActive = true;
            
            EventBus<ActiveStackChangedEvent>.Raise(new ActiveStackChangedEvent());
        }
    }
}