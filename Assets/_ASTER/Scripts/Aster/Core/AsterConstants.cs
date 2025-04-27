namespace Aster.Core
{
    public class AsterConstants
    {
        public readonly float          LIGHT_RAY_Y = 0.3670001f;
        private static  AsterConstants _instance;

        public static AsterConstants Instance
        {
            get
            {
                if (_instance == null) _instance = new AsterConstants();

                return _instance;
            }
        }
    }
}