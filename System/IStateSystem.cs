using QFramework;

namespace ShootGame
{
    public interface IStateSystem : ISystem
    {
        BindableProperty<int> killCount { get; }
    }


    public class StateSystem : AbstractSystem, IStateSystem
    {
        public BindableProperty<int> killCount { get; } = new BindableProperty<int>(0);

        protected override void OnInit()
        {
            
        }
    }
}