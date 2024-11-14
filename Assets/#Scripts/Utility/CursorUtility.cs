using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameUtility
{
    public static class CursorUtility 
    {
        public static void CenterCursor()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        public static void ReleaseCursor()
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }
}
