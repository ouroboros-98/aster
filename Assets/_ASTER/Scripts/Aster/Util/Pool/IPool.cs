using Aster.Core;
using UnityEngine;

namespace Aster.Utils.Pool
{
    public interface IPool<T> where T : AsterMono, IPoolable
    {
        T    Get();
        T    Get(Vector3     position, Quaternion rotation);
        void Return(T        obj);
        void ReturnDelayed(T obj, float delay);
    }
}