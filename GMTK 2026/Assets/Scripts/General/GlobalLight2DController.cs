using UnityEngine;

public class GlobalLight2DController : Light2DController
{
    public static GlobalLight2DController INST;

    protected override void Initialize()
    {
        INST = this;
        base.Initialize();
    }
}
