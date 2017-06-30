using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAsset
{
    string Name { get; }
    Object LoadAsset(string name);
    void Dispose(bool force = false);
}
