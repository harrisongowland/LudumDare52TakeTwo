using UnityEngine;
using UnityEngine.Playables;

namespace HutongGames.PlayMaker.Actions
{

	[ActionCategory(ActionCategory.Animation)]
	[Tooltip("Set the speed at which a given timeline plays back.")]
	public class SetTimelinePlaybackSpeed : FsmStateAction
	{

		public PlayableDirector Director;
		public FsmFloat PlaybackSpeed; 
		
		// Code that runs on entering the state.
		public override void OnEnter()
		{
			Director.playableGraph.GetRootPlayable(0).SetSpeed(PlaybackSpeed.Value);
			Finish();
		}


	}

}
