namespace Heathside
{
    public interface IDamageReceiver
    {
        void TakeDamage(float incomingDamage);
        void TakeEffect();
    }
}