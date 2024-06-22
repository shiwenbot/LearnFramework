using UnityEngine;

namespace ShootGame
{
    public class TimeSystemTest : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            Debug.Log(Time.time);

            ShootingEditor.Interface.GetSystem<ITimeSystem>()
                .AddDelayTask(3, () =>
                {
                    Debug.Log(Time.time);
                });
        }
    }
}
