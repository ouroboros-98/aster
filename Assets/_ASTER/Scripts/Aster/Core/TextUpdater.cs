using System;
using Aster.Core;
using TMPro; // Required for TextMeshPro

using UnityEngine;

namespace _ASTER.Scripts.Aster.Core
{
    public class UpdateText : MonoBehaviour
    {
        public TMP_Text myText;
        private int _counter = 1;


        private void Start()
        {
            myText.text = _counter.ToString("D2");
        }

        public void OnEnable()
        {
            AsterEvents.Instance.OnLevelEnd += IncrementAndUpdateText;
        }

        private void OnDisable()
        {
            AsterEvents.Instance.OnLevelEnd -= IncrementAndUpdateText;

        }

        private void IncrementAndUpdateText(int obj)
        {

            _counter++;
            myText.text = _counter.ToString("D2"); // D2 means: at least 2 digits, pad with zero if needed

        }
    }
}