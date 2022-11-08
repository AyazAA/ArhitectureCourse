using System.Collections;
using UnityEngine;

namespace CodeBase.Infrasstructure
{
    public interface ICoroutineRunner
    {
        Coroutine StartCoroutine(IEnumerator coroutine);
    }
}