using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;
using YourWork.Scripts.Core.API;
using YourWork.Scripts.Jenga;

namespace YourWork.Scripts.Core
{
    public class StackLoader : MonoBehaviour
    {
        [SerializeField] private JengaStack stackPrefab;
        [SerializeField] private Transform stackParent;

        [SerializeField] private Transform firstStackTransform;
        [SerializeField] private Vector3 stackOffset;

        private const float buildDelay = 1f;
        
        private async void Start()
        {
            BuildStacksFromAPI();
        }

        private List<JengaStack> _stacks = new List<JengaStack>(); 
        
        private async UniTaskVoid BuildStacksFromAPI()
        {
            var stackPieces = await StackAPI.GetStackPiecesFromAPI();
            
            if (stackPieces.Count > 0)
            {
                // Group by grade
                var groupedPieces = stackPieces.GroupBy(piece => piece.Grade)
                    .ToDictionary(group => group.Key, group => group.ToList());

                foreach (var gradeGroup in groupedPieces)
                {
                    var orderedGradeGroup = gradeGroup.Value
                        .OrderBy(p => p.Domain)
                        .ThenBy(p => p.Cluster)
                        .ThenBy(p => p.StandardId)
                        .ToList();
                    
                    BuildStackForGrade(gradeGroup.Key,  orderedGradeGroup); // Build the stack for each grade
                }
            }
            else
            {
                Debug.Log("No stack pieces were fetched.");
            }
        }

        private void BuildStackForGrade(string grade, List<StackPieceDTO> pieces)
        {
            var newStackPosition = firstStackTransform.position + (stackOffset * _stacks.Count);
            var newStack = Instantiate(stackPrefab, newStackPosition, Quaternion.identity);
            newStack.transform.parent = stackParent;
            
            newStack.Initialize(grade, pieces, buildDelay * _stacks.Count);
            _stacks.Add(newStack);
        }
    }
}