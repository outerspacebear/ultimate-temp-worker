using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum CreaturePartType { Head = 0, Torso = 1, Bottom = 2 }

public class CreaturePart : MonoBehaviour
{
    public CreaturePartType type;
    public Element elementalAffinity;
}
