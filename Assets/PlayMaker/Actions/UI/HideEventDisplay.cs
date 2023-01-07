using UnityEngine;
using UnityEngine.EventSystems;

namespace HutongGames.PlayMaker.Actions
{

	[ActionCategory(ActionCategory.UI)]
	[Tooltip("Shut off the event display canvas.")]
	public class HideEventDisplay : FsmStateAction
	{

		// Code that runs on entering the state.
		public override void OnEnter()
		{
			EventHandlerSystem.instance.ShowEventCanvas(false);
			Finish();
		}


	}

}
