using UnityEngine;

namespace VR_Surgery.Scripts.Movement
{
    /// <summary>
    /// Manage the Player's movement
    /// </summary>
    public class PlayerMov : MonoBehaviour
    {
        private CharacterController playerController;
        private Vector3 playerVelocity;

        //public Transform pointOfPlayer;

        public float speed;
        public float maxSpeed = 30f;
        public float initialSpeed = 5f;
        public float run = 5f;
        public float timeCount;
        float rotationSpeed = 100f;
        private bool in_Grounded;
        public float gravity = -9.8f;
        public float jumpHeight = 3f;

        void Start()
        {
            playerController = GetComponent<CharacterController>();
        }

        void Update()
        {
            in_Grounded = playerController.isGrounded;
        }
        public void PlayerMovement(Vector2 input, Vector2 rotation)
        {

            Vector3 moveDirection = Vector3.zero;
            moveDirection.x = input.x;
            moveDirection.z = input.y;

            if (PlayerInput.Instance.getVector2.y > 0.7f)
            {

                if (timeCount < run)
                {
                    timeCount += Time.deltaTime;
                    speed = Mathf.Lerp(initialSpeed, maxSpeed, timeCount / run);
                }

                else
                {
                    speed = maxSpeed;
                }

            }

            else
            {
                speed = initialSpeed;
                timeCount = 0f;
            }

            playerController.Move(transform.TransformDirection(moveDirection) * speed * Time.deltaTime);
            float rotationAmount = rotation.x * rotationSpeed * Time.deltaTime;
            playerController.transform.Rotate(Vector3.up, rotationAmount);
        }

    }

}
