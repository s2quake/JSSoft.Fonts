using System.Threading.Tasks;
using System.Windows.Media;

namespace JSSoft.Font
{
    public interface IBackgroundTask
    {
        Task RunAsync();

        string Name { get; }
    }
}