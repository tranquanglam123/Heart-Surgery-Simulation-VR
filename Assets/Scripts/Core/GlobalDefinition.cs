using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static VR_Surgery.Scripts.Core.TypeEnums;
namespace VR_Surgery.Scripts.Core
{
    /// <summary>
    /// File contains constant values to use throughout the application
    /// </summary>
    public static class GlobalDefinition
    {
        //Prefabs name
        public static string SurgicalRoom = "SurgicalRoom";
        public static string StartMenu = "StartMenu";
        public static string ModeMenu = "ModeMenu";
        public static string BaseMenu = "BaseMenu";
        public static string BackgroundMusic = "BackgroundMusic";
        public static string PatientPrefab = "AllAnimation.fbx";
        public static string CutAnimPrefab = "Cut.fbx";

        // static Game Object in scene
        public static GameObject currentMenu = null;
        public static GameObject currentMessageMenu = null;
        public static AudioSource audioPlayer = null;
        public static GameObject modeExecutionObj = null;
        public static GameObject patientObj = null;
        public static GameObject heartObj = null;
        public static GameObject patientHeartObj = null;
        public static GameObject patientHeartOldObj = null;

        // Gameobject names in scene
        public static string PatientObjName = "AllAnimation";
        public static string PatientHeartObjName = "pasted__pasted__Heart_Animated_Master";
        public static string PatientHeartOldObjName = "pasted__pasted__Heart_Animated_MasterOld";
        public static string TransplantHeartObjName = "heart-and-lung-animation";
        public static string HitBox = "HitBox";
        public static string Knife = "Knife";
        public static string StretchTool = "StretchTool";
        public static string StretchTool2nd = "StretchTool2nd";

        // Constant values
        public static Vector3 StretchToolSpawnPos = new Vector3(0.3131f, 1.7394f, 0.119999997f);
        public static Vector3 StretchTool2ndSpawnPos = new Vector3(0.0879999995f, 1.7394f, 0.119999997f);
        public static Vector3 StretchToolEndPos = new Vector3(0.3773f, 1.7394f, 0.119999997f);
        public static Vector3 StretchTool2ndEndPos = new Vector3(0.0363f, 1.7394f, 0.119999997f);
        public static float duration = 1.2f;
        public static bool Tool1InPos = false;
        public static bool Tool2InPos = false;

        // Other data
        public static List<AnimationClip> AnimationClips = null;

        // Enum                                                                                                                                                                                                                                                                      
        public static OperatingMode PlayMode = OperatingMode.Null;

    }

}
