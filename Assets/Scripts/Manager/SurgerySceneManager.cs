using Oculus.Interaction;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using VR_Surgery.Scripts.Core;
using VR_Surgery.Scripts.Gameplay;
using VR_Surgery.Scripts.Utilities;
using static VR_Surgery.Scripts.Core.GlobalDefinition;
using static VR_Surgery.Scripts.Core.TypeEnums;
namespace VR_Surgery.Scripts.Manager
{
    /// <summary>
    /// Main script to run the application
    /// Manage the initial process of the whole application
    /// </summary>
    public class SurgerySceneManager : MonoBehaviour
    {
        [Tooltip ("Parent Object for spawn")]
        public GameObject spawnHolder;
        [Tooltip("The Spawn object prefab")]
        public GameObject spawnPrefab;

        /// <summary>
        /// Once the application start running, this function will
        /// be automatically called
        /// </summary>
        void Start()
        {
            InitValue();
        }

        /// <summary>
        /// Assign the default values for the application
        /// </summary>
        void InitValue()
        {
            try
            {
                GlobalDefinition.PlayMode = OperatingMode.Null;

                // Audio Player
                audioPlayer = new GameObject("BackgroundMusic").AddComponent<AudioSource>();
                audioPlayer.clip = Resources.Load(ResourcesPath.AudioFolder + GlobalDefinition.BackgroundMusic) as AudioClip;
                audioPlayer.loop = true;
                audioPlayer.volume = 0.1f;
                audioPlayer.playOnAwake = false;
                audioPlayer.PlayDelayed(1.5f);

                // Menu Instantiate
                Helper.CreateMenu(GlobalDefinition.StartMenu);
                StartCoroutine(DelayAction(() =>
                {
                    currentMenu.GetComponentInChildren<HeadFollow>().enabled = false;
                }));
                currentMenu.GetComponentInChildren<PointableUnityEventWrapper>().WhenSelect.AddListener((PointerEvent data) =>
                {
                    Helper.CreateMenu(GlobalDefinition.ModeMenu);
                    StartCoroutine(DelayAction(() =>
                    {
                        currentMenu.GetComponentInChildren<HeadFollow>().enabled = false;
                    }));
                    var SurgeryWrapper = currentMenu.transform.Find("SurgeryMode").GetComponentInChildren<PointableUnityEventWrapper>();
                    SurgeryWrapper.WhenSelect.AddListener((PointerEvent data) =>
                    {
                        //GlobalDefinition.PlayMode = TypeEnums.OperatingMode.Surgery;
                        modeExecutionObj.GetComponent<ModeExecution>().InitPlayMode(OperatingMode.Surgery);
                        // Debug
                        //Helper.CreateMessageMenu($"Play mode set to {GlobalDefinition.PlayMode}");
                    });
                    var TransplantWrapper = currentMenu.transform.Find("TransplantMode").GetComponentInChildren<PointableUnityEventWrapper>();
                    TransplantWrapper.WhenSelect.AddListener((PointerEvent data) =>
                    {
                        GlobalDefinition.PlayMode = TypeEnums.OperatingMode.Transplant;
                        modeExecutionObj.GetComponent<ModeExecution>().InitPlayMode(OperatingMode.Transplant);
                        //Debug
                        //Helper.CreateMessageMenu($"Play mode set to {GlobalDefinition.PlayMode}");
                    });
                });

                modeExecutionObj = new GameObject("ModeExecution");
                modeExecutionObj.AddComponent<ModeExecution>();
                modeExecutionObj.AddComponent<AnimationHelper>();
#if UNITY_EDITOR
                modeExecutionObj.GetComponent<ModeExecution>().InitPlayMode(OperatingMode.Surgery);
#endif
            }
            catch (Exception e)
            {
                currentMessageMenu = Helper.CreateMessageMenu(e.Message);
            }

}
        /// <summary>
        /// Ultilities to delay an action
        /// </summary>
        /// <param name="callback"> The action needs to be delayed</param>
        /// <returns></returns>
        IEnumerator DelayAction(UnityAction callback)
        {
            yield return new WaitForSeconds(2F);
            callback();
        }

        
    }
}
