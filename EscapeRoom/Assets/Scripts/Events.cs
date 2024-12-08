using UnityEngine;

namespace Unity.FPS.Game
{
    // The Game Events used across the Game.
    // Anytime there is a need for a new event, it should be added here.

    public static class Events
    {
        // Jibbe's Events
        public static XpUpdateEvent XpUpdateEvent = new XpUpdateEvent();

    }

    // Jibbe's Events
    public class XpUpdateEvent : GameEvent
    {
        public int XP = 0;
    }
}
