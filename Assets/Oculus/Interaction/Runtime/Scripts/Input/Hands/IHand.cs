/************************************************************************************
Copyright : Copyright (c) Facebook Technologies, LLC and its affiliates. All rights reserved.

Your use of this SDK or tool is subject to the Oculus SDK License Agreement, available at
https://developer.oculus.com/licenses/oculussdk/

Unless required by applicable law or agreed to in writing, the Utilities SDK distributed
under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF
ANY KIND, either express or implied. See the License for the specific language governing
permissions and limitations under the License.
************************************************************************************/

using UnityEngine;
using System;

namespace Oculus.Interaction.Input
{
    public interface IHand
    {
        Handedness Handedness { get; }

        bool IsConnected { get; }

        /// <summary>
        /// 손은 연결되고 추적되며, root pose의 추적 데이터는 높은 신뢰도로 표시됩니다.
        /// 이 값이 참이면 IsConnected 및 IsRootPoseValid도 참임을 의미하므로 이 항목 외에 확인할 필요가 없습니다.
        /// The hand is connected and tracked, and the root pose's tracking data is marked as
        /// high confidence.
        /// If this is true, then it implies that IsConnected and IsRootPoseValid are also true,
        /// so they don't need to be checked in addition to this.
        /// </summary>
        bool IsHighConfidence { get; }

        bool IsDominantHand { get; }
        float Scale { get; }
        bool GetFingerIsPinching(HandFinger finger);
        bool GetIndexFingerIsPinching();

        /// <summary>
        /// <see cref=GetPointerPose"/>를 통해 검색할 수 있는 포인터 포즈가 있으면 true를 반환합니다.
        /// Will return true if a pointer pose is available, that can be retrieved via
        /// <see cref="GetPointerPose"/>
        /// </summary>
        bool IsPointerPoseValid { get; }

        /// <summary>
        /// worldSpace에서 raycasting의 root로 사용할 수 있는 포즈 계산 시도.
        /// 유효한 추적 데이터가 없으면 false 리턴
        /// Attempts to calculate the pose that can be used as a root for raycasting, in world space
        /// Returns false if there is no valid tracking data.
        /// </summary>
        bool GetPointerPose(out Pose pose);

        /// <summary>
        /// worldSpace에서 요청된 손 관절의 포즈 계산을 시도합니다.
        /// 골격이 아직 초기화되지 않았거나, 유효한 추적 데이터가 없는 경우 false를 반환합니다.
        /// Attempts to calculate the pose of the requested hand joint, in world space.
        /// Returns false if the skeleton is not yet initialized, or there is no valid
        /// tracking data.
        /// </summary>
        bool GetJointPose(HandJointId handJointId, out Pose pose);

        /// <summary>
        /// localSpace에서 요청된 손 관절의 포즈 계산을 시도합니다.
        /// 골격이 아직 초기화되지 않았거나, 유효한 추적 데이터가 없는 경우 false를 반환합니다.
        /// Attempts to calculate the pose of the requested hand joint, in local space.
        /// Returns false if the skeleton is not yet initialized, or there is no valid
        /// tracking data.
        /// </summary>
        bool GetJointPoseLocal(HandJointId handJointId, out Pose pose);

        /// <summary>
        /// 각 관절의 로컬 포즈가 들어있는 배열을 반환합니다.
        /// 그 포즈는 root 포즈나 손의 크기가 적용되어 있지 않습니다.
        /// Returns an array containing the local pose of each joint. The poses
        /// do not have the root pose applied, nor the hand scale. It is in the same coordinate
        /// system as the hand skeleton.
        /// </summary>
        /// <param name="localJointPoses">The array with the local joint poses.
        /// It will be empty if no poses where found</param>
        /// <returns>
        /// True if the poses collection was correctly populated. False otherwise.
        /// </returns>
        bool GetJointPosesLocal(out ReadOnlyHandJointPoses localJointPoses);

        /// <summary>
        /// Attempts to calculate the pose of the requested hand joint relative to the wrist.
        /// Returns false if the skeleton is not yet initialized, or there is no valid
        /// tracking data.
        /// </summary>
        bool GetJointPoseFromWrist(HandJointId handJointId, out Pose pose);

        /// <summary>
        /// Returns an array containing the pose of each joint relative to the wrist. The poses
        /// do not have the root pose applied, nor the hand scale. It is in the same coordinate
        /// system as the hand skeleton.
        /// </summary>
        /// <param name="jointPosesFromWrist">The array with the joint poses from the wrist.
        /// It will be empty if no poses where found</param>
        /// <returns>
        /// True if the poses collection was correctly populated. False otherwise.
        /// </returns>
        bool GetJointPosesFromWrist(out ReadOnlyHandJointPoses jointPosesFromWrist);

        /// <summary>
        /// Obtains palm pose in local space.
        /// </summary>
        /// <param name="pose">The pose to populate</param>
        /// <returns>
        /// True if pose was obtained.
        /// </returns>
        bool GetPalmPoseLocal(out Pose pose);

        bool GetFingerIsHighConfidence(HandFinger finger);
        float GetFingerPinchStrength(HandFinger finger);

        /// <summary>
        /// True if the hand is currently tracked, thus tracking poses are available for the hand
        /// root and finger joints.
        /// This property does not indicate pointing pose validity, which has its own property:
        /// <see cref="IsPointerPoseValid"/>.
        /// </summary>
        bool IsTrackedDataValid { get; }

        /// <summary>
        /// Gets the root pose of the wrist, in world space.
        /// Will return true if a pose was available; false otherwise.
        /// Confidence level of the pose is exposed via <see cref="IsHighConfidence"/>.
        /// </summary>
        bool GetRootPose(out Pose pose);

        /// <summary>
        /// Will return true if an HMD Center Eye pose available, that can be retrieved via
        /// <see cref="GetCenterEyePose"/>
        /// </summary>
        bool IsCenterEyePoseValid { get; }

        /// <summary>
        /// Gets the pose of the center eye (HMD), in world space.
        /// Will return true if a pose was available; false otherwise.
        /// </summary>
        bool GetCenterEyePose(out Pose pose);

        /// <summary>
        /// The transform that was applied to all tracking space poses to convert them to world
        /// space.
        /// </summary>
        Transform TrackingToWorldSpace { get; }

        /// <summary>
        /// Incremented every time the source tracking or state data changes.
        /// </summary>
        int CurrentDataVersion { get; }

        /// <summary>
        /// An Aspect provides additional functionality on top of what the HandState provides.
        /// The underlying hand is responsible for finding the most appropriate component.
        /// It is usually, but not necessarily, located within the same GameObject as the
        /// underlying hand.
        /// For example, this method can be used to source the SkinnedMeshRenderer representing the
        /// hand, if one exists.
        /// <returns>true if an aspect of the requested type was found, false otherwise</returns>
        /// </summary>
        bool GetHandAspect<TComponent>(out TComponent foundComponent) where TComponent : class;

        event Action WhenHandUpdated;
    }
}
