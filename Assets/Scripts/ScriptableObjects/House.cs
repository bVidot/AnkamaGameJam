using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class House : ScriptableObject {

    private int nbOfFailMax = 3;
    public int nbOfFail = 0;
    public enum DoorType { Level1, Level2, Level3 };
    public DoorType doorType;
    public int impatience;
    public int budget = 0;
    public bool isLocked;

    public void IncreaseFail()
    {
        nbOfFail++;
        if (nbOfFail >= nbOfFailMax)
            isLocked = true;
    }

}
