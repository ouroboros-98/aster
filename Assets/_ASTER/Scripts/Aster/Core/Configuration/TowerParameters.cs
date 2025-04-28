using Aster.Towers;
using Aster.Towers.Destabilizer;
using Aster.Utils.Attributes;
using NaughtyAttributes;
using UnityEngine;

namespace Aster.Core
{
    public partial class AsterConfiguration
    {
        [SerializeField, BoxedProperty] private TowerConfigurations towers;
        public                                  TowerConfigurations Towers => towers;

        [System.Serializable]
        public class TowerConfigurations
        {
            [SerializeField, Label("Splitter")]     private SplitterParameters     splitterParameters;
            [SerializeField, Label("Prism")]        private SplitterParameters     prismParameters;
            [SerializeField, Label("Destabilizer")] private DestabilizerParameters destabilizerParameters;

            public SplitterParameters     Splitter     => splitterParameters;
            public SplitterParameters     Prism        => prismParameters;
            public DestabilizerParameters Destabilizer => destabilizerParameters;
        }
    }
}