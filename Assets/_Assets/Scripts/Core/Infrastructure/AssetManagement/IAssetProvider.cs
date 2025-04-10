using System.Threading.Tasks;
using UnityEngine.AddressableAssets;

namespace _Assets.Scripts.Core.Infrastructure.AssetManagement
{
    public interface IAssetProvider
    {
        public Task<T> Load<T>(AssetReference assetReference) where T : class;
        public Task<T> Load<T>(string address) where T : class;
        public void CleanUp();
    }
}