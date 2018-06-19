using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Tools.Delay
{
    public class DelayService : IDelayService, ITickable
    {
        private List<DelayedCallback> _delayedCalls = new List<DelayedCallback>();
        private int _delayIndex;

        public int DelayedCallsCount
        {
            get { return _delayedCalls.Count; }
        }

        public void Tick()
        {
            
            if (0 == _delayedCalls.Count)
                return;
            
            _delayedCalls.RemoveAll((DelayedCallback currentCall) =>
            {
                if (currentCall.Delay > GetTime(currentCall.useRealTime))
                    return false;
                
                var objParam = currentCall.ParamObj;

                if (currentCall.Callback != null)
                    currentCall.Callback();

                if (currentCall.ObjDelCallback != null)
                    currentCall.ObjDelCallback(objParam);

                currentCall.ObjDelCallback = null;
                currentCall.Callback = null;
                currentCall.ObjDelCallback = null;

                return true;
            });
        }

        protected virtual float GetTime(bool useRealTime)
        {
            return useRealTime ? Time.realtimeSinceStartup : Time.time;
        }

        #region Add Delay

        public int DelayedCall(float delay, Action callback, bool useRealTime = true)
        {
            var idx = _delayIndex++;
            _delayedCalls.Add(new DelayedCallback(delay + GetTime(useRealTime), idx, callback, useRealTime));
            return idx;
        }

        public int DelayedCall(float delay, object [] param, Action<object[]> callback, bool useRealTime = true)
        {
            var idx = _delayIndex++;
            _delayedCalls.Add(new DelayedCallback(delay + GetTime(useRealTime), idx, callback, param, useRealTime));       
            return idx;
        }

        #endregion

        #region Remove Delay

        public void RemoveDelayedCallsTo(Action callback)
        {
            if (0 == _delayedCalls.Count)
                return;
            
            _delayedCalls.RemoveAll((DelayedCallback currentCall) =>
            {
                if (currentCall.Callback == callback)
                {
                    currentCall.Callback = null;

                    return true;
                }

                return false;
            });
        }

        public void RemoveDelayedCallsTo(Action<object[]> callback)
        {
            if (0 == _delayedCalls.Count)
                return;
            
            _delayedCalls.RemoveAll((DelayedCallback currentCall) =>
            {
                if (currentCall.ObjDelCallback == callback)
                {
                    currentCall.ObjDelCallback = null;
                    currentCall.ParamObj = null;

                    return true;
                }

                return false;
            });
        }

        public void RemoveAtIndex(int index)
        {
            if (0 == _delayedCalls.Count)
                return;
            
            _delayedCalls.RemoveAll((DelayedCallback currentCall) =>
            {
                if (currentCall.Index == index)
                {
                    currentCall.ObjDelCallback = null;
                    currentCall.Callback = null;
                    currentCall.ParamObj = null;

                    return true;
                }

                return false;
            });
        }

        public void RemoveAll()
        {
            if (0 == _delayedCalls.Count)
                return;
            
            _delayedCalls.RemoveAll((DelayedCallback currentCall) =>
            {
                currentCall.ObjDelCallback = null;
                currentCall.Callback = null;
                currentCall.ParamObj = null;

                return true;
            });
        }
        #endregion
    }

    #region Item Callbacks

    class DelayedCallback
    {
        public float Delay;
        public Action Callback;
        public Action<object[]> ObjDelCallback;
        public object[] ParamObj;
        public bool useRealTime = true;
        public int Index;

        public DelayedCallback(float delay, int idx, Action callback, bool useRealTime = true)
        {
            Delay = delay;
            Index = idx;
            Callback = callback;
            this.useRealTime = useRealTime;
        }

        public DelayedCallback(float delay, int idx, Action<object[]> callback, object[] param, bool useRealTime = true)
        {
            Delay = delay;
            Index = idx;
            ParamObj = param;
            ObjDelCallback = callback;
            this.useRealTime = useRealTime;
        }
    }

    #endregion
}