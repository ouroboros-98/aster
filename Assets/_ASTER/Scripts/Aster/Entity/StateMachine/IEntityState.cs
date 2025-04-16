using Aster.Entity;
using Aster.StateMachine;

namespace _ASTER.Scripts.Aster.Entity.StateMachine
{
    public interface IEntityState : IState
    {
    }

    public abstract class BaseEntityState : IEntityState
    {
        protected BaseEntityController Entity;

        protected BaseEntityState(BaseEntityController entity)
        {
            Entity = entity;
        }

        public virtual void OnEnter()
        {
        }

        public virtual void Update()
        {
        }

        public virtual void FixedUpdate()
        {
        }

        public virtual void OnExit()
        {
        }
    }
}