using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;
using YourWork.Scripts.Jenga;

namespace YourWork.Scripts.Core.API
{
    public static class StackAPI
    {
        private const string STACK_API_URI = "https://ga1vqcu3o1.execute-api.us-east-1.amazonaws.com/Assessment/stack";

        public static async UniTask<List<StackPieceDTO>> GetStackPiecesFromAPI()
        {
            var stackPiecesJSON = await GetRequestAsync(STACK_API_URI);
            if (!string.IsNullOrEmpty(stackPiecesJSON))
            {
                var stackPieces = JsonConvert.DeserializeObject<List<StackPieceDTO>>(stackPiecesJSON);
                return stackPieces;
            }
            else
            {
                return new List<StackPieceDTO>(); // Return an empty list if the request fails
            }
        }
        
        private static async UniTask<string> GetRequestAsync(string uri)
        {
            using var webRequest = UnityWebRequest.Get(uri);

            await webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError ||
                webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError($"WebRequest Error: {webRequest.error}");
                return null;
            }
            else
                return webRequest.downloadHandler.text;
            
        }
    }
}