using System;
using SceneManagement;
using UnityEngine;

namespace Roro.Scripts.SettingImplementations
{
    [CreateAssetMenu(fileName =" GeneralSettings" )]
    public class GeneralSettings : ScriptableObject
    {
        private static GeneralSettings _GeneralSettings;

        private static GeneralSettings generalSettings
        {
            get
            {
                if (!_GeneralSettings)
                {
                    _GeneralSettings = Resources.Load<GeneralSettings>($"Settings/GeneralSettings");

                    if (!_GeneralSettings)
                    {
#if UNITY_EDITOR
                        Debug.Log("General Settings not found AND NOT creating and a new one");
                        //_GeneralSettings = CreateInstance<GeneralSettings>();
                        // var path = "Assets/Resources/Settings/GeneralSettings.asset";
                        // AssetDatabaseHelpers.CreateAssetMkdir(_GeneralSettings, path);
#else
 				//		throw new Exception("Global settings could not be loaded");
#endif
                    }
                }

                return _GeneralSettings;
            }
        }
        
        public static GeneralSettings Get()
        {
            return generalSettings;
        }
        
        public float SceneTransitionDuration = 2f;
        
        public float IntroWaitDuration = 2f;
        
        public float LoadingFadeInDuration = 0.8f;
        public float LoadingFadeOutDuration = 0.8f;
        
        //WHAT?
        public float MinuteToRealTime = 0.8f;

        // TODO: HATE THE NAMES
        public int DayHour = 10;
        
        public int NightHour = 20;
        
    }

    [Serializable]
    public class ScreenShakeValues
    {
        public float Duration = 0;
        public float Magnitude = 1;
    }

    [Serializable]
    public struct SceneToScene
    {
        public SceneId From;
        public SceneId To;
    }
}
