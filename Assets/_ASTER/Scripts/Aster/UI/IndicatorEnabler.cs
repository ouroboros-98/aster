using Aster.Core;
using Aster.Core.UI;
using UnityEngine;

public class IndicatorEnabler : AsterMono
{
    [SerializeField] private PopUpIndicator indicator;

    private void Awake()
    {
        indicator.SetEnabled(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            indicator.SetEnabled(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            indicator.SetEnabled(false);
    }
}