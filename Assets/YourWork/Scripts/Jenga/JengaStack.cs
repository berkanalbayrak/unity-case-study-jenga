using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using ProjectDrive.EventBus;
using Sirenix.Utilities;
using TMPro;
using UnityEngine;
using YourWork.Scripts.Utils;

namespace YourWork.Scripts.Jenga
{
    public class JengaStack : MonoBehaviour
    {
        [Header("Piece References")]
        [SerializeField] private JengaPiece glassPiecePrefab;
        [SerializeField] private JengaPiece woodenPiecePrefab;
        [SerializeField] private JengaPiece stonePiecePrefab;

        [SerializeField] private Transform pieceParentTransform;
        [SerializeField] private TextMeshPro gradeText;

        public List<JengaPiece> Pieces = new List<JengaPiece>();

        private float pieceLength => glassPiecePrefab.transform.localScale.x;
        private float pieceWidth => glassPiecePrefab.transform.localScale.y;
        private int piecesPerLayer = 3;
        private int currentLayer = 0;
        private int piecesInCurrentLayer = 0;

        public bool isActive = false;

        private const float delayPerPiece = 0.02f;

        private EventBinding<TestModeStartEvent> testModeClickedEventBinding;
    
        private void OnEnable()
        {
            testModeClickedEventBinding = new EventBinding<TestModeStartEvent>(OnTestModeClicked);
            EventBus<TestModeStartEvent>.Register(testModeClickedEventBinding);
        }
    
        private void OnDisable()
        {
            EventBus<TestModeStartEvent>.Deregister(testModeClickedEventBinding);
        }

        public void Initialize(string grade, List<StackPieceDTO> stackPieceDTOs, float buildDelay)
        {
            BuildTower(stackPieceDTOs, buildDelay).Forget();
            UpdateGradeText(grade);
        }

        private void UpdateGradeText(string grade)
        {
            gradeText.text = grade;
        }

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.Space))
                StartTestMode().Forget();
        }

        private async UniTaskVoid BuildTower(List<StackPieceDTO> stackPieceDTOs, float buildDelay)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(buildDelay));
        
            foreach (var stackPieceDTO in stackPieceDTOs)
            {
                AddPiece(stackPieceDTO);
            }
        }

        private void AddPiece(StackPieceDTO stackPieceDTO)
        {
            var selectedPiece = GetPrefabForPieceType(stackPieceDTO.GetPieceType());
            var nextPiecePosition = transform.position + GetNextPiecePosition();
            var nextPieceRotation = GetNextPieceRotation();
        
            var newPiece = Instantiate(selectedPiece, nextPiecePosition + (Vector3.up * 10f), nextPieceRotation, pieceParentTransform);
            newPiece.Initialize(this, stackPieceDTO, nextPiecePosition, nextPieceRotation);
        
            newPiece.transform.DOMove(nextPiecePosition, 0.4f).SetEase(Ease.OutQuart).SetDelay(Pieces.Count * delayPerPiece);
            Pieces.Add(newPiece);

            // Update counters for the next piece
            piecesInCurrentLayer++;
            if (piecesInCurrentLayer >= piecesPerLayer)
            {
                piecesInCurrentLayer = 0;
                currentLayer++;
            }
        }
        
        private Vector3 GetNextPiecePosition()
        {
            // Calculate offset based on number of pieces per layer and include the gap
            var offset = pieceLength * (piecesPerLayer - 1) / 2.0f;

            if (currentLayer % 2 == 0)
            {
                // Even layers: pieces placed along the x-axis
                var xPosition = (piecesInCurrentLayer * pieceLength) - offset;
                return new Vector3(xPosition, currentLayer * pieceWidth, 0);
            }
            else
            {
                // Odd layers: pieces placed along the z-axis
                var zPosition = (piecesInCurrentLayer * pieceLength) - offset;
                return new Vector3(0, currentLayer * pieceWidth, zPosition);
            }
        }
    
        private Quaternion GetNextPieceRotation()
        {
            // Odd layers: pieces are, rotated 90 degrees
            return currentLayer % 2 == 0 ? Quaternion.identity : Quaternion.Euler(0, 90, 0);
        }

        private JengaPiece GetPrefabForPieceType(JengaPieceType pieceType)
        {
            return pieceType switch
            {
                JengaPieceType.Glass => glassPiecePrefab,
                JengaPieceType.Wood => woodenPiecePrefab,
                JengaPieceType.Stone => stonePiecePrefab,
                _ => throw new ArgumentOutOfRangeException("JengaPieceType", pieceType, "Invalid piece type.")
            };
        }

        private async UniTaskVoid StartTestMode()
        {
            RemoveGlassPieces();
            Pieces.Where(piece => piece.StackPieceDTO.GetPieceType() != JengaPieceType.Glass)
                .ForEach(piece => piece.SetPhysicsActive(true));
            await UniTask.Delay(TimeSpan.FromSeconds(5));
            Pieces.ForEach(piece =>
            {
                piece.SetPhysicsActive(false);
                piece.ReturnToOriginalPosition();
            });
        
            EventBus<TestModeEndEvent>.Raise(new TestModeEndEvent());
        }
    
        public void ReturnPiecesToOriginalPosition()
        {
            Pieces.ForEach(piece => piece.ReturnToOriginalPosition());
        }

        private void RemoveGlassPieces()
        {
            Pieces.Where(piece => piece.StackPieceDTO.GetPieceType() == JengaPieceType.Glass).ToList().ForEach(piece =>
            {
                var direction = UnityEngine.Random.Range(0, 2) * 2 - 1; // Generates -1 or 1
                piece.transform.DOMove(piece.transform.position + (piece.transform.forward * (direction * 100)), 5f)
                    .SetSpeedBased(true)
                    .SetEase(Ease.OutQuart);
            });
        }
    
        public void SetAllPhysicsActive(bool active)
        {
            Pieces.ForEach(piece => piece.SetPhysicsActive(active));
        }
    
        private void OnTestModeClicked(TestModeStartEvent obj)
        {
            if(isActive)
                StartTestMode().Forget();
        }
    }
}