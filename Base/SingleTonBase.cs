using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootGame
{
    public class SingleTonBase<T> where T : new()
    {
        private static T m_instance;

        public static T Instance
        {
            get
            {
                if (m_instance == null)
                {
                    m_instance = new T();
                }

                return m_instance;
            }
        }
    }
}
