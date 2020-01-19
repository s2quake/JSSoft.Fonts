using System;
using System.ComponentModel.Composition;
using System.Threading.Tasks;
using System.Windows.Media;

namespace JSSoft.Font
{
    [Export(typeof(IBackgroundTaskService))]
    class BackgroundTaskService : IBackgroundTaskService
    {
        private IBackgroundTask task;

        public Task AddAsync(IBackgroundTask task)
        {
            throw new NotImplementedException();
        }

        public IBackgroundTask Task => throw new NotImplementedException();
    }
}