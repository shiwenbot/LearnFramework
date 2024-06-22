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
            UnityEngine.Object.DontDestroyOnLoad(gameObject); //��ֹ�л�������ʱ������
            gameObject.AddComponent<TimeSystemMono>().OnUpdate += Update;
            currenSeconds = 0;
        }
        //���update�ᱻTimeSystemMono��Update����(��OnInitע���)
        private void Update()
        {
            currenSeconds += Time.deltaTime;
            if(mDelayTasks.Count > 0 )
            {
                var cur = mDelayTasks.First;
                //�������е��ӳ�����
                while ( cur != null )
                {
                    var next = cur.Next;
                    //��ʼ����
                    if (cur.Value.State == DelayTaskState.NotStarted)
                    {
                        cur.Value.State = DelayTaskState.Started;
                        cur.Value.StartSeconds = currenSeconds;
                        cur.Value.FinishSeconds = currenSeconds + cur.Value.Seconds;
                    }
                    //�ж��Ƿ��������
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
