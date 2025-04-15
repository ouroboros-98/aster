using Aster.Core.Entity;
using Aster.Light;

namespace Aster.Core.Gameplay.Towers
{
    public abstract class BaseTower : AsterMono
    {
        protected EntityHP HP;
        protected float angle;

        public abstract void OnLightRayHit(LightRay ray);
    }
}