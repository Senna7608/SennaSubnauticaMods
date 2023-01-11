using System.Collections;
using UnityEngine;
#pragma warning disable CS1591

namespace SMLExpander
{
    public class FragmentTracker : MonoBehaviour
    {
        PingInstance pingInstance;
        PrefabIdentifier prefabIdentifier;
        private bool isSignalReady = false;

        public void Awake()
        {
            pingInstance = gameObject.EnsureComponent<PingInstance>();
            pingInstance.origin = transform;

            prefabIdentifier = gameObject.GetComponent<PrefabIdentifier>();

            if (prefabIdentifier == null)
            {
                DestroyImmediate(this);
            }

            isSignalReady = true;
        }

        public void OnDestroy()
        {
            if (pingInstance != null)
            {
                PingManager.Unregister(pingInstance);
            }
        }

        public void OnEnable()
        {
            StartCoroutine(EnableSignalAsync());
        }

        private IEnumerator EnableSignalAsync()
        {
            while (!isSignalReady)
            {
                yield return null;
            }


            pingInstance.displayPingInManager = true;
            pingInstance.pingType = PingType.Signal;
            pingInstance.visible = true;
            pingInstance.minDist = 5;
            pingInstance.range = 10;
            pingInstance.SetType(PingType.Signal);
            pingInstance.SetLabel(prefabIdentifier.ClassId);
            pingInstance.SetColor(2);

            yield break;
        }
    }

    /*
    public class FragmentTracker : MonoBehaviour
    {
        GameObject signal;
        PingInstance pingInstance;
        TechTag techTag;

        public void Awake()
        {
            //Debug.Log($"Spawning fragment at position: {transform.position}");

            signal = Instantiate(Resources.Load<GameObject>("worldentities/environment/generated/signal"));

            techTag = gameObject.GetComponent<TechTag>();
            
            SignalPing signalPing = signal.GetComponent<SignalPing>();
            signalPing.enabled = false;

            pingInstance = signal.GetComponent<PingInstance>();

            pingInstance.displayPingInManager = true;
            pingInstance.pingType = PingType.Signal;
            pingInstance.visible = true;
            pingInstance.minDist = 5;
            pingInstance.maxDist = 10;
            pingInstance.origin = transform;            
        }

        public void OnDestroy()
        {
            if (pingInstance != null)
            {
                PingManager.Unregister(pingInstance);
            }
        }

        public void OnEnable()
        {
            pingInstance.SetLabel(techTag.type.ToString());
            pingInstance.SetColor(2);
        }
    */
}