using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{

	[ActionCategory(ActionCategory.GameLogic)]
	[Tooltip("Start the Gather phase with the agents you have left.")]
	public class Gather : FsmStateAction
	{

		// Code that runs on entering the state.
		public override void OnEnter()
		{
			EventHandlerSystem.instance.Gather();
			Finish();
		}


	}

}
