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
namespace VR_Surgery.Scripts.Gameplay
{

    public class ModeExecution : MonoBehaviour
    {
        Collider hitBox;
        Collider knife;
        Collider stretchTool;
        Collider stretchTool2nd;
        private Vector3 spawnPos = new Vector3(0.0710000172f, -0.498999953f, -0.681000054f);
        private ModePhase modePhase = ModePhase.Idle;
        private List<AnimationClip> animationClips;
        public ModePhase ModePhase { get { return modePhase; } set { modePhase = value; } }

        private Coroutine currentCoroutine;
        private void Awake()
        {
            try
            {
                hitBox = GameObject.Find("HitBox").GetComponent<Collider>();
                knife = GameObject.Find("Knife").GetComponent<Collider>();
                stretchTool = GameObject.Find("StretchTool").GetComponent<Collider>();
                stretchTool2nd = GameObject.Find("StretchTool2nd").GetComponent<Collider>();
            }
            catch (Exception ex) 
            {
                Helper.CreateMessageMenu(ex.Message);
            }
        }
        public void InitPlayMode(OperatingMode playmode)
        {
            currentMenu.SetActive(false);
            GlobalDefinition.PlayMode = playmode;
            this.modePhase = ModePhase.Idle;
            patientObj = GameObject.Find("AllAnimation").gameObject;
            patientObj.GetComponent<Animation>().wrapMode = WrapMode.ClampForever;
            patientObj.GetComponent<Animation>().Play();
            animationClips = gameObject.GetComponent<AnimationHelper>().GetAnimationClipsFromImporter(patientObj.GetComponent<Animation>()).ToList();
            switch (playmode)
            {
                case OperatingMode.Transplant:
                    GameObject.Find("Cut").SetActive(false);
                    heartObj = GameObject.Find("heart-and-lung-animation");
                    heartObj.GetComponent<Animation>().wrapMode = WrapMode.Loop;
                    heartObj.GetComponent<Animation>().Play();
                    break;
                case OperatingMode.Surgery:
                    heartObj = GameObject.Find("heart-and-lung-animation");
                    heartObj.SetActive(false);
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
                    if (stretchTool.bounds.Intersects(hitBox.bounds))
                    {
                        StopCurrentCoroutine();
                        this.modePhase = ModePhase.Stretch;
                        Debug.Log("Touched Stretch Tool");
                    }
                    break;
                case ModePhase.Stretch:
                    currentCoroutine ??= StartCoroutine(PhaseExecute(WrapMode.ClampForever));
                    if (knife.bounds.Intersects(hitBox.bounds))
                    {
                        StopCurrentCoroutine();
                        this.modePhase = ModePhase.Lining;
                    }
                    break;
                case ModePhase.Lining:
                    Debug.Log("Moved to Lining");

                    break;
            }

        }
        void TransplantModeExecution()
        {
            switch (this.modePhase)
            {
                case ModePhase.Idle:

                    patientObj.GetComponent<Animation>().clip = animationClips.ToList().Find(x => x.name == "Lining");
                    patientObj.GetComponent<Animation>().wrapMode = WrapMode.ClampForever;
                    patientObj.GetComponent<Animation>().Play();
                    this.modePhase = ModePhase.Cut;
                    break;
                case ModePhase.Cut:
                    break;
                case ModePhase.Stretch:
                    break;
                case ModePhase.Lining:
                    Debug.Log("Moved to Lining");

                    break;
            }
            
        }
        
        IEnumerator PhaseExecute(WrapMode animationWrapMode)
        {
            yield return new WaitForSeconds(1f);
            patientObj.GetComponent<Animation>().wrapMode = animationWrapMode;
            patientObj.GetComponent<Animation>().clip = animationClips.ToList().Find(x => x.name == modePhase.ToString());
            patientObj.GetComponent<Animation>().Play();
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

