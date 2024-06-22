using System;

namespace ShootGame
{
    public enum DelayTaskState
    {
        NotStarted,
        Started,
        Finished
    }

    public class DelayTask
    {
        public float Seconds { get; set; } // 这个任务的持续时间
        public float StartSeconds { get; set; }
        public float FinishSeconds { get; set; }
        public DelayTaskState State { get; set; }
        public Action OnFinish { get; set; }
    }
}