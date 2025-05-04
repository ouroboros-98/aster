using Aster.Core;
using UnityEngine;

namespace _ASTER.Scripts.Aster.UI
{
    public class KeepUpright: AsterMono
    {
        Vector3   originalWorldOffset;

        void Start()
        {
            // record the initial world-space offset from the parent
            originalWorldOffset = transform.position - transform.parent.position;
        }

        void LateUpdate()
        {
            // 1) lock world position:
            transform.position = transform.parent.position + originalWorldOffset;
        }
    }

}