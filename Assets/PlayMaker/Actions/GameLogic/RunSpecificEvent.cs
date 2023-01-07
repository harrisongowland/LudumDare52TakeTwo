using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{

	[ActionCategory(ActionCategory.GameLogic)]
	[Tooltip("Run a specific Board Event.")]
	public class RunSpecificEvent : FsmStateAction
	{

		public BoardEvent EventToRun;
		// Code that runs on entering the state.
		public override void OnEnter()
		{
			EventHandlerSystem.instance.DisplayEvent(EventToRun);
			Finish();
		}


	}

}
