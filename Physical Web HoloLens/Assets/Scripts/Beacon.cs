using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beacon : MonoBehaviour {
    string address { get; set; }
    List<int> strength { get; set; }
    string output { get; set; }

    public Beacon(string address, int strength, string output)
    {
        this.address = address;
        this.strength.Add(strength);
        this.output = output;
    }

    public string getAddress()
    {
        return address;
    }

    public void addStrength(int strength)
    {
        this.strength.Add(strength);
    }
}
