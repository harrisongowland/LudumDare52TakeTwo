using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{

	[ActionCategory(ActionCategory.GameLogic)]
	[Tooltip("Set the current mode of the card screen (either ADD or TRADE).")]
	public class SetCardScreenMode : FsmStateAction
	{

		public EventHandlerSystem.CardScreenMode Mode; 
		
		// Code that runs on entering the state.
		public override void OnEnter()
		{
			EventHandlerSystem.instance.CurrentCardScreenMode = Mode; 
			Finish();
		}
	}

}
