using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace zSpace.zView.Samples
{
	public class Zview : MonoBehaviour
	{

		public Button ZV_ConDiscon,ZV_StdMode,ZV_ARMode,ZV_NoMode;
		public Text ZV_ConDisconLabel;
		public bool ZV_Connected;
		private IntPtr connection;
		private bool isModeSwitchEnabled;

		void Start ()
		{
			_zView = GameObject.FindObjectOfType<ZView> ();
			if (_zView == null) {
				Debug.LogError ("Unable to find reference to zSpace.zView.ZView Monobehaviour.");
				this.enabled = false;
				return;
			}

			// Register event handlers.
			_zView.ConnectionAccepted += OnConnectionAccepted;
			_zView.ConnectionModeSwitched += OnConnectionModeSwitched;
			_zView.ConnectionModeActive += OnConnectionModeActive;
			_zView.ConnectionModePaused += OnConnectionModePaused;
			_zView.ConnectionClosed += OnConnectionClosed;
			_zView.ConnectionError += OnConnectionError;

			_zView.VideoRecordingInactive += OnVideoRecordingInactive;
			_zView.VideoRecordingActive += OnVideoRecordingActive;
			_zView.VideoRecordingFinished += OnVideoRecordingFinished;
			_zView.VideoRecordingPaused += OnVideoRecordingPaused;
			_zView.VideoRecordingError += OnVideoRecordingError;
			_zView.VideoRecordingQualityChanged += OnVideoRecordingQualityChanged;

			// Set the node name.
			_zView.SetNodeName ("Zview");

			// Perform any additional setup for the sample scene.
		}

		void Update ()
		{
			connection = _zView.GetCurrentActiveConnection ();

			//Connect / Disconnect Monitoring
			if (connection == IntPtr.Zero) {
				ZV_Connected = false;
				ZV_ConDisconLabel.text = "与ZView建立连接";
				ZV_ARMode.interactable = false;
				ZV_StdMode.interactable = false;
				ZV_NoMode.interactable = false;
			} else {
				ZV_Connected = true;
				ZV_ConDisconLabel.text = "与ZView断开连接";
				ZV_ARMode.interactable = true;
				ZV_StdMode.interactable = true;
				ZV_NoMode.interactable = true;
			}

			ZView.ConnectionState connectionState = ZView.ConnectionState.Error;
			if (connection != IntPtr.Zero) {
				connectionState = _zView.GetConnectionState (connection);
			}

			isModeSwitchEnabled = (connectionState == ZView.ConnectionState.NoMode) ||
				(connectionState == ZView.ConnectionState.ModeActive) ||
				(connectionState == ZView.ConnectionState.ModePaused);
		}

		public void ZV_Conn ()
		{
			if (!ZV_Connected)
				_zView.ConnectToDefaultViewer ();
			else
				_zView.CloseConnection (connection,ZView.ConnectionCloseAction.None,ZView.ConnectionCloseReason.UserRequested,string.Empty);
		}

		public void ZV_Std ()
		{
			if (isModeSwitchEnabled && this.IsModeAvailable (connection, _zView.GetStandardMode ())) {
				_zView.SetConnectionMode (connection, _zView.GetStandardMode ());
			}
		}

		public void ZV_AR ()
		{
			if (isModeSwitchEnabled && this.IsModeAvailable (connection, _zView.GetAugmentedRealityMode ())) {
				_zView.SetConnectionMode (connection, _zView.GetAugmentedRealityMode ());
			}
		}

		public void ZV_No ()
		{
			if (isModeSwitchEnabled) {
				_zView.SetConnectionMode (connection, IntPtr.Zero);
			}
		}

		//	void OnGUI()
		//	{
		//		GUILayout.Label("Connection UI:");
		//		this.ProcessConnectionUI();
		//
		//		GUILayout.Space(15);
		//		GUILayout.Label("Mode UI:");
		//		this.ProcessModeUI();
		//
		//		GUILayout.Space(15);
		//		GUILayout.Label("Video Recording UI:");
		//		this.ProcessVideoRecordingUI();
		//	}


		//////////////////////////////////////////////////////////////////
		// Private Methods
		//////////////////////////////////////////////////////////////////

		private void ProcessConnectionUI ()
		{
			// Get the current active connection.
			IntPtr connection = _zView.GetCurrentActiveConnection ();

			if (connection == IntPtr.Zero) {
				// Handle connecting to the default viewer.
				if (GUILayout.Button ("Connect To Default Viewer")) {
					_zView.ConnectToDefaultViewer ();
				}
			} else {
				// Handle disconnecting from the default viewer.
				if (GUILayout.Button ("Disconnect From Default Viewer")) {
					_zView.CloseConnection (
						connection,
						ZView.ConnectionCloseAction.None,
						ZView.ConnectionCloseReason.UserRequested,
						string.Empty);
				}
			}
		}

		private void ProcessModeUI ()
		{
			// Get the current active connection.
			IntPtr connection = _zView.GetCurrentActiveConnection ();

			// If connected, get the connection's state.
			ZView.ConnectionState connectionState = ZView.ConnectionState.Error;
			if (connection != IntPtr.Zero) {
				connectionState = _zView.GetConnectionState (connection);
			}

			// Handle pausing the current mode.
			GUI.enabled = (connectionState == ZView.ConnectionState.ModeActive);
			{
				if (GUILayout.Button ("Pause Mode")) {
					_zView.PauseMode (connection);
				}
			}
			GUI.enabled = true;

			// Handle resuming the current mode.
			GUI.enabled = (connectionState == ZView.ConnectionState.ModePaused);
			{
				if (GUILayout.Button ("Resume Mode")) {
					_zView.ResumeMode (connection);
				}
			}
			GUI.enabled = true;

			// Check if it is safe to switch modes.
			bool isModeSwitchEnabled = (connectionState == ZView.ConnectionState.NoMode) ||
			                          (connectionState == ZView.ConnectionState.ModeActive) ||
			                          (connectionState == ZView.ConnectionState.ModePaused);

			// Handle switching to no mode.
			GUI.enabled = isModeSwitchEnabled;
			{
				if (GUILayout.Button ("Switch To No Mode")) {
					_zView.SetConnectionMode (connection, IntPtr.Zero);
				}
			}
			GUI.enabled = true;

			// Handle switching to standard mode.
			GUI.enabled = isModeSwitchEnabled && this.IsModeAvailable (connection, _zView.GetStandardMode ());
			{
				if (GUILayout.Button ("Switch To Standard Mode")) {
					_zView.SetConnectionMode (connection, _zView.GetStandardMode ());
				}
			}
			GUI.enabled = true;

			// Handle switching to augmented reality mode.
			GUI.enabled = isModeSwitchEnabled && this.IsModeAvailable (connection, _zView.GetAugmentedRealityMode ());
			{
				if (GUILayout.Button ("Switch To Augmented Reality Mode")) {
					_zView.SetConnectionMode (connection, _zView.GetAugmentedRealityMode ());
				}
			}
			GUI.enabled = true;

			// Handle moving and resizing the augmented reality overlay.
			GUI.enabled = isModeSwitchEnabled && (_zView.GetConnectionMode (connection) == _zView.GetAugmentedRealityMode ());
			{
				// Restore defaults.
				if (GUILayout.Button ("Reset Overlay Position and Size")) {
					_zView.SetSetting (connection, ZView.SettingKey.OverlayOffsetX, 0.0f);
					_zView.SetSetting (connection, ZView.SettingKey.OverlayOffsetY, 0.0f);
					_zView.SetSetting (connection, ZView.SettingKey.OverlayScaleX, 1.0f);
					_zView.SetSetting (connection, ZView.SettingKey.OverlayScaleY, 1.0f);
				}

				// Handle move.
				GUILayout.BeginHorizontal ();
				{
					GUILayout.Label ("Move Overlay: ");

					// Move left by 1 pixel:
					if (GUILayout.Button ("Left")) {
						float overlayOffsetX = _zView.GetSettingFloat (connection, ZView.SettingKey.OverlayOffsetX);
						_zView.SetSetting (connection, ZView.SettingKey.OverlayOffsetX, overlayOffsetX - 1.0f);
					}

					// Move right by 1 pixel:
					if (GUILayout.Button ("Right")) {
						float overlayOffsetX = _zView.GetSettingFloat (connection, ZView.SettingKey.OverlayOffsetX);
						_zView.SetSetting (connection, ZView.SettingKey.OverlayOffsetX, overlayOffsetX + 1.0f);
					}

					// Move up by 1 pixel:
					if (GUILayout.Button ("Up")) {
						float overlayOffsetY = _zView.GetSettingFloat (connection, ZView.SettingKey.OverlayOffsetY);
						_zView.SetSetting (connection, ZView.SettingKey.OverlayOffsetY, overlayOffsetY + 1.0f);
					}

					// Move down by 1 pixel:
					if (GUILayout.Button ("Down")) {
						float overlayOffsetY = _zView.GetSettingFloat (connection, ZView.SettingKey.OverlayOffsetY);
						_zView.SetSetting (connection, ZView.SettingKey.OverlayOffsetY, overlayOffsetY - 1.0f);
					}
				}
				GUILayout.EndHorizontal ();

				// Handle resize.
				GUILayout.BeginHorizontal ();
				{
					GUILayout.Label ("Resize Overlay: ");

					// Uniformly decrease scale by 0.1 factor:
					if (GUILayout.Button ("Decrease")) {
						float overlayScaleX = _zView.GetSettingFloat (connection, ZView.SettingKey.OverlayScaleX);
						_zView.SetSetting (connection, ZView.SettingKey.OverlayScaleX, overlayScaleX - 0.1f);

						float overlayScaleY = _zView.GetSettingFloat (connection, ZView.SettingKey.OverlayScaleY);
						_zView.SetSetting (connection, ZView.SettingKey.OverlayScaleY, overlayScaleY - 0.1f);
					}

					// Uniformly increase scale by 0.1 factor:
					if (GUILayout.Button ("Increase")) {
						float overlayScaleX = _zView.GetSettingFloat (connection, ZView.SettingKey.OverlayScaleX);
						_zView.SetSetting (connection, ZView.SettingKey.OverlayScaleX, overlayScaleX + 0.1f);

						float overlayScaleY = _zView.GetSettingFloat (connection, ZView.SettingKey.OverlayScaleY);
						_zView.SetSetting (connection, ZView.SettingKey.OverlayScaleY, overlayScaleY + 0.1f);
					}
				}
				GUILayout.EndHorizontal ();
			}            
			GUI.enabled = true;
		}

		private void ProcessVideoRecordingUI ()
		{
			// Get the current active connection.
			IntPtr connection = _zView.GetCurrentActiveConnection ();

			// Get the connection's current video recording state.
			ZView.VideoRecordingState videoRecordingState = ZView.VideoRecordingState.Error;
			if (connection != IntPtr.Zero &&
			   _zView.DoesConnectionSupportCapability (connection, ZView.Capability.VideoRecording)) {
				videoRecordingState = _zView.GetVideoRecordingState (connection);
			}

			// Handle setting the video recording quality level.
			GUI.enabled = (videoRecordingState == ZView.VideoRecordingState.NotRecording);
			{
				if (GUILayout.Button ("Set Video Recording Quality 480p")) {
					_zView.SetSetting (connection, ZView.SettingKey.VideoRecordingQuality, (UInt32)ZView.VideoRecordingQuality.Resolution480p);
				}

				if (GUILayout.Button ("Set Video Recording Quality 720p")) {
					_zView.SetSetting (connection, ZView.SettingKey.VideoRecordingQuality, (UInt32)ZView.VideoRecordingQuality.Resolution720p);
				}

				if (GUILayout.Button ("Set Video Recording Quality 1080p")) {
					_zView.SetSetting (connection, ZView.SettingKey.VideoRecordingQuality, (UInt32)ZView.VideoRecordingQuality.Resolution1080p);
				}
			}
			GUI.enabled = true;

			// Handle starting video recording.
			GUI.enabled = (videoRecordingState == ZView.VideoRecordingState.NotRecording);
			{
				if (GUILayout.Button ("Start Video Recording")) {
					_zView.StartVideoRecording (connection);
				}
			}
			GUI.enabled = true;

			// Handle finishing video recording.
			GUI.enabled = (videoRecordingState == ZView.VideoRecordingState.Recording) ||
			(videoRecordingState == ZView.VideoRecordingState.Paused);
			{
				if (GUILayout.Button ("Finish Video Recording")) {
					_zView.FinishVideoRecording (connection);
				}
			}
			GUI.enabled = true;

			// Handle pausing video recording.
			GUI.enabled = (videoRecordingState == ZView.VideoRecordingState.Recording);
			{
				if (GUILayout.Button ("Pause Video Recording")) {
					_zView.PauseVideoRecording (connection);
				}
			}
			GUI.enabled = true;

			// Handle resuming video recording.
			GUI.enabled = (videoRecordingState == ZView.VideoRecordingState.Paused);
			{
				if (GUILayout.Button ("Resume Video Recording")) {
					_zView.ResumeVideoRecording (connection);
				}
			}
			GUI.enabled = true;

			// Handle saving a video recording.
			GUI.enabled = (videoRecordingState == ZView.VideoRecordingState.Finished);
			{
				if (GUILayout.Button ("Save Video Recording")) {
					string path = Application.dataPath;
					if (Application.isEditor) {
						// If running from the editor, save the video to the project root
						// directory instead of the Assets directory to prevent Unity from 
						// attempting to import the newly saved video.
						path += "/../";
					}

					string fileName = Path.Combine (path, "BasicPresenterSampleVideoRecordingSave.mp4");
					_zView.SaveVideoRecording (connection, fileName);
				}
			}
			GUI.enabled = true;

			// Handle discarding a video recording.
			GUI.enabled = (videoRecordingState == ZView.VideoRecordingState.Finished);
			{
				if (GUILayout.Button ("Discard Video Recording")) {
					_zView.DiscardVideoRecording (connection);
				}
			}
			GUI.enabled = true;
		}

		private void OnConnectionAccepted (ZView sender, IntPtr connection)
		{
			string connectedNodeName = sender.GetConnectedNodeName (connection);
			string connectedNodeStatus = sender.GetConnectedNodeStatus (connection);

			Debug.Log (string.Format ("<color=green>Connection Accepted: {0} ({1})</color>", connectedNodeName, connectedNodeStatus));
		}

		private void OnConnectionModeSwitched (ZView sender, IntPtr connection)
		{
			IntPtr mode = sender.GetConnectionMode (connection);
			string modeName = "None";

			if (mode == sender.GetStandardMode ()) {
				modeName = "Standard";
			} else if (mode == sender.GetAugmentedRealityMode ()) {
				modeName = "Augmented Reality";
			}

			Debug.Log (string.Format ("<color=green>Connection Mode Switched: {0}</color>", modeName));
		}

		private void OnConnectionModeActive (ZView sender, IntPtr connection)
		{
			Debug.Log ("<color=green>Connection Mode Active</color>");
		}

		private void OnConnectionModePaused (ZView sender, IntPtr connection)
		{
			Debug.Log ("<color=green>Connection Mode Paused</color>");
		}

		private void OnConnectionClosed (ZView sender, IntPtr connection)
		{
			ZView.ConnectionCloseAction action = sender.GetConnectionCloseAction (connection);
			ZView.ConnectionCloseReason reason = sender.GetConnectionCloseReason (connection);

			Debug.Log (string.Format ("<color=green>Connection Closed: (Action={0}, Reason={1})</color>", action, reason));
		}

		private void OnConnectionError (ZView sender, IntPtr connection)
		{
			ZView.PluginError error = sender.GetConnectionError (connection);

			Debug.LogError (string.Format ("<color=green>Connection Error: {0}</color>", error));
		}

		private void OnVideoRecordingInactive (ZView sender, IntPtr connection)
		{
			Debug.Log ("<color=red>Video Recording Inactive</color>");
		}

		private void OnVideoRecordingActive (ZView sender, IntPtr connection)
		{
			Debug.Log ("<color=red>Video Recording Active</color>");
		}

		private void OnVideoRecordingFinished (ZView sender, IntPtr connection)
		{
			Debug.Log ("<color=red>Video Recording Finished</color>");
		}

		private void OnVideoRecordingPaused (ZView sender, IntPtr connection)
		{
			Debug.Log ("<color=red>Video Recording Paused</color>");
		}

		private void OnVideoRecordingError (ZView sender, IntPtr connection)
		{
			ZView.PluginError error = sender.GetVideoRecordingError (connection);

			Debug.LogError (string.Format ("<color=red>Video Recording Error: {0}</color>", error));

			sender.ClearVideoRecordingError (connection);
		}

		private void OnVideoRecordingQualityChanged (ZView sender, IntPtr connection)
		{
			ZView.VideoRecordingQuality videoRecordingQuality =
				(ZView.VideoRecordingQuality)sender.GetSettingUInt32 (connection, ZView.SettingKey.VideoRecordingQuality);

			Debug.Log (string.Format ("<color=red>Video Recording Quality Changed: {0}</color>", videoRecordingQuality));
		}

		private bool IsModeAvailable (IntPtr connection, IntPtr mode)
		{
			if (connection != IntPtr.Zero) {
				int numSupportedModes = _zView.GetNumConnectionSupportedModes (connection);

				for (int i = 0; i < numSupportedModes; ++i) {
					ZView.SupportedMode supportedMode = _zView.GetConnectionSupportedMode (connection, i);
					if (supportedMode.Mode == mode) {
						return (supportedMode.ModeAvailability == ZView.ModeAvailability.Available);
					}
				}
			}

			return false;
		}


		//////////////////////////////////////////////////////////////////
		// Private Members
		//////////////////////////////////////////////////////////////////

		private ZView _zView = null;

		private GameObject _cubeObject = null;
		private GameObject _sphereObject = null;
		private GameObject _capsuleObject = null;
		private GameObject _leftPillarObject = null;
		private GameObject _rightPillarObject = null;
	}
}