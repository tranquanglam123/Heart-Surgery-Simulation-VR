using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Windows;
using VR_Surgery.Scripts.Movement;

namespace VR_Surgery.Scripts.Manager
{
    /// <summary>
    /// Manager script to get execute movement logics
    /// </summary>
    public class InputManager : MonoBehaviour
    {
        [SerializeField, Tooltip("The Player gameobject to control the movement")]
        GameObject Player;
        public float speed = 5f;
        public float rotationSpeed = 100f;

        #region Parameters
        [SerializeField] float walkSpeed;
        [SerializeField] float maxSpeed = 30f;
        [SerializeField] float initialSpeed = 5f;
        [SerializeField] float runSpeed = 5f;
        [SerializeField] float timeCount;

        [SerializeField] bool in_Grounded;
        [SerializeField] float gravity = -9.8f;
        [SerializeField] float jumpHeight = 3f;
        #endregion

        private Vector2 DIR;
        private Vector2 forwardMovement;
        //private PlayerMov playerMovement;
        //private PlayerLook playerLook;

        private void Start()
        {
            //    playerMovement = FindAnyObjectByType<PlayerMov>();
            //    playerLook = FindAnyObjectByType<PlayerLook>();
        }
        private void FixedUpdate()
        {
            GetInput();
        }

        private void Update()
        {
        }

        private void GetInput()
        {

            forwardMovement = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.LTouch);
            DIR = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.RTouch);
            //forwardMovement = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.LTouch);
            PlayerInput.Instance.getVector2 = forwardMovement.normalized;
            PlayerInput.Instance.DIR =DIR.normalized;
        }


    }
}

