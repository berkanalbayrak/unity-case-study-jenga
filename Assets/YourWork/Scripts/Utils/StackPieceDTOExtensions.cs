using YourWork.Scripts.Jenga;

namespace YourWork.Scripts.Utils
{
    public static class StackPieceDTOExtensions
    {
        public static JengaPieceType GetPieceType(this StackPieceDTO stackPieceDTO)
        {
            return stackPieceDTO.Mastery switch
            {
                0 => JengaPieceType.Glass,
                1 => JengaPieceType.Wood,
                2 => JengaPieceType.Stone,
            };
        }
    }
}