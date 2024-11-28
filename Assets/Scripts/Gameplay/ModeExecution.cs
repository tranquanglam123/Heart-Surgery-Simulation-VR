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
    /// <summary>
    /// Handle the progess of modes
    /// </summary>
    public class ModeExecution : MonoBehaviour
    {
        Collider hitBox;
        Collider knife;
        Collider stretchTool;
        Collider stretchTool2nd;
        private ModePhase modePhase = ModePhase.Idle;
        public ModePhase ModePhase { get { return modePhase; } set { modePhase = value; } }

        private Coroutine currentCoroutine;
        private void Awake()
        {
            try
            {
                hitBox = GameObject.Find(HitBox).GetComponent<Collider>();
                knife = GameObject.Find(Knife).GetComponent<Collider>();
                stretchTool = GameObject.Find(StretchTool).GetComponent<Collider>();
                stretchTool2nd = GameObject.Find(StretchTool2nd).GetComponent<Collider>();
            }
            catch (Exception ex) 
            {
                Helper.CreateMessageMenu(ex.Message);
            }
        }
        public void InitPlayMode(OperatingMode playmode)
        {
            currentMenu.SetActive(false);
            Tool1InPos = false;
            Tool2InPos = false;
            GlobalDefinition.PlayMode = playmode;
            patientObj = GameObject.Find(PatientObjName).gameObject;
            patientObj.GetComponent<Animation>().wrapMode = WrapMode.ClampForever;
            patientObj.GetComponent<Animation>().Play();
            heartObj = GameObject.Find(TransplantHeartObjName);
            heartObj.GetComponent<Animation>().wrapMode = WrapMode.Loop;
            heartObj.GetComponent<Animation>().Play();

            AnimationClips = gameObject.GetComponent<AnimationHelper>().GetAnimationClipsFromImporter(patientObj.GetComponent<Animation>()).ToList();
            switch (playmode)
            {
                case OperatingMode.Transplant:
                    this.modePhase = ModePhase.TransplantIdle;
                    GameObject.Find(PatientHeartObjName).SetActive(false);
                    patientHeartObj = GameObject.Find(PatientHeartOldObjName);
                    break;
                case OperatingMode.Surgery:
                    this.modePhase = ModePhase.Idle;
                    heartObj.SetActive(false);
                    GameObject.Find(PatientHeartOldObjName).SetActive(false);
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
        /// <summary>
        /// Handle the Surgery mode
        /// </summary>
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
                    if (!Tool1InPos && stretchTool.bounds.Intersects(hitBox.bounds))
                    {
                        stretchTool.transform.position = new Vector3(0.3131f, 1.7394f, 0.119999997f);
                        stretchTool.transform.rotation = Quaternion.Euler(176.947f, 90f, 0f);
                        Tool1InPos = true;
                    }
                    if (!Tool2InPos && stretchTool2nd.bounds.Intersects(hitBox.bounds))
                    {
                        stretchTool2nd.transform.SetPositionAndRotation(new Vector3(0.0879999995f, 1.7394f, 0.119999997f), Quaternion.Euler(4.65488195f, 90f, 180f));
                        Tool2InPos = true;
                    }
                    if(Tool1InPos && Tool2InPos)
                    {
                        StopCurrentCoroutine();
                        this.modePhase = ModePhase.Stretch;
                    }
                    break;
                case ModePhase.Stretch:
                    currentCoroutine ??= StartCoroutine(PhaseExecute(WrapMode.ClampForever));
                    break;
            }

        }
        /// <summary>
        /// Handle the Transplant Mode
        /// </summary>
        void TransplantModeExecution()
        {
            switch (this.modePhase)
            {
                case ModePhase.TransplantIdle:
                    patientObj.GetComponent<Animation>().clip = AnimationClips.ToList().Find(x => x.name == this.modePhase.ToString());
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
        
        /// <summary>
        /// Play the animation of the current phase
        /// </summary>
        /// <param name="animationWrapMode">Wrapemode for the animationn, EG: loop or clamp(play and keep last frame)</param>
        /// <returns></returns>
        IEnumerator PhaseExecute(WrapMode animationWrapMode)
        {
            yield return new WaitForSeconds(1f);
            patientObj.GetComponent<Animation>().wrapMode = animationWrapMode;
            patientObj.GetComponent<Animation>().clip = AnimationClips.ToList().Find(x => x.name == modePhase.ToString());
            patientObj.GetComponent<Animation>().Play();

            // Extend animation only for the Stretch Phase
            // Move 2 stretch tools along with the cut 
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
                    stretchTool.transform.position = Vector3.Lerp(startPosition, StretchToolEndPos, elapsedTime / duration);
                    stretchTool2nd.transform.position = Vector3.Lerp(start2Position, StretchTool2ndEndPos, elapsedTime / duration);
                    elapsedTime += Time.deltaTime;
                    yield return null;
                }

                // Ensure the final position is set
                stretchTool.transform.position = StretchToolEndPos;
                stretchTool2nd.transform.position = StretchTool2ndEndPos;
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

