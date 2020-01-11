using UnityEngine;
using Debuggers.UE._SealedClasses;
using Debuggers.UE._Structures;

namespace Debuggers.UE._Object._Component._Behaviour
{
    public static class _Animator
    {
        public static void DebugAnimator(this Animator animator, string prefixString = null)
        {
            if (prefixString == null)
            {
                DLog.Log($"\n[Animator] ({animator.name})");
                prefixString = "this";
            }

            DLog.Log($"{prefixString}.angularVelocity: {animator.angularVelocity}");
            DLog.Log($"{prefixString}.applyRootMotion: {animator.applyRootMotion}");

            animator.avatar.DebugAvatar($"{prefixString}.avatar");

            DLog.Log($"{prefixString}.bodyPosition: {animator.bodyPosition}");
            DLog.Log($"{prefixString}.bodyRotation: {animator.bodyRotation}");
            DLog.Log($"{prefixString}.cullingMode: {animator.cullingMode}");
            DLog.Log($"{prefixString}.deltaPosition: {animator.deltaPosition}");
            DLog.Log($"{prefixString}.deltaRotation: {animator.deltaRotation}");
            DLog.Log($"{prefixString}.feetPivotActive: {animator.feetPivotActive}");
            DLog.Log($"{prefixString}.fireEvents: {animator.fireEvents}");
            DLog.Log($"{prefixString}.gameObject: {animator.gameObject.name}");
            DLog.Log($"{prefixString}.parameterCount: {animator.parameterCount}");

            for (int i = 0; i < animator.parameters.Length; i++)
            {                
                animator.parameters[i].DebugAnimatorControllerParameter($"{prefixString}.parameters[{i}]");
            }

            DLog.Log($"{prefixString}.pivotPosition: {animator.pivotPosition}");
            DLog.Log($"{prefixString}.pivotWeight: {animator.pivotWeight}");

            animator.playableGraph.DebugPlayableGraph($"{prefixString}.playableGraph");

            DLog.Log($"{prefixString}.playbackTime: {animator.playbackTime}");
            DLog.Log($"{prefixString}.recorderMode: {animator.recorderMode}");
            DLog.Log($"{prefixString}.recorderStartTime: {animator.recorderStartTime}");
            DLog.Log($"{prefixString}.recorderStopTime: {animator.recorderStopTime}");
            DLog.Log($"{prefixString}.rightFeetBottomHeight: {animator.rightFeetBottomHeight}");
            DLog.Log($"{prefixString}.rootPosition: {animator.rootPosition}");
            DLog.Log($"{prefixString}.rootRotation: {animator.rootRotation}");

            animator.runtimeAnimatorController.DebugRuntimeAnimatorController($"{prefixString}.runtimeAnimatorController");

            DLog.Log($"{prefixString}.tag: {animator.tag}");
            DLog.Log($"{prefixString}.targetPosition: {animator.targetPosition}");
            DLog.Log($"{prefixString}.targetRotation: {animator.targetRotation}");
            DLog.Log($"{prefixString}.transform: {animator.transform.name}");
            DLog.Log($"{prefixString}.updateMode: {animator.updateMode}");
            DLog.Log($"{prefixString}.velocity: {animator.velocity}");
        }
    }
}
