using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beacon : MonoBehaviour {
    ulong address { get; set; }
    List<int> strength = new List<int>();
    string output { get; set; }

    public Beacon(ulong address, int strength, string output)
    {
        this.address = address;
        this.strength.Add(strength);
        this.output = output;
    }

    public ulong getAddress()
    {
        return address;
    }

    public void addStrength(int strength)
    {
        this.strength.Add(strength);
    }

    public int getStrength(int fromIndex)
    {
        int length = strength.Count;
        int average = 0;
        if(length < fromIndex)
        {
            return int.MaxValue;
        } else
        {
            for(int i = length - fromIndex; i < length; i++)
            {
                int strengthIndex = strength[i];
                average += strengthIndex;
            }

            average = average / fromIndex;
            return average;
        }
    }
}
