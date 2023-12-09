using System;
using Character.NpcImplementations;
using Roro.Scripts.Sounds.Core;
using Roro.Scripts.Sounds.Data;
using Unity.VisualScripting;
using UnityCommon.Runtime.Extensions;
using UnityCommon.Runtime.Utility;
using UnityCommon.Variables;
using UnityEngine;
using UnityEngine.Serialization;
using Utility;

namespace PlayerManagement
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerInput : Character.Character
    {
        
        private SoundDatabase m_SoundDatabase;
        [SerializeField]
        private Sound m_EnterAreaSound;
        [SerializeField]
        private Sound m_ExitAreaSound;

        private Rigidbody2D m_Rb;
        private Collider2D m_Collider;
        
        private EnemyBehaviour m_EnemyBehaviour;

        private TimedAction m_AttackTimedAction;

        private BoolVariable m_GameIsRunning;
        
        
        protected override void Awake()
        {            
            base.Awake();
            
            m_GameIsRunning = Variable.Get<BoolVariable>("GameIsRunning");
            
            m_Rb = GetComponent<Rigidbody2D>();
            m_Collider = GetComponent<Collider2D>();
            m_SoundDatabase = SoundDatabase.Get();
            
            m_AttackTimedAction = new TimedAction(Attack,0,0.5f);
            
            SMIns.PlayLoop(m_SoundDatabase.InAreaLoopSound);
        }

        private void Update()
        {
            if(!m_GameIsRunning.Value)
                return;
            
            Move();
            PlayInteractionSounds();

            if (Input.GetKey(KeyCode.P))
            {
                m_AttackTimedAction.Update(Time.deltaTime);
            }
        }

        private void Move()
        {
            if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            {
                transform.position += Vector3.up * (m_Speed * Time.deltaTime);
            }
            if(Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            {
                transform.position += Vector3.down * (m_Speed * Time.deltaTime);
            }
            if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                transform.position += Vector3.left * (m_Speed * Time.deltaTime);
            }
            if(Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                transform.position += Vector3.right * (m_Speed * Time.deltaTime);
            }

            if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical")  != 0)
            {
                SMIns.PlayLoop(m_WalkSound);
            }
            else
            {
                SMIns.StopSound(m_WalkSound.GetId());
            }
        }

        private void PlayInteractionSounds()
        {
            if(Input.GetKeyDown(KeyCode.LeftBracket))
            {
                SoundManager.Instance.PlayOneShot(m_SoundDatabase.LBracketSound);
            }
            if(Input.GetKeyDown(KeyCode.RightBracket))
            {
                SoundManager.Instance.PlayOneShot(m_SoundDatabase.RBracketSound);
            }
            // if(Input.GetKeyDown(KeyCode.P))
            // {
            //     SoundManager.Instance.PlayOneShot(m_SoundDatabase.PSound);
            // }
            // if(Input.GetKeyDown(KeyCode.O))
            // {
            //     SoundManager.Instance.PlayOneShot(m_SoundDatabase.OSound);
            // }
        }

        public override void Attack()
        {
            if (m_EnemyBehaviour != null)
            {
                Debug.Log("hitting enemy");
                
                //base.Attack();
            
                m_EnemyBehaviour.TakeDamage(m_AttackDamage);
                SMIns.PlayOneShot(m_SuccessfulAttackSound);
            }
            else
            {
                Debug.Log("NO HIT");

                
                SMIns.PlayOneShot(m_AttackSound);
            }
            
        }


        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent<EnemyBehaviour>(out var enemyBehaviour))
            {
                SMIns.PlayOneShot(m_EnterAreaSound);
                m_EnemyBehaviour = enemyBehaviour;
                
                SMIns.PlayLoop(m_SoundDatabase.InAreaLoopSound);
                SMIns.StopSound(m_SoundDatabase.OutAreaLoopSound.GetId());
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.TryGetComponent<EnemyBehaviour>(out var enemyBehaviour))
            {
                SMIns.PlayOneShot(m_ExitAreaSound);
                m_EnemyBehaviour = null;
                
                SMIns.PlayLoop(m_SoundDatabase.OutAreaLoopSound);
                SMIns.StopSound(m_SoundDatabase.InAreaLoopSound.GetId());
            }
        }
    }
}
