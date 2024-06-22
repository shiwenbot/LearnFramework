using QFramework;
using UnityEngine.SceneManagement;

namespace ShootGame
{
    public class HurtPlayerCommand : AbstractCommand
    {
        protected override void OnExecute()
        {
            var playerModel = this.GetModel<IPlayerModel>();

            playerModel.HP.Value--;

            if (playerModel.HP.Value <= 0)
            {
                SceneManager.LoadScene("GameOver");
            }
        }
    }
}
