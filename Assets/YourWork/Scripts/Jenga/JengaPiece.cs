using DG.Tweening;
using UnityEngine;

namespace YourWork.Scripts.Jenga
{
    public class JengaPiece : MonoBehaviour
    {
        public JengaStack parentStack;
        public StackPieceDTO StackPieceDTO { get; private set; }

        private Rigidbody _rigidbody;

        private Vector3 originalPosition;
        private Quaternion originalRotation;
        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        public void Initialize(JengaStack stack, StackPieceDTO stackPieceDTO, Vector3 defaultPosition, Quaternion defaultRotation)
        {
            originalPosition = defaultPosition;
            originalRotation = defaultRotation;
            parentStack = stack;
            StackPieceDTO = stackPieceDTO;
        }

        public void SetPhysicsActive(bool active) => _rigidbody.isKinematic = !active;


        public void ReturnToOriginalPosition()
        {
            transform.DOKill();
            transform.DOMove(originalPosition, 0.2f).SetEase(Ease.OutQuart);
            transform.DORotateQuaternion(originalRotation, 0.2f).SetEase(Ease.OutQuart);
        }
    }
}
