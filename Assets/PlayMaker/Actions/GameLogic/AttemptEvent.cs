using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory(ActionCategory.GameLogic)]
    [Tooltip("Attempt a Board Event.")]
    public class AttemptEvent : FsmStateAction
    {
        [Tooltip("Leave null to attempt the event currently saved in the system.")]
        public BoardEvent TestEvent;

        // Code that runs on entering the state.
        public override void OnEnter()
        {
            if (TestEvent != null)
            {
                EventHandlerSystem.instance.AttemptEvent(TestEvent);
            }
            else
            {
                EventHandlerSystem.instance.AttemptEvent();
            }
            Finish();
        }
    }
}