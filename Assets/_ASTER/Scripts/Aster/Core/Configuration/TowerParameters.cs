using Aster.Towers;
using Aster.Towers.Destabilizer;
using Aster.Utils.Attributes;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Aster.Core
{
    public partial class AsterConfiguration
    {
        private TowerConfigurations towers = new();
        public  TowerConfigurations Towers => towers;

        [BoxGroup("Towers")]
        [LabelText("Splitter")]
        [SerializeField]
        private SplitterParameters splitterParameters;

        [BoxGroup("Towers")]
        [LabelText("Prism")]
        [SerializeField]
        private SplitterParameters prismParameters;

        [BoxGroup("Towers")]
        [LabelText("Destabilizer")]
        [SerializeField]
        private DestabilizerParameters destabilizerParameters;

        public class TowerConfigurations
        {
            public AsterConfiguration _config;

            public SplitterParameters     Splitter     => _config.splitterParameters;
            public SplitterParameters     Prism        => _config.prismParameters;
            public DestabilizerParameters Destabilizer => _config.destabilizerParameters;
        }
    }
}