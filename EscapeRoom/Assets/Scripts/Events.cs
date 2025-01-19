using Unity.VisualScripting;
using UnityEngine;

namespace Unity.FPS.Game
{
    // The Game Events used across the Game.
    // Anytime there is a need for a new event, it should be added here.

    public static class Events
    {
        // Jibbe's Events
        public static ValidateAnswerEvent ValidateAnswerEvent = new();
        public static ClickEvent ClickEvent = new();

    }

    // Jibbe's Events
    public class ClickEvent : GameEvent
    {
    }

    public class ValidateAnswerEvent : GameEvent
    {
        private string _answer;
        
        public string Answer
        {
            get { return _answer; }
            set
            {
                _answer = value;
                EventManager.Broadcast(Events.ValidateAnswerEvent);
            }
        }
    }
}
