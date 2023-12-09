using Roro.Scripts.Serialization;
using Roro.Scripts.Sounds.Core;
using Roro.Scripts.Utility;
using Sirenix.OdinInspector;
using UnityCommon.Modules;
using UnityCommon.Singletons;
using UnityCommon.Variables;
using UnityEngine;
using Utility;

namespace Roro.Scripts.GameManagement
{
    [DefaultExecutionOrder(ExecOrder.GameManager)]
    public class GameManager : SingletonBehaviour<GameManager>
    {
        [SerializeField]
        private int m_TargetFrameRate = 60;

        [SerializeField] 
        private BoolVariable m_GameIsRunning;
        
        public BoolVariable GameIsRunning => m_GameIsRunning;

        [Button]
        public void ToggleGame()
        {
            m_GameIsRunning.Value = !m_GameIsRunning.Value;
        }

        private void Awake()
        {
            if (!SetupInstance())
                return;

            Variable.Initialize();
            
            Screen.sleepTimeout = SleepTimeout.NeverSleep;

            Application.backgroundLoadingPriority = ThreadPriority.Normal;

            Application.targetFrameRate = m_TargetFrameRate;
            
            ConditionalsModule.CreateSingletonInstance();

            var a = SerializationWizard.Default.GetInt("sound_count", 0);
            Debug.Log(a);
            SerializationWizard.Default.SetInt("sound_count", 2);
            a = SerializationWizard.Default.GetInt("sound_count", 0);
            Debug.Log(a);
            
            
            m_GameIsRunning =  Variable.Get<BoolVariable>("GameIsRunning");
            
            m_GameIsRunning.Value = true;
            
            SoundManager.Instance.PlayOneShot(SoundDatabase.Get().CountDownSound);

            //Conditional.Wait(10).Do(() => m_GameIsRunning.Value = true);

        }
    }
}