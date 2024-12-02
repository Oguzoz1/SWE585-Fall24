using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDayTimeEvent
{
    /// <summary>
    /// Use it as a main method to subscribe timed events.
    /// </summary>
    public void HandleTimeEvents();
}
