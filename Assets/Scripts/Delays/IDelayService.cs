using System;

namespace Tools.Delay
{
    public interface IDelayService
    {
        int DelayedCall(float delay, Action callback, bool useRealTime = true);

        int DelayedCall(float delay,  object[] param, Action<object[]> callback, bool useRealTime = true);

        void RemoveDelayedCallsTo(Action callback);

        void RemoveDelayedCallsTo(Action<object[]> callback);

        void RemoveAtIndex(int index);

        void RemoveAll();
    }
}