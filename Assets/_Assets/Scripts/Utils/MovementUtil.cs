using UnityEngine;

namespace _Assets.Scripts.Utils
{
    public static class MovementUtil
    {
        public static bool IsInsideBoundary(Vector2 position, Vector2 mapSize)
        {
            var halfWidth = mapSize.x / 2f;
            var halfHeight = mapSize.y / 2f;
        
            return position.x >= -halfWidth && position.x <= halfWidth && 
                   position.y >= -halfHeight && position.y <= halfHeight;
        }
    
        public static Vector2 ClampPositionToBoundary(Vector2 position, Vector2 mapSize)
        {
            var halfWidth = mapSize.x / 2f;
            var halfHeight = mapSize.y / 2f;
        
            var x = Mathf.Clamp(position.x, -halfWidth, halfWidth);
            var y = Mathf.Clamp(position.y, -halfHeight, halfHeight);
        
            return new Vector2(x, y);
        }
    }
}