using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootGame
{
    public abstract class BuffBase : IReference
    {
        public BuffState m_buffState { get; set; } = BuffState.InActive;
        public abstract void OnInit(); //新建的时候调用一次
        public abstract void OnEnter(); //每次挂buff的时候调用，如诺手A了对面一下
        public abstract void OnUpdate(float deltatime); //如果状态为active，就会一直被调用
        public abstract void OnExit(); //会被OnUpdate调用，用于重置buff状态，等待下一次使用
        public abstract void OnDestroy(); //销毁的时候调用
        public abstract void OnRefresh(); //处理多个同名buff叠加的情况
        public abstract void Clear();
        
    }
}
