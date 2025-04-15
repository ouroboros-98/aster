namespace Aster.Core.Gameplay.Towers
{
    public abstract class BaseTower : AsterMono
    {
        protected EntityHp HP;
        protected float angle;

        public abstract void OnLightRayHit(LightRay ray);
    }
}