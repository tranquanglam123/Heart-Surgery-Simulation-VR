using Oculus.Interaction;
using System;
using UnityEngine;
using VR_Surgery.Scripts.Core;
using VR_Surgery.Scripts.Gameplay;
using VR_Surgery.Scripts.Utilities;
using static VR_Surgery.Scripts.Core.GlobalDefinition;
using static VR_Surgery.Scripts.Core.TypeEnums;
namespace VR_Surgery.Scripts.Manager
{
    public class SurgerySceneManager : MonoBehaviour
    {
       
        void Start()
        {
            InitValue();
        }

        void InitValue()
        {
            try
            {

                // Audio Player
                audioPlayer = new GameObject("BackgroundMusic").AddComponent<AudioSource>();
                audioPlayer.clip = Resources.Load(ResourcesPath.AudioFolder + GlobalDefinition.BackgroundMusic) as AudioClip;
                audioPlayer.loop = true;
                audioPlayer.volume = 0.1f;
                audioPlayer.playOnAwake = false;
                audioPlayer.PlayDelayed(1.5f);

                // Play Mode Handling
                modeExecutionObj = new GameObject("ModeExecution");
                modeExecutionObj.AddComponent<ModeExecution>();

                // Menu Instantiate
                Helper.CreateMenu(GlobalDefinition.StartMenu);
                currentMenu.GetComponentInChildren<PointableUnityEventWrapper>().WhenSelect.AddListener((PointerEvent data) =>
                {
                    Helper.CreateMenu(GlobalDefinition.ModeMenu);
                    var SurgeryWrapper = currentMenu.transform.Find("SurgeryMode").GetComponentInChildren<PointableUnityEventWrapper>();
                    SurgeryWrapper.WhenSelect.AddListener((PointerEvent data) =>
                    {
                        //GlobalDefinition.PlayMode = TypeEnums.OperatingMode.Surgery;
                        modeExecutionObj.GetComponent<ModeExecution>().InitPlayMode(OperatingMode.Surgery);
                        // Debug
                        Helper.CreateMessageMenu($"Play mode set to {GlobalDefinition.PlayMode}");
                    });
                    var TransplantWrapper = currentMenu.transform.Find("Transplant").GetComponentInChildren<PointableUnityEventWrapper>();
                    TransplantWrapper.WhenSelect.AddListener((PointerEvent data) =>
                    {
                        GlobalDefinition.PlayMode = TypeEnums.OperatingMode.Transplant;
                        modeExecutionObj.GetComponent<ModeExecution>().InitPlayMode(OperatingMode.Transplant);
                        //Debug
                        Helper.CreateMessageMenu($"Play mode set to {GlobalDefinition.PlayMode}");
                    });
                });
            }
            catch (Exception e)
            {
                currentMessageMenu = Helper.CreateMessageMenu(e.Message);
            }

        }



        private void Update()
        {
            

        }

    }
}
