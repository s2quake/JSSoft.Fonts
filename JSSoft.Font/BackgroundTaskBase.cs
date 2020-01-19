using System;
using System.Threading.Tasks;
using System.Windows.Media;

namespace JSSoft.Font
{
    public abstract class BackgroundTaskBase : IBackgroundTask
    {
        protected BackgroundTaskBase(string name)
        {
            this.Name = name ?? throw new ArgumentNullException(nameof(name));
        }

        public string Name { get; }

        protected abstract Task OnRunAsync();

        Task IBackgroundTask.RunAsync()
        {
            return this.OnRunAsync();
        }
    }
}