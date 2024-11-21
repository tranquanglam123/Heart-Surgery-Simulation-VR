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
        private ModePhase modePhase = ModePhase.Idle;
        private List<AnimationClip> animationClips;
        public ModePhase ModePhase { get { return modePhase; } set { modePhase = value; } }
        private void Awake()
        {
            try
            {
                hitBox = patientObj.transform.Find("HitBox").GetComponent<Collider>();
                knife = GameObject.Find("Scissors_T1").GetComponent<Collider>();
                stretchTool = GameObject.Find("Scissors_T2").GetComponent<Collider>();
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
            animationClips = gameObject.GetComponent<AnimationHelper>().GetAnimationClipsFromImporter(patientObj.GetComponent<Animation>()).ToList();
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
                    patientObj.transform.Find("BodyRaw").gameObject.SetActive(true);
                    patientObj.transform.Find("Body").gameObject.SetActive(false);
                    if (hitBox.bounds.Intersects(knife.bounds))
                    {
                        this.modePhase = ModePhase.Cut;
                    }
                    StartCoroutine(PhaseExecute());
#if UNITY_EDITOR
                    StartCoroutine(EditorTest(ModePhase.Cut));
#endif
                    break;
                case ModePhase.Cut:
                    patientObj.transform.Find("BodyRaw").gameObject.SetActive(false);
                    patientObj.transform.Find("Body").gameObject.SetActive(true);
                    StartCoroutine(PhaseExecute());
                    patientObj.GetComponent<Animation>().wrapMode = WrapMode.ClampForever;
                    if (hitBox.bounds.Intersects(stretchTool.bounds))
                    {
                        this.modePhase = ModePhase.Stretch;
                    }
#if UNITY_EDITOR
                   StartCoroutine(EditorTest(ModePhase.Stretch)); 
#endif
                    break;
                case ModePhase.Stretch:
                    StartCoroutine(PhaseExecute());
                    patientObj.GetComponent<Animation>().wrapMode = WrapMode.ClampForever;
#if UNITY_EDITOR
                    StartCoroutine(EditorTest(ModePhase.Lining));
#endif

                    break;
                case ModePhase.Lining:
                    patientObj.transform.Find("Lining").gameObject.SetActive(true);
                    StartCoroutine(PhaseExecute());
                    patientObj.GetComponent<Animation>().wrapMode = WrapMode.Loop;
                    break;
            }

        }
        void TransplantModeExecution()
        {

        }
        IEnumerator PhaseExecute()
        {
            yield return null; ;
            patientObj.GetComponent<Animation>().clip = animationClips.Find(x => x.name == modePhase.ToString());
            patientObj.GetComponent<Animation>().Play();
        }

        IEnumerator EditorTest(ModePhase mode)
        {
            yield return new WaitForSeconds(patientObj.GetComponent<Animation>().clip.length + 2f);
            modePhase = mode;
        }
    }
}

