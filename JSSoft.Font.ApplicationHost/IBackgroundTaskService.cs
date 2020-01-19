using System;
using System.Threading.Tasks;
using System.Windows.Media;

namespace JSSoft.Font.ApplicationHost
{
    public interface IBackgroundTaskService
    {
        Task AddAsync(IBackgroundTask task);

        IBackgroundTask Task { get; }
    }
}