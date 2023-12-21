using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Enigma : MonoBehaviour
{
    [Header(nameof(Enigma))]
    [SerializeField] protected Mechanism mechanism;
    [SerializeField] UnityEvent OnEnigmaCompleted;
    protected bool completed;

    public virtual void CompleteEnigma()
    {
        completed = true;
        mechanism.Unlock();
        OnEnigmaCompleted.Invoke();
    }

    public abstract bool Completed();
}
