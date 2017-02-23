using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeaconManager : MonoBehaviour {
    List<Beacon> beacons { get; set; }

	// Use this for initialization
	void Start () {
        beacons = new List<Beacon>();
    }

    /// <summary>
    /// Update the beacon list
    /// If the beacon does not exist yet, add a new one
    /// Else add an updated strength value to the beacon
    /// </summary>
    /// <param name="address">Unique bluetooth address</param>
    /// <param name="strength">Signal strength</param>
    /// <param name="output">Eddystone URL</param>
    void  Update(string address, int strength, string output)
    {
        // If beacon does not exist yet, create a new one
        if(!existInList(address))
        {                                      
            Beacon beacon = new Beacon(address, strength, output);
            beacons.Add(beacon);
        } else // Beacon already exists. Add an updated strength value to it
        {
            Beacon beacon = getBeacon(address);
            beacon.addStrength(strength);
        }
    }

    /// <summary>
    /// Check if the beacon already exists in the list with beacons
    /// </summary>
    /// <param name="address">Unique bluetooth address</param>
    /// <returns>If the beacon already exists in the list of beacons</returns>
    bool existInList(string address)
    {
        foreach(Beacon beacon in beacons)
        {
            if (beacon.getAddress().Equals(address))
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
    Beacon getBeacon(string address)
    {
        foreach (Beacon beacon in beacons)
        {
            if (beacon.getAddress().Equals(address))
            {
                return beacon;
            }
        }

        return null;
    }
}
