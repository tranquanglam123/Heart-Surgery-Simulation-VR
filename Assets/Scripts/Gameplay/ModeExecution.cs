using System.Collections;
using UnityEngine;
using VR_Surgery.Scripts.Core;
using static VR_Surgery.Scripts.Core.TypeEnums;
using static VR_Surgery.Scripts.Core.GlobalDefinition;
using System;
using VR_Surgery.Scripts.Utilities;
using UnityEngine.Rendering;
using System.Linq;
using System.Collections.Generic;
using TMPro;
using Oculus.Interaction;
namespace VR_Surgery.Scripts.Gameplay
{

    public class ModeExecution : MonoBehaviour
    {
        Collider hitBox;
        Collider knife;
        Collider stretchTool;
        Collider stretchTool2nd;
        //InteractableUnityEventWrapper liningWrapper;
        private Vector3 spawnPos = new Vector3(0.0710000172f, -0.498999953f, -0.681000054f);
        private Vector3 stretchToolEndPos = new Vector3(0.3773f, 1.7394f, 0.119999997f);
        private Vector3 stretchTool2ndEndPos = new Vector3(0.0363f, 1.7394f, 0.119999997f);
        private ModePhase modePhase = ModePhase.Idle;
        private List<AnimationClip> animationClips;
        float duration = 1.2f;
        public ModePhase ModePhase { get { return modePhase; } set { modePhase = value; } }

        private Coroutine currentCoroutine;
        private bool tool1InPos = false;
        private bool tool2InPos = false;
        private void Awake()
        {
            try
            {
                hitBox = GameObject.Find("HitBox").GetComponent<Collider>();
                knife = GameObject.Find("Knife").GetComponent<Collider>();
                stretchTool = GameObject.Find("StretchTool").GetComponent<Collider>();
                stretchTool2nd = GameObject.Find("StretchTool2nd").GetComponent<Collider>();
                //liningWrapper = GameObject.Find("Daynoi").GetComponent<InteractableUnityEventWrapper>();
            }
            catch (Exception ex) 
            {
                Helper.CreateMessageMenu(ex.Message);
            }
        }
        public void InitPlayMode(OperatingMode playmode)
        {
            currentMenu.SetActive(false);
            tool1InPos = false;
            tool2InPos = false;
            GlobalDefinition.PlayMode = playmode;
            patientObj = GameObject.Find("AllAnimation").gameObject;
            patientObj.GetComponent<Animation>().wrapMode = WrapMode.ClampForever;
            patientObj.GetComponent<Animation>().Play();
            heartObj = GameObject.Find("heart-and-lung-animation");
            heartObj.GetComponent<Animation>().wrapMode = WrapMode.Loop;
            heartObj.GetComponent<Animation>().Play();

            animationClips = gameObject.GetComponent<AnimationHelper>().GetAnimationClipsFromImporter(patientObj.GetComponent<Animation>()).ToList();
            switch (playmode)
            {
                case OperatingMode.Transplant:
                    this.modePhase = ModePhase.TransplantIdle;
                    GameObject.Find("pasted__pasted__Heart_Animated_Master").SetActive(false);
                    patientHeartObj = GameObject.Find("pasted__pasted__Heart_Animated_MasterOld");;
                    break;
                case OperatingMode.Surgery:
                    this.modePhase = ModePhase.Idle;
                    heartObj.SetActive(false);
                    GameObject.Find("pasted__pasted__Heart_Animated_MasterOld").SetActive(false);
                    //patientHeartObj = GameObject.Find("pasted__pasted__Heart_Animated_Master");
                    //patientHeartObj.GetComponent<Animation>().wrapMode = WrapMode.Loop;
                    break;

            }
        }

        // Update is called once per frame
        void Update()
        {
            if(GlobalDefinition.PlayMode == TypeEnums.OperatingMode.Surgery)
            {
                SurgeryModeExecution();
            }
            if(GlobalDefinition.PlayMode == TypeEnums.OperatingMode.Transplant)
            {
                TransplantModeExecution();
            }
        }

        void SurgeryModeExecution()
        {
            switch (this.modePhase) 
            {
                case ModePhase.Idle:
                    if (knife.bounds.Intersects(hitBox.bounds))
                    {
                        this.modePhase = ModePhase.Cut;
                    }
                    break;
                case ModePhase.Cut:
                    currentCoroutine ??= StartCoroutine(PhaseExecute(WrapMode.ClampForever));
                    if (!tool1InPos && stretchTool.bounds.Intersects(hitBox.bounds))
                    {
                        stretchTool.transform.position = new Vector3(0.3131f, 1.7394f, 0.119999997f);
                        stretchTool.transform.rotation = Quaternion.Euler(176.947f, 90f, 0f);
                        tool1InPos = true;
                    }
                    if (!tool2InPos && stretchTool2nd.bounds.Intersects(hitBox.bounds))
                    {
                        stretchTool2nd.transform.position = new Vector3(0.0879999995f, 1.7394f, 0.119999997f);
                        stretchTool2nd.transform.rotation = Quaternion.Euler(4.65488195f, 90f, 180f); 
                        tool2InPos = true;
                    }
                    if(tool1InPos && tool2InPos)
                    {
                        StopCurrentCoroutine();
                        this.modePhase = ModePhase.Stretch;
                        Debug.Log("Touched Stretch Tool");
                    }
                    break;
                case ModePhase.Stretch:
                    currentCoroutine ??= StartCoroutine(PhaseExecute(WrapMode.ClampForever));
                    break;
            }

        }
        void TransplantModeExecution()
        {
            switch (this.modePhase)
            {
                case ModePhase.TransplantIdle:
                    patientObj.GetComponent<Animation>().clip = animationClips.ToList().Find(x => x.name == "TransplantIdle");
                    patientObj.GetComponent<Animation>().wrapMode = WrapMode.ClampForever;
                    patientObj.GetComponent<Animation>().Play();
                    this.modePhase = ModePhase.HeartOut;
                    break;
                case ModePhase.HeartOut:

                    break;
                case ModePhase.HeartIn:
                    break;
            }
            
        }
        
        IEnumerator PhaseExecute(WrapMode animationWrapMode)
        {
            yield return new WaitForSeconds(1f);
            patientObj.GetComponent<Animation>().wrapMode = animationWrapMode;
            patientObj.GetComponent<Animation>().clip = animationClips.ToList().Find(x => x.name == modePhase.ToString());
            patientObj.GetComponent<Animation>().Play();
            if (patientObj.GetComponent<Animation>().clip.name == ModePhase.Stretch.ToString())
            {
                Vector3 startPosition = stretchTool.transform.position;
                Vector3 start2Position = stretchTool2nd.transform.position;
                float elapsedTime = 0;
                while (elapsedTime < 0.65f) {
                    elapsedTime += Time.deltaTime;
                    yield return null;
                }
                elapsedTime = 0;
                while (elapsedTime < patientObj.GetComponent<Animation>().clip.length)
                {
                    stretchTool.transform.position = Vector3.Lerp(startPosition, stretchToolEndPos, elapsedTime / duration);
                    stretchTool2nd.transform.position = Vector3.Lerp(start2Position, stretchTool2ndEndPos, elapsedTime / duration);
                    elapsedTime += Time.deltaTime;
                    yield return null;
                }

                // Ensure the final position is set
                stretchTool.transform.position = stretchToolEndPos;
                stretchTool2nd.transform.position = stretchTool2ndEndPos;
            }
        }

        private void StopCurrentCoroutine()
        {
            if (currentCoroutine != null)
            {
                StopCoroutine(currentCoroutine);
                currentCoroutine = null;
            }
        }

    }
}

