using _Assets.Scripts.Core.Infrastructure.EventManagement;
using _Assets.Scripts.Spells.Enum;

namespace _Assets.Scripts.Events
{
    public struct CurrentSpellChanged : IEvent
    {
        public SpellType CurrentSpellType{ get; }

        public CurrentSpellChanged(SpellType currentSpellType) =>
            CurrentSpellType = currentSpellType;
    }
}