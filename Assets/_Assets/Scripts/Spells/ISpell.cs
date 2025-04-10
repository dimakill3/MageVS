using System;

namespace _Assets.Scripts.Spells
{
    public interface ISpell : IDisposable
    {
        void Cast();
    }
}