using UnityEngine;
using UnityEngine.SceneManagement;

namespace ShootGame
{
    //�����ڴ������ϣ����������������ʱ��������һ������
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