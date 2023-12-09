using System;
using Roro.Scripts.Sounds.Core;
using Roro.Scripts.Sounds.Data;
using UnityEngine;

namespace Character
{
    public abstract class Character : MonoBehaviour,IAlive
    {
        [SerializeField] 
        protected Sound m_DieSound;
        [SerializeField] 
        protected Sound m_TakeDamageSound;
        [SerializeField] 
        protected Sound m_AttackSound;
        [SerializeField] 
        protected Sound m_SuccessfulAttackSound;
        [SerializeField] 
        protected Sound m_WalkSound;
        [SerializeField]
        protected float m_Speed = 10f;
        [SerializeField]
        protected float m_AttackRange = 1f;
        [SerializeField]
        protected float m_AttackDamage = 1f;
        [SerializeField]
        protected float m_Health = 10f;

        protected SoundManager SMIns;

        protected virtual void Awake()
        {
            SMIns = SoundManager.Instance;
        }


        public virtual void Die()
        {
            SMIns.PlayOneShot(m_DieSound);
        }

        public virtual void TakeDamage(float damage)
        {
            SMIns.PlayOneShot(m_TakeDamageSound);
            m_Health -= damage;
        }

        public virtual void Heal(float heal)
        {
            throw new System.NotImplementedException();
        }
        public virtual void Attack()
        {
            throw new System.NotImplementedException();
        }
    }
}
