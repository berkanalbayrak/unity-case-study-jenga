using YourWork.Scripts.Utils;

namespace YourWork.Scripts.Jenga
{
    public record StackPieceDTO
    {
        public int Id { get; init; }
        public string Subject { get; init; }
        public string Grade { get; init; }
        public int Mastery { get; init; }
        public string DomainId { get; init; }
        public string Domain { get; init; }
        public string Cluster { get; init; }
        public string StandardId { get; init; }
        public string StandardDescription { get; init; }
    }
}