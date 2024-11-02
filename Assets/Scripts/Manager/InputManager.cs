using UnityEngine;
using VR_Surgery.Scripts.Movement;

namespace VR_Surgery.Scripts.Manager
{
    /// <summary>
    /// Manager script to get execute movement logics
    /// </summary>
    public class InputManager : MonoBehaviour
    {

        public GameObject menu;
        public Vector2 DIR;
        public Camera map;


        private void FixedUpdate()
        {
            GetInput();
        }

        private void Update()
        {

            if (OVRInput.GetDown(OVRInput.Button.One))
            {

            }

            if (OVRInput.GetDown(OVRInput.Button.Two))
            {

            }

            if (OVRInput.GetDown(OVRInput.Button.Three))
            {

            }

            if (OVRInput.GetDown(OVRInput.Button.Four))
            {

            }

        }

        private void GetInput()
        {

            DIR = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);
            PlayerInput.Instance.getVector2 = DIR.normalized;
            //return DIR.normalized;
        }
    }
}

