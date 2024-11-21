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
    public class SurgerySceneManager : MonoBehaviour
    {
        public GameObject spawnHolder;
        public GameObject spawnPrefab;
        private Vector3 spawnPos = new Vector3(0.1906184f, 1.373776f, 0.6312699f);
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

                patientObj = GameObject.Find("PatientAllStep").gameObject;
                //patientObj.GetComponent<Animation>().wrapMode = WrapMode.ClampForever;
                //patientObj.GetComponent<Animation>().Play();    
                patientObj.transform.Find("BodyRaw").gameObject.SetActive(true);
                patientObj.transform.Find("Body").gameObject.SetActive(false);
                patientObj.transform.Find("Lining").gameObject.SetActive(false);
                // Play Mode Handling
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



        private void Update()
        {


        }

        IEnumerator DelayAction(UnityAction callback)
        {
            yield return new WaitForSeconds(2F);
            callback();
        }

        //IEnumerator TestAnimation(GameObject animationComponent)
        //{
        //    yield return new WaitForSeconds(5F);
        //    Debug.Log("Changing state");
        //    modeExecutionObj.GetComponent<ModeExecution>().ModePhase = ModePhase.Cut;
        //    yield return new WaitForSeconds(patient);
        //    modeExecutionObj.GetComponent<ModeExecution>().ModePhase = ModePhase.Stretch;
        //    //animationComponent.GetComponent<Animation>().Stop();
        //    //animationComponent.transform.Find("BodyRaw").gameObject.SetActive(false);
        //    //animationComponent.transform.Find("Body").gameObject.SetActive(true);
        //    //animationComponent.GetComponent<Animation>().Play(ModePhase.Cut.ToString());
        //    //yield return new WaitForSeconds(animationComponent.GetComponent<Animation>().clip.length + 1f);
        //    //animationComponent.GetComponent<Animation>().Play(ModePhase.Stretch.ToString());
        //}

    }
}
