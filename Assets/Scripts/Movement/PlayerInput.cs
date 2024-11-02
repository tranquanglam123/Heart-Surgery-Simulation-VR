using System;
using UnityEngine;
using VR_Surgery.Scripts.Core;
namespace VR_Surgery.Scripts.Movement
{

    public class PlayerInput : Singleton<PlayerInput>
    {
        [Header("AI Voice Instruction "), Space(8)]
        public AudioClip descriptionVoice;
        public bool playVoiceBool = false;

        [Header("Hand and Controller"), Space(8)]
        public bool controllerIsUse;
        public GameObject controller;
        public GameObject hand;
        public GameObject controllerMenu;
        public GameObject handMenu;


        [Header("Movement"), Space(8)]
        public Vector2 getVector2;
        public GameObject Player;
        private PlayerMov motor;
        private float Speed = 10.0f;
        private float timeCount = 0.0f;
        private bool isRotating = false;

        private GameObject MainCamera;


        private PlayerLook look;
        
        void Awake()
        {

            motor = GetComponent<PlayerMov>();
            look = GetComponent<PlayerLook>();
            MainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            motor.PlayerMovement(getVector2);
            //
            //gameObject.transform.position = Camera.main.transform.position;
            //motor.PlayerMovement(getVector2);
        }

        private void Update()
        {
            SwapHandAndController();
        }

        void LateUpdate()
        {

            //if (isRotating) SetRotate(Player, MainCamera);
            SetRotate(MainCamera, Player);
            SetRotate(Player, MainCamera);
            //if (Player.transform.rotation.eulerAngles.y == Player.transform.rotation.eulerAngles.y)
            //    isRotating = !isRotating;
        }

        void SetRotate(GameObject toRotate, GameObject camera)
        {
            float newVector = toRotate.transform.rotation.x;
            //You can call this function for any game object and any camera, just change the parameters when you call this function
            Player.transform.rotation = Quaternion.Lerp(toRotate.transform.rotation, camera.transform.rotation, Speed * timeCount);
            Player.transform.rotation = Quaternion.Euler(0, MainCamera.transform.rotation.eulerAngles.y, 0);
            timeCount = timeCount + Time.deltaTime;
        }

        private void SwapHandAndController()
        {
            throw new NotImplementedException();
        }

        private void SetActive()
        {
            controller.SetActive(controllerIsUse);
            hand.SetActive(!controllerIsUse);

            controllerMenu.SetActive(controllerIsUse);
            handMenu.SetActive(!controllerIsUse);

        }

    }
}

