using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{

	[ActionCategory(ActionCategory.GameLogic)]
	[Tooltip("Get if the player currently has too many cards.")]
	public class IsDiscardRequired : FsmStateAction
	{

		public FsmEvent DiscardRequired;
		public FsmEvent DiscardNotRequired;
		
		// Code that runs on entering the state.
		public override void OnEnter()
		{
			Fsm.Event(EventHandlerSystem.instance.DiscardRequired ? DiscardRequired : DiscardNotRequired);
			Finish();
		}
	}

}
