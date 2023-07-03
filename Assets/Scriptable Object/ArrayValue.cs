using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu]

public class ArrayValue : ScriptableObject, ISerializationCallbackReceiver
{
    public int[] initialValue = new int[2];
    public int[] defaultValue = new int[2];

    public void OnAfterDeserialize() { initialValue = defaultValue; }

    public void OnBeforeSerialize() { }
}
