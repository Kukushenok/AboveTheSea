using UnityEngine;

namespace Player
{
    public static class LerpFunctions
    {
        public static float LerpTFixedTime(float dampCoeff)
        {
            return 1 - Mathf.Pow(dampCoeff, Time.fixedDeltaTime);
        }
        public static float LerpTDeltaTime(float dampCoeff)
        {
            return 1 - Mathf.Pow(dampCoeff, Time.deltaTime);
        }
        public static void DampByLerp(ref float curr, float target, float lerpCoeff)
        {
            curr = Mathf.Lerp(curr, target, lerpCoeff);
        }
        public static void DampByFixedTime(ref float curr, float target, float dampCoeff)
        {
            DampByLerp(ref curr, target, LerpTFixedTime(dampCoeff));
        }
        public static void DampAngleByFixedTime(ref float curr, float target, float dampCoeff)
        {
            DampByLerp(ref curr, target, LerpTFixedTime(dampCoeff));
        }
        public static void DampByDeltaTime(ref float curr, float target, float dampCoeff)
        {
            DampByLerp(ref curr, target, LerpTDeltaTime(dampCoeff));
        }
        public static void DampAngleByDeltaTime(ref float curr, float target, float dampCoeff)
        {
            DampByLerp(ref curr, target, LerpTDeltaTime(dampCoeff));
        }

        public static void DampByFixedTime(ref Vector2 vec, Vector2 target, float dampCoeff)
        {
            DampByFixedTime(ref vec.x, target.x, dampCoeff);
            DampByFixedTime(ref vec.y, target.y, dampCoeff);
        }
    }
}