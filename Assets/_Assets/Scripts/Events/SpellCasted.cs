using _Assets.Scripts.Core.Infrastructure.EventManagement;

namespace _Assets.Scripts.Events
{
    public struct SpellCasted : IEvent
    {
        public int Cooldown { get; }

        public SpellCasted(int cooldown) =>
            Cooldown = cooldown;
    }
}