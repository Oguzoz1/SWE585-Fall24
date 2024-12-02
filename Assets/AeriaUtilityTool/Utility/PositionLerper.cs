using UnityEngine;

namespace AeriaUtil
{
    /// <summary>
    /// Utility struct to linearly interpolate between two Vector3 values. Allows for flexible linear interpolations
    /// where current and target change over time.
    /// </summary>
    public struct PositionLerper
    {
        // Calculated time elapsed for the most recent interpolation
        static float m_CurrentLerpTime;

        // The duration of the interpolation, in seconds
        const float LERPTIME = 0.1f;

        public PositionLerper(Vector3 start)
        {
            m_CurrentLerpTime = 0f;
        }

        /// <summary>
        /// Linearly interpolate between two Vector3 values.
        /// </summary>
        /// <param name="current"> Start of the interpolation. </param>
        /// <param name="target"> End of the interpolation. </param>
        /// <returns> A Vector3 value between current and target. </returns>
        public static Vector3 LerpPosition(Vector3 current, Vector3 target)
        {
            if (current != target)
            {
                m_CurrentLerpTime = 0f;
            }

            m_CurrentLerpTime += Time.deltaTime;
            if (m_CurrentLerpTime > LERPTIME)
            {
                m_CurrentLerpTime = LERPTIME;
            }

            var lerpPercentage = m_CurrentLerpTime / LERPTIME;

            return Vector3.Lerp(current, target, lerpPercentage);
        }
    }
}