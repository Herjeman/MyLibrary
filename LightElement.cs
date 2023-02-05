using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightElement : MonoBehaviour
{
    public int unlitLayer;
    public int litLayer;

    void Start()
    {
        gameObject.layer = unlitLayer;
    }

    public virtual bool EnterLight() // virtual so that inheriting classes can override to do any behaviour
    {
        gameObject.layer = litLayer;
        return true;
    }

    public virtual void ExitLight() // virtual so that inheriting classes can override to do any behaviour
    {
        gameObject.layer = unlitLayer;
    }

    public bool IsLit { get { return gameObject.layer == litLayer; } } // returns true if element is lit
}
