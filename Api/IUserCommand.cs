using System.Threading.Tasks;

namespace Api
{
    public interface IUserCommand
    {
        string Description { get; }
        Task ExecuteAsync();
    }
}
