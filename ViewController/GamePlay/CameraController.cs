using UnityEngine;

namespace ShootGame
{
    public class CameraController : MonoBehaviour
    {
        private Transform playerTransform;

        private void Start()
        {
            playerTransform = GameObject.FindWithTag("Player").transform;
        }

        private void Update()
        {
            
            if (playerTransform != null)
            {
                var camPosition = transform.position;
                camPosition.x = playerTransform.position.x + 3;
                camPosition.y = playerTransform.position.y + 3;

                transform.position = camPosition;
            }
            else return;
        }
    }
}