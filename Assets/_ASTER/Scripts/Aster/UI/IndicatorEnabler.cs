using Aster.Core;
using Aster.Core.UI;
using Aster.Entity.Player;
using UnityEngine;

public class IndicatorEnabler : AsterMono
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            other.gameObject.GetComponent<PlayerIndicator>().SetIndicator(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            other.gameObject.GetComponent<PlayerIndicator>().SetIndicator(false);
    }
}