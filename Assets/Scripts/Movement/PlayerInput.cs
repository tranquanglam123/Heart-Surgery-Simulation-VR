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


        [Header("Movement"), Space(8)]
        public Vector2 getVector2;
        public Vector2 DIR;
        public GameObject Player;
        private PlayerMov motor;
        private float Speed = 10.0f;
        private float timeCount = 0.0f;
        public float rotationSpeed = 100f;
        //private bool isRotating = false;

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

            motor.PlayerMovement(getVector2, DIR);


        }


    }
}

