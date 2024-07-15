using UnityEngine;

namespace ShootGame
{
    public abstract class FrameworkComponent : MonoBehaviour
    {
        protected virtual void Awake()
        {
            GameEntry.RegisterComponent(this);
        }
    }
}