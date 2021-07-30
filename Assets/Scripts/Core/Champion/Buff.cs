using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Record all possible affects of any gameobjects
public class Buff
{
    public float startTime;
    public float duration;
    public int controlType; // -1 if this buff doesn't offer control
    public float[] affectedAmount; // Record the positive/negative affects on heros. 0=maxHP,1=ATK,2=DEF,3=AGI

    public Buff(float startTime, float duration, int controlType, float[] affectedAmount)
    {
        this.startTime = startTime;
        this.duration = duration;
        this.controlType = controlType;
        this.affectedAmount = affectedAmount;
    }
}
