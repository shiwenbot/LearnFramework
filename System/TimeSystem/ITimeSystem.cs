using QFramework;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace ShootGame
{
    public interface ITimeSystem : ISystem
    {
        float currenSeconds { get; }
        void AddDelayTask(float delaySeconds, Action callback);
    }

    public class TimeSystem : AbstractSystem, ITimeSystem
    {
        public float currenSeconds { get; private set; }
        private LinkedList<DelayTask> mDelayTasks = new LinkedList<DelayTask>();

        protected override void OnInit()
        {
            var gameObject = new GameObject("TimeSystemMono");
            UnityEngine.Object.DontDestroyOnLoad(gameObject); //防止切换场景的时候被销毁
            gameObject.AddComponent<TimeSystemMono>().OnUpdate += Update;
            currenSeconds = 0;
        }
        //这个update会被TimeSystemMono的Update调用(在OnInit注册的)
        private void Update()
        {
            currenSeconds += Time.deltaTime;
            if(mDelayTasks.Count > 0 )
            {
                var cur = mDelayTasks.First;
                //遍历所有的延迟任务
                while ( cur != null )
                {
                    var next = cur.Next;
                    //开始任务
                    if (cur.Value.State == DelayTaskState.NotStarted)
                    {
                        cur.Value.State = DelayTaskState.Started;
                        cur.Value.StartSeconds = currenSeconds;
                        cur.Value.FinishSeconds = currenSeconds + cur.Value.Seconds;
                    }
                    //判断是否结束任务
                    else if (cur.Value.State == DelayTaskState.Started)
                    {
                        if (currenSeconds >= cur.Value.FinishSeconds)
                        {
                            cur.Value.State = DelayTaskState.Finished;
                            cur.Value.OnFinish?.Invoke();
                            mDelayTasks.Remove(cur);
                        }
                    }
                    cur = next;
                }
            }
        }

        public void AddDelayTask(float delaySeconds, Action callback)
        {
            DelayTask delay = new DelayTask()
            {
                Seconds = delaySeconds,
                OnFinish = callback,
                State = DelayTaskState.NotStarted
            };
            mDelayTasks.AddLast(delay);
        }
    }
    public class TimeSystemMono : MonoBehaviour
    {
        public event Action OnUpdate;
        private void Update()
        {
            OnUpdate?.Invoke();
        }
    }
}
