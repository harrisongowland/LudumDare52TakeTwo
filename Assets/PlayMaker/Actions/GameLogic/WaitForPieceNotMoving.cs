using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{

	[ActionCategory(ActionCategory.GameLogic)]
	[Tooltip("Wait for the player piece to stop moving")]
	public class WaitForPieceNotMoving : FsmStateAction
	{

		public FsmEvent RunOnComplete; 
		
		// Code that runs on entering the state.
		public override void OnEnter()
		{
			
		}

		public override void OnUpdate()
		{
			if (DiceSystem.instance.m_Piece.Moving == false)
			{
				Fsm.Event(RunOnComplete);
				Finish();
			}
		}


	}

}
