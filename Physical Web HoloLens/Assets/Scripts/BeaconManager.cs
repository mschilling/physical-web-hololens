using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeaconManager : MonoBehaviour {
    List<Beacon> beacons { get; set; }
    int beaconInRange = 60;
    int beaconStrengthNumersInAverage = 3;

	// Use this for initialization
	void Start () {
        beacons = new List<Beacon>();
    }

    void Update ()
    {
        Beacon foundBeaconInRange = closeToBeacon();
        if(foundBeaconInRange != null)
        {
            System.Diagnostics.Debug.WriteLine("BeaconInRange: " + foundBeaconInRange.getAddress());
        }
    }

    /// <summary>
    /// Update the beacon list
    /// If the beacon does not exist yet, add a new one
    /// Else add an updated strength value to the beacon
    /// </summary>
    /// <param name="address">Unique bluetooth address</param>
    /// <param name="strength">Signal strength</param>
    /// <param name="output">Eddystone URL</param>
    public void UpdateBeacon(ulong address, int strength, string output)
    {
        // If beacon does not exist yet, create a new one
        if(!existInList(address))
        {                                      
            Beacon beacon = new Beacon(address, strength, output);
            beacons.Add(beacon);
            System.Diagnostics.Debug.WriteLine("Added Beacon: " + address);
        } else // Beacon already exists. Add an updated strength value to it
        {
            Beacon beacon = getBeacon(address);
            beacon.addStrength(strength);
            System.Diagnostics.Debug.WriteLine("Updated Beacon: " + address);
        }
    }

    /// <summary>
    /// Check if the beacon already exists in the list with beacons
    /// </summary>
    /// <param name="address">Unique bluetooth address</param>
    /// <returns>If the beacon already exists in the list of beacons</returns>
    bool existInList(ulong address)
    {
        foreach(Beacon beacon in beacons)
        {
            if (beacon.getAddress() == address)
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Get the beacon for the given unique bluetooth address
    /// Beacon should already be in the list
    /// </summary>
    /// <param name="address">Unique bluetooth address</param>
    /// <returns>Beacon data that belongs to the given address else null</returns>
    Beacon getBeacon(ulong address)
    {
        foreach (Beacon beacon in beacons)
        {
            if (beacon.getAddress() == address)
            {
                return beacon;
            }
        }

        return null;
    }

    Beacon closeToBeacon()
    {
        foreach (Beacon beacon in beacons)
        {
            if (beacon.getStrength(beaconStrengthNumersInAverage) <= beaconInRange)
            {
                return beacon;
            }
        }
        return null;
    }
}
