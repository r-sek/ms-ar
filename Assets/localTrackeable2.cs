/*==============================================================================
Copyright (c) 2010-2014 Qualcomm Connected Experiences, Inc.
All Rights Reserved.
Confidential and Proprietary - Protected under copyright and other laws.
==============================================================================*/

using UnityEngine;
using System.Collections;

namespace Vuforia
{
	/// <summary>
	/// A custom handler that implements the ITrackableEventHandler interface.
	/// </summary>

	public class localTrackeable2 : MonoBehaviour,
	ITrackableEventHandler
	{
		public SetImage2 setimage;
		public MeshRenderer text;
		public SpriteRenderer sprite;
		public MeshRenderer board;
		public ParticleSystem ps;
		private Coroutine movie;


		#region PRIVATE_MEMBER_VARIABLES

		private TrackableBehaviour mTrackableBehaviour;

		#endregion // PRIVATE_MEMBER_VARIABLES



		#region UNTIY_MONOBEHAVIOUR_METHODS

		void Start()
		{
			mTrackableBehaviour = GetComponent<TrackableBehaviour>();
			if (mTrackableBehaviour)
			{
				mTrackableBehaviour.RegisterTrackableEventHandler(this);
			}
		}

		#endregion // UNTIY_MONOBEHAVIOUR_METHODS



		#region PUBLIC_METHODS

		/// <summary>
		/// Implementation of the ITrackableEventHandler function called when the
		/// tracking state changes.
		/// </summary>
		public void OnTrackableStateChanged(
			TrackableBehaviour.Status previousStatus,
			TrackableBehaviour.Status newStatus)
		{
			if (newStatus == TrackableBehaviour.Status.DETECTED ||
				newStatus == TrackableBehaviour.Status.TRACKED ||
				newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED)
			{
				OnTrackingFound();
			}
			else
			{
				OnTrackingLost();
			}
		}

		#endregion // PUBLIC_METHODS



		#region PRIVATE_METHODS


		private void OnTrackingFound()
		{
			movie = StartCoroutine (Movie());
			setimage.Texturechange ();
			text.enabled = true;
			sprite.enabled = true;
			board.enabled = true;
			Debug.Log ("FoundMarker4");
		}
		private void OnTrackingLost()
		{
			if (movie != null) {
				StopCoroutine (movie);
			}
			text.enabled = false;
			sprite.enabled = false;
			board.enabled = false;
			Debug.Log ("LostMarker4");
		}

		private IEnumerator Movie(){
			ps.Play ();
			yield return new WaitForSeconds (1.0f);
			ps.Stop ();
			yield return new WaitForSeconds (2.0f);
		}

		#endregion // PRIVATE_METHODS
	}
}
