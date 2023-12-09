namespace Character
{
    public interface IAlive
    {
        public void Die();
        public void TakeDamage(float damage);
        public void Heal(float heal);
    }
}
