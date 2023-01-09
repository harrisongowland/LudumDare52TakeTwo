using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{

	[ActionCategory(ActionCategory.GameLogic)]
	[Tooltip("Enter the Prepare phase.")]
	public class Prepare : FsmStateAction
	{

		// Code that runs on entering the state.
		public override void OnEnter()
		{
			EventHandlerSystem.instance.Prepare();
			Finish();
		}


	}

}
