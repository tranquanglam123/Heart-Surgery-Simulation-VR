using System.Collections;
using System.Collections.Generic;
using VR_Surgery.Scripts.Core;
using static VR_Surgery.Scripts.Core.GlobalDefinition;
using UnityEngine;
using TMPro;
using Oculus.Interaction;
namespace VR_Surgery.Scripts.Utilities
{
    public static class Helper
    {
        /// <summary>
        /// Log the message as a menu to screen
        /// </summary>
        public static GameObject CreateMessageMenu(string message)
        {
            if (currentMessageMenu == null)
            {
                currentMenu.SetActive(false);
                GameObject msgMenu = GameObject.Instantiate(Resources.Load(ResourcesPath.UIPrefabFolder + GlobalDefinition.BaseMenu) as GameObject);
                msgMenu.transform.Find("Panel").Find("BG").Find("Title").GetComponent<TextMeshProUGUI>().text = "Exception Caught !!";
                msgMenu.transform.Find("Panel").Find("BG").Find("Description").GetComponent<TextMeshProUGUI>().text = message;
                msgMenu.GetComponentInChildren<PointableUnityEventWrapper>().WhenSelect.AddListener((PointerEvent eventData) =>
                {
                    currentMenu.SetActive(true);
                    currentMessageMenu.SetActive(false);
                });
                return msgMenu;
            }
            else
            {
                currentMessageMenu.transform.Find("Panel").Find("BG").Find("Title").GetComponent<TextMeshProUGUI>().text = "Exception Caught !!";
                currentMessageMenu.transform.Find("Panel").Find("BG").Find("Description").GetComponent<TextMeshProUGUI>().text = message;
                currentMessageMenu.GetComponentInChildren<PointableUnityEventWrapper>().WhenSelect.AddListener((PointerEvent eventData) =>
                {
                    currentMenu.SetActive(true);
                    currentMessageMenu.SetActive(false);
                });
                return currentMessageMenu;
            }
        }
        
        public static void CreateMenu(string menuName)
        {
            if (currentMenu != null)
            {
                GameObject.Destroy(currentMenu);
            }
            currentMenu = GameObject.Instantiate(Resources.Load(ResourcesPath.UIPrefabFolder + menuName) as GameObject);
        }
    }
}
