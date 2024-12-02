
namespace AeriaUtil.Systems.GlobalEvent
{
    /// <summary>
    /// Apply this when one wants to subscribe to a global event.
    /// </summary>
    public interface IGlobalEvent
    {
        /// <summary>
        /// Use it as a main method to subscribe events. Call GlobalEventSystem
        /// </summary>
        public void HandleGlobalEvents();
    }
}
