using Aster.Core;
using Aster.Light;
using UnityEngine;

namespace Aster.Towers.Prism
{
    public class Prism : Splitter
    {
        protected override void AssignParametersFromConfig(AsterConfiguration config)
        {
            SplitterParameters configParams                  = config.Towers.Prism;
            if (IsNotNull(configParams)) _splitterParameters = configParams;
        }
    }
}