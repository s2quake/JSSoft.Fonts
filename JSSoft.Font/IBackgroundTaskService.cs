using System;
using System.Threading.Tasks;
using System.Windows.Media;

namespace JSSoft.Font
{
    public interface IBackgroundTaskService
    {
        Task AddAsync(IBackgroundTask task);

        IBackgroundTask Task { get; }
    }
}