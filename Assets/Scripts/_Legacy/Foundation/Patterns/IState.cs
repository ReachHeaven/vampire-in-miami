namespace Foundation.Patterns
{
    /// <summary>
    /// Single state in a state machine.
    /// </summary>
    public interface IState
    {
        void Enter();
        void Update();
        void Exit();
    }
}
