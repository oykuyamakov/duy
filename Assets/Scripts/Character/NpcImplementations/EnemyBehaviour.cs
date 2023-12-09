using UnityCommon.Runtime.Utility;
using UnityEngine;

namespace Character.NpcImplementations
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class EnemyBehaviour : Character
    {
        private int m_Counter = 0;
        
        private TimedAction m_TimedAction;
        
        private Vector3 m_Direction = Vector3.zero;
        protected override void Awake()
        {
            base.Awake();
            
            m_Direction = UnityEngine.Random.insideUnitCircle;

            m_TimedAction = new TimedAction(() =>
            {
                m_Direction = UnityEngine.Random.insideUnitCircle;

            }, 10f, 10f);

        }
        
        private void MoveRandomly()
        {
            transform.position += new Vector3(m_Direction.x, m_Direction.y, 0) * (m_Speed * Time.deltaTime);
        }

        private void Update()
        {
            m_TimedAction.Update(Time.deltaTime);
            MoveRandomly();
        }

        public override void Die()
        {
            //TODO 
            SMIns.PlayOneShot(m_DieSound);
            gameObject.SetActive(false);
        }

        public override void TakeDamage(float damage)
        {
            base.TakeDamage(damage);
            
            if(m_Health <= 0)
                Die();
        }
    }
}
