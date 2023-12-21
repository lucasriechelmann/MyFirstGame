using System.Collections.Generic;
using System;

namespace Engine2D.Objects;

public interface IGameObjectPool<T> where T : BaseGameObject
{
    List<T> ActiveObjects { get; }

    T GetOrCreate(Func<T> createNbObjectFn);

    void DeactivateObject(T gameObject, Action<T> postDeactivateFn);
    void DeactivateAllObjects(Action<T> postDeactivateFn);
    void DeactivateAllObjects();
}
