using System.Collections.Generic;
using System;

namespace ShootGame
{

    public class EventManager : SingleTonBase<EventManager>
    {
        // 我们没办法在容器声明的时候声明Action<T>/List<Action<T>>，这种写法代码编辑器下就会报错了，所以需要通过接口，我们封装一层，这也是设计模式中的里氏替换原则，用父类装子类
        private Dictionary<string, IRegister> m_eventDic = new Dictionary<string, IRegister>();

        #region 注册事件

        public void RegisterEvent<T>(string eventName, Action<T> callBack)
        {
            if (m_eventDic.ContainsKey(eventName))
            {
                (m_eventDic[eventName] as RegisterFunc<T>).m_callBack += callBack;
            }
            else
            {
                m_eventDic[eventName] = new RegisterFunc<T>();
                (m_eventDic[eventName] as RegisterFunc<T>).m_callBack = callBack;
            }
        }

        public void RegisterEvent<T, K>(string eventName, Action<T, K> callBack)
        {
            if (m_eventDic.ContainsKey(eventName))
            {
                (m_eventDic[eventName] as RegisterFunc<T, K>).m_callBack += callBack;
            }
            else
            {
                m_eventDic[eventName] = new RegisterFunc<T, K>();
                (m_eventDic[eventName] as RegisterFunc<T, K>).m_callBack = callBack;
            }
        }

        #endregion

        #region 销毁事件

        public void UnRegisterEvent<T>(string eventName, Action<T> callBack)
        {
            if (m_eventDic.ContainsKey(eventName))
            {
                if ((m_eventDic[eventName] as RegisterFunc<T>).m_callBack != null)
                {
                    (m_eventDic[eventName] as RegisterFunc<T>).m_callBack -= callBack;
                }
            }
        }

        public void UnRegisterEvent<T, K>(string eventName, Action<T, K> callBack)
        {
            if (m_eventDic.ContainsKey(eventName))
            {
                if ((m_eventDic[eventName] as RegisterFunc<T, K>).m_callBack != null)
                {
                    (m_eventDic[eventName] as RegisterFunc<T, K>).m_callBack -= callBack;
                }
            }
        }

        #endregion

        #region 发送事件

        public void SendEvent<T>(string eventName, T parm)
        {
            IRegister func = null;
            if (m_eventDic.TryGetValue(eventName, out func))
            {
                (func as RegisterFunc<T>).m_callBack?.Invoke(parm);
            }
        }

        public void SendEvent<T, K>(string eventName, T parm1, K parm2)
        {
            IRegister func = null;
            if (m_eventDic.TryGetValue(eventName, out func))
            {
                (func as RegisterFunc<T, K>).m_callBack?.Invoke(parm1, parm2);
            }
        }

        #endregion
    }
}