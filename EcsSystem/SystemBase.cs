using System.Collections.Generic;

namespace ShootGame
{
    public abstract class SystemBase : IReference
    {
        public LinkedList<Entity> m_entities = new LinkedList<Entity>();
        public abstract void Tick();
        public abstract void Clear();
    }
}
