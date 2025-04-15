namespace Aster.Core.Gameplay.Towers
{
    public abstract class BaseTower : AsterMono
    {
        private EntityHp HP;
        private float angle;

        public abstract void OnLightRayHit(LightRay ray);
    }
}