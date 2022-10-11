/*
 * Copyright (c) Meta Platforms, Inc. and affiliates.
 * All rights reserved.
 *
 * Licensed under the Oculus SDK License Agreement (the "License");
 * you may not use the Oculus SDK except in compliance with the License,
 * which is provided at the time of installation or download, or which
 * otherwise accompanies this software in either electronic or hard copy form.
 *
 * You may obtain a copy of the License at
 *
 * https://developer.oculus.com/licenses/oculussdk/
 *
 * Unless required by applicable law or agreed to in writing, the Oculus SDK
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

#if UNITY_ANDROID && !UNITY_EDITOR
#define OVR_ANDROID_MRC
#endif

using UnityEngine;
using System.Collections;

#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN || UNITY_ANDROID

public abstract class OVRComposition {

	public bool cameraInTrackingSpace = false;	//カメラのトラッキングスペース
	public OVRCameraRig cameraRig = null;		//カメラリグ

	/// <summary>
	/// コンストラクタ
	/// </summary>
	/// <param name="parentObject">親オブジェクト</param>
	/// <param name="mainCamera">メインカメラ</param>
	/// <param name="configuration">構成</param>
	protected OVRComposition(GameObject parentObject, Camera mainCamera, OVRMixedRealityCaptureConfiguration configuration) {
		RefreshCameraRig(parentObject, mainCamera);
	}

	/// <summary>
	/// 構成メソッド
	/// </summary>
	/// <returns></returns>
	public abstract OVRManager.CompositionMethod CompositionMethod();

	/// <summary>
	/// 
	/// </summary>
	/// <param name="gameObject"></param>
	/// <param name="mainCamera"></param>
	/// <param name="configuration">キャプチャ構成</param>
	/// <param name="trackingOrigin">トラッキング期限</param>
	public abstract void Update(GameObject gameObject, Camera mainCamera, OVRMixedRealityCaptureConfiguration configuration, OVRManager.TrackingOrigin trackingOrigin);
	public abstract void Cleanup();

	/// <summary>
	/// 位置決め
	/// </summary>
	public virtual void RecenterPose() { }

	protected bool usingLastAttachedNodePose = false;		//
	protected OVRPose lastAttachedNodePose = new OVRPose(); // Sometimes the attach node pose is not readable (lose tracking, low battery, etc.) Use the last pose instead when it happens

	//
	public void RefreshCameraRig(GameObject parentObject, Camera mainCamera)
	{
		OVRCameraRig cameraRig = mainCamera.GetComponentInParent<OVRCameraRig>();
		if (cameraRig == null)
		{
			cameraRig = parentObject.GetComponent<OVRCameraRig>();
		}
		cameraInTrackingSpace = (cameraRig != null && cameraRig.trackingSpace != null);
		this.cameraRig = cameraRig;
		Debug.Log(cameraRig == null ? "[OVRComposition] CameraRig not found" : "[OVRComposition] CameraRig found");
	}

	/// <summary>
	/// カメラのワールド座標の計算
	/// </summary>
	/// <param name="extrinsics"></param>
	/// <param name="mainCamera"></param>
	/// <returns></returns>
	public OVRPose ComputeCameraWorldSpacePose(OVRPlugin.CameraExtrinsics extrinsics, Camera mainCamera)
	{
		OVRPose trackingSpacePose = ComputeCameraTrackingSpacePose(extrinsics);
		OVRPose worldSpacePose = OVRExtensions.ToWorldSpacePose(trackingSpacePose, mainCamera);
		return worldSpacePose;
	}

	/// <summary>
	/// カメラトラッキングの計算
	/// </summary>
	/// <param name="extrinsics"></param>
	/// <returns></returns>
	public OVRPose ComputeCameraTrackingSpacePose(OVRPlugin.CameraExtrinsics extrinsics)
	{
		OVRPose trackingSpacePose = new OVRPose();

		OVRPose cameraTrackingSpacePose = extrinsics.RelativePose.ToOVRPose();
#if OVR_ANDROID_MRC
		OVRPose stageToLocalPose = OVRPlugin.GetTrackingTransformRelativePose(OVRPlugin.TrackingOrigin.Stage).ToOVRPose();
		cameraTrackingSpacePose = stageToLocalPose * cameraTrackingSpacePose;
#endif
		trackingSpacePose = cameraTrackingSpacePose;

		if (extrinsics.AttachedToNode != OVRPlugin.Node.None && OVRPlugin.GetNodePresent(extrinsics.AttachedToNode))
		{
			if (usingLastAttachedNodePose)
			{
				Debug.Log("The camera attached node get tracked");
				usingLastAttachedNodePose = false;
			}
			OVRPose attachedNodePose = OVRPlugin.GetNodePose(extrinsics.AttachedToNode, OVRPlugin.Step.Render).ToOVRPose();
			lastAttachedNodePose = attachedNodePose;
			trackingSpacePose = attachedNodePose * trackingSpacePose;
		}
		else
		{
			if (extrinsics.AttachedToNode != OVRPlugin.Node.None)
			{
				if (!usingLastAttachedNodePose)
				{
					Debug.LogWarning("The camera attached node could not be tracked, using the last pose");
					usingLastAttachedNodePose = true;
				}
				trackingSpacePose = lastAttachedNodePose * trackingSpacePose;
			}
		}

		return trackingSpacePose;
	}

}

#endif
