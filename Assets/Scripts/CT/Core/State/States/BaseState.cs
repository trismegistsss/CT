using System;
using Zenject;

namespace CT.Core.State
{
    public abstract class BaseState
    {
        public virtual void Initialize(){}
        public virtual void Handle(){}
        public virtual void Tick(){}
        public virtual void Dispose(){}
    }
}