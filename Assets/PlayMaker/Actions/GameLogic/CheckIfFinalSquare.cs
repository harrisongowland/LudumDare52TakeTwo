using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{

	[ActionCategory(ActionCategory.GameLogic)]
	[Tooltip("Check if the player is on the final square.")]
	public class CheckIfFinalSquare : FsmStateAction
	{

		public FsmEvent FinalSquareReached;
		public FsmEvent FinalSquareNotReached;
		
		// Code that runs on entering the state.
		public override void OnEnter()
		{
			Fsm.Event(EventHandlerSystem.instance.FinalSquareReached ? FinalSquareReached : FinalSquareNotReached);
			Finish();
		}


	}

}
