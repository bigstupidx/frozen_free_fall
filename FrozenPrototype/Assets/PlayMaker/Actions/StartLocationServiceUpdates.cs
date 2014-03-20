// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{	
	[ActionCategory(ActionCategory.Device)]
	[Tooltip("Starts location service updates. Last location coordinates can be retrieved with GetLocationInfo.")]
	public class StartLocationServiceUpdates : FsmStateAction
	{
		[Tooltip("Maximum time to wait in seconds before failing.")]
		public FsmFloat maxWait;
		public FsmFloat desiredAccuracy;
		public FsmFloat updateDistance;
		[Tooltip("Event to send when the location services have started.")]
		public FsmEvent successEvent;
		[Tooltip("Event to send if the location services fail to start.")]
		public FsmEvent failedEvent;


        public override void Reset()
		{
			maxWait = 20;
			desiredAccuracy = 10;
			updateDistance = 10;
			successEvent = null;
			failedEvent = null;
		}

		public override void OnEnter()
        {
            Finish();
		}
		
		public override void OnUpdate()
        {
        }
	}
}