using UnityEngine;

namespace VR_Surgery.Scripts.Movement
{
    /// <summary>
    /// Manage the user's POV
    /// </summary>
    public class PlayerLook : MonoBehaviour
    {
        public Camera cam;
        private float xRotation = 0f;

        public float xSensitivity = 30f;
        public float ySensitivity = 30f;
        // Start is called before the first frame update

        public void ProcessLook(Vector2 input)
        {
            Debug.Log(" x = " + input);
            float mouseX = input.x;
            float mouseY = input.y;
            xRotation = (mouseY * Time.deltaTime) * ySensitivity;
            xRotation = Mathf.Clamp(xRotation, -80f, 80f);
            transform.Rotate(Vector3.up * (mouseX * Time.deltaTime) * xSensitivity);
        }
    }
}

