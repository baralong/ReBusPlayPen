using System.Threading.Tasks;

namespace Api
{
    public interface IUserCommand
    {
        string Description { get; }
        char Key { get; }
        Task ExecuteAsync();
    }
}
