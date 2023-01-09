using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{

	[ActionCategory(ActionCategory.GameLogic)]
	[Tooltip("Display the card screen.")]
	public class DisplayCardScreen : FsmStateAction
	{

		public FsmBool cardScreenType; 
		public FsmBool display; 
		// Code that runs on entering the state.
		public override void OnEnter()
		{
			if (display.Value)
			{
				EventHandlerSystem.instance.DisplayCardScreen(cardScreenType.Value);
			}
			else
			{
				EventHandlerSystem.instance.SetCanvasVisibility(EventHandlerSystem.instance.CardScreen, false);
			}

			Finish();
		}


	}

}
