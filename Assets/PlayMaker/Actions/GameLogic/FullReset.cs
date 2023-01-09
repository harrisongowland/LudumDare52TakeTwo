using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{

	[ActionCategory(ActionCategory.GameLogic)]
	[Tooltip("Do a full reset of the game state.")]
	public class FullReset : FsmStateAction
	{

		// Code that runs on entering the state.
		public override void OnEnter()
		{
			EventHandlerSystem.instance.FullReset();
			Finish();
		}


	}

}
