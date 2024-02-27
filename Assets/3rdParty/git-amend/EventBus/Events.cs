using YourWork.Scripts;
using YourWork.Scripts.Jenga;

namespace ProjectDrive.EventBus
{
    public interface IEvent {}
    
    public struct StackPieceClickedEvent : IEvent
    {
        public JengaPiece clickedPiece;
    }

    public struct ActiveStackChangedEvent : IEvent { }
    
    public struct TestModeStartEvent : IEvent { }
    
    public struct TestModeEndEvent : IEvent { }
    
}