using UnityEngine;

namespace AncientForgeQuest.Utility
{
    public static class RandomExtension
    {
        public static bool IsSuccess(this float percentage)
        {
            return Random.value < percentage;
        }
        
        public static int GetRandom(this Vector2Int range)
        {
            return Random.Range(range.x, range.y);
        }
    }
}
