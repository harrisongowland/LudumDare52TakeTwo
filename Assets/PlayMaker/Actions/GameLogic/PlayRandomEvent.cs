using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{

	[ActionCategory(ActionCategory.GameLogic)]
	[Tooltip("Choose a random event and play it.")]
	public class PlayRandomEvent : FsmStateAction
	{

		public FsmBool ignorePlayedEvents; 
		// Code that runs on entering the state.
		public override void OnEnter()
		{
			EventHandlerSystem.instance.DisplayEvent(ignorePlayedEvents.Value);
			Finish();
		}


	}

}
