using UnityEngine;
using UnityEngine.SceneManagement;

namespace ShootGame
{
    //挂载在传送门上，当玩家碰到传送门时，加载下一个场景
    public class NextLevel : MonoBehaviour
    {
        public string levelName;
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                SceneManager.LoadScene(levelName);
            }
        }
    }
}