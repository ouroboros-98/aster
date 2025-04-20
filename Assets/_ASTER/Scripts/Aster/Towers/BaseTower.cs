using Aster.Core;
using Aster.Core.Entity;
using Aster.Light;

namespace Aster.Towers
{
    public abstract class BaseTower : BaseLightHittable
    {
        protected EntityHP HP;
        protected float    angle;
    }
}