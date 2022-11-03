using System.Collections;
using UnityEngine;

namespace Assets.CodeBase.Infrasstructure
{
    public interface ICoroutineRunner
    {
        Coroutine StartCoroutine(IEnumerator coroutine);
    }
}