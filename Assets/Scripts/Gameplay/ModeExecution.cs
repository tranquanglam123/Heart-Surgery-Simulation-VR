using System.Collections;
using UnityEngine;
using VR_Surgery.Scripts.Core;
using static VR_Surgery.Scripts.Core.TypeEnums;
using static VR_Surgery.Scripts.Core.GlobalDefinition;
using System;
using VR_Surgery.Scripts.Utilities;
namespace VR_Surgery.Scripts.Gameplay
{

    public class ModeExecution : MonoBehaviour
    {
        Collider hitBox;
        Collider knife;
        Collider stretchTool;
        private ModePhase modePhase = ModePhase.Idle;
        private void Awake()
        {
            try
            {
                hitBox = patientObj.transform.Find("BodyRaw").GetComponent<Collider>();
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
                    break;
                case ModePhase.Cut:
                    StartCoroutine(PhaseExecute());
                    if (hitBox.bounds.Intersects(stretchTool.bounds))
                    {
                        this.modePhase = ModePhase.Stretch;
                    }
                    break;
                case ModePhase.Stretch:
                    StartCoroutine(PhaseExecute());
                    // If the user touch the lining -> 
                    // this.modePhase = ModePhase.Lining;
                    break;
                case ModePhase.Lining:
                    break;
            }

        }
        void TransplantModeExecution()
        {

        }
        IEnumerator PhaseExecute()
        {
            yield return new WaitForSeconds(2);
            patientObj.GetComponent<Animation>().wrapMode = WrapMode.ClampForever;
            patientObj.GetComponent<Animation>().Play(this.modePhase.ToString());
        }
    }
}

