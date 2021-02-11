namespace Heathside.Control
{
    public interface IDamageReceiver
    {
        void TakeDamage(float incomingDamage);
        void TakeEffect(IEffectState effect);
    }
}