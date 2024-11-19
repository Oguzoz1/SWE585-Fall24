using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Player.CameraMovement
{
    /// <summary>
    /// Marker for classes that interacts with the camera
    /// </summary>
    public interface ICameraDependent
    {
        /// <summary>
        /// This is when to free the camera from its responsibilities
        /// </summary>
        public void StopManipulating();
        /// <summary>
        /// This is when to grab back the responsibilities.
        /// </summary>
        public void ResumeManipulating();
        public bool isManipulating();
    }
}
