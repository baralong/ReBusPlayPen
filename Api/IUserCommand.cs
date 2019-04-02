namespace Api
{
    public interface IUserCommand
    {
        string Description { get; }
        char Key { get; }
        void Execute();
    }
}
