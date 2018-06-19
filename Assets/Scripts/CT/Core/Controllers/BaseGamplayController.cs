using UniRx;

namespace CT.Core.Controllers
{
    public class BaseGamplayController
    {
        protected CompositeDisposable Disposables;

        public virtual void Dispose()
        {
            if (null != Disposables)
                Disposables.Dispose();
        }

        public virtual void Initialization()
        {
            Disposables = new CompositeDisposable();
        }

        public virtual void Tick()
        {

        }
    }
}
