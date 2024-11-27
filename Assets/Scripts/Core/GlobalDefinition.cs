using UnityEngine;
using static VR_Surgery.Scripts.Core.TypeEnums;
namespace VR_Surgery.Scripts.Core
{
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

        // Enum
        public static OperatingMode PlayMode = OperatingMode.Null;

    }

}
