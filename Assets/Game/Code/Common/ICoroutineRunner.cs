using System.Collections;
using UnityEngine;

namespace Game.Code.Common
{
    public interface ICoroutineRunner
    {
        Coroutine StartCoroutine(IEnumerator coroutine);
    }
}