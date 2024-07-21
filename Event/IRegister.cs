using System;

namespace ShootGame
{
    public interface IRegister
    {

    }

    public class RegisterFunc : IRegister
    {
        public Action m_callBack = null;
    }

    public class RegisterFunc<T> : IRegister
    {
        public Action<T> m_callBack = null;

    }

    public class RegisterFunc<T, K> : IRegister
    {
        public Action<T, K> m_callBack = null;

    }

}