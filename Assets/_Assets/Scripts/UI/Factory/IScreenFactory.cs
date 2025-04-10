using System;
using System.Threading.Tasks;
using _Assets.Scripts.UI.HUD;

namespace _Assets.Scripts.UI.Factory
{
    public interface IScreenFactory : IDisposable
    {
        public Task<Hud> CreateHud();
    }
}