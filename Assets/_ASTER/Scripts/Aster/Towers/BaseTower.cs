using Aster.Core.Entity;

namespace Aster.Core.Gameplay.Towers
{
    public abstract class BaseTower : AsterMono
    {
        private EntityHP HP;
        private float angle;

        public abstract void OnLightRayHit(LightRay ray);
    }
}