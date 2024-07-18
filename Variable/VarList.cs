using System.Collections.Generic;

namespace ShootGame
{
    public sealed class VarList<T> : Variable<List<T>>
    {
        public VarList() { }
        /// <summary>
        /// 隐式转换，在把List<T>类赋值给VarList<T>的时候调用
        /// </summary>
        /// <param name="list"></param>
        public static implicit operator VarList<T>(List<T> list)
        {
            VarList<T> m_list = ReferencePool.Acquire<VarList<T>>();
            m_list.Value = list;
            return m_list;
        }

        public static implicit operator List<T>(VarList<T> value)
        {
            return value.Value;
        }
    }
}