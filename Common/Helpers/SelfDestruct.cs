﻿using UnityEngine;
#pragma warning disable CS1591

namespace Common.Helpers
{
    public class SelfDestruct : MonoBehaviour
    {
        private bool isRoot = false;

        private void Start()
        {
            isRoot = transform.parent == null ? true : false;
        }

        private void Update()
        {
            if (isRoot && Vector3.Distance(transform.position, Vector3.zero) > 8000)
            {
                Destroy(gameObject);
            }
        }
    }
}
