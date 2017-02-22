using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

#if WINDOWS_UWP
using Windows.Devices.Bluetooth.Advertisement;
#endif

public class Scanner : MonoBehaviour {

    // Use this for initialization
    void Start () {
#if WINDOWS_UWP
        // Setup an Advertisement watcher
        BluetoothLEAdvertisementWatcher watcher = new BluetoothLEAdvertisementWatcher();
        watcher.Received += OnAdvertisementReceived;
        // Set the scanning mode to keep activily scanning for beacons
        watcher.ScanningMode = BluetoothLEScanningMode.Active;
        watcher.Start();
#endif
    }
	
	// Update is called once per frame
	void Update () {
		
	}

#if WINDOWS_UWP
    private void OnAdvertisementReceived(BluetoothLEAdvertisementWatcher watcher, BluetoothLEAdvertisementReceivedEventArgs eventArgs)
    {
        // We only need the scannable devices containing data
        if (eventArgs.AdvertisementType != BluetoothLEAdvertisementType.ConnectableUndirected || eventArgs.Advertisement.DataSections.Count < 3)
            return;

        // We need to encode an Eddystone URL to a readable format
        Encoding asciiEncoding = ASCIIEncoding.ASCII;

        // Debug all beacon data
        System.Diagnostics.Debug.WriteLine("=========================================================================================== NEW ADVERTISEMENT ===========================================================================================");
        System.Diagnostics.Debug.WriteLine("Address: " + eventArgs.BluetoothAddress);
        System.Diagnostics.Debug.WriteLine("Type: " + eventArgs.AdvertisementType);
        System.Diagnostics.Debug.WriteLine("Strength: " + eventArgs.RawSignalStrengthInDBm);
        System.Diagnostics.Debug.WriteLine("Datasections Count: " + eventArgs.Advertisement.DataSections.Count);
        System.Diagnostics.Debug.WriteLine("Flags: " + eventArgs.Advertisement.Flags);
        System.Diagnostics.Debug.WriteLine("LocalName: " + eventArgs.Advertisement.LocalName);
        System.Diagnostics.Debug.WriteLine("Uuids: " + eventArgs.Advertisement.ServiceUuids[0]);

        // Create a string variable to store the output
        string output = "";

        // Loop through all DataSections
        foreach (BluetoothLEAdvertisementDataSection data in eventArgs.Advertisement.DataSections)
        {
            // Read and store the data from a DataSections
            var dataReader = Windows.Storage.Streams.DataReader.FromBuffer(data.Data);
            byte[] fileContent = new byte[dataReader.UnconsumedBufferLength];
            dataReader.ReadBytes(fileContent);

            // Chech if the fileContent array is as long as the Eddystone format
            if (fileContent.Length == 18)
            {
                // Byte 0 and 1 are for the id
                // Byte 2 is the frame identifier
                // Byte 3 is the transmission power 
                // Get the Url Scheme from the 4th byte in the array
                byte urlSchemeByte = fileContent[4];
                output += getUrlScheme(urlSchemeByte);

                // We don't need the first 5 bytes from the array anymore
                byte[] newArray = new byte[fileContent.Length - 5];
                Buffer.BlockCopy(fileContent, 5, newArray, 0, newArray.Length);

                // Encode the Url
                string dataSectionOutput = asciiEncoding.GetString(newArray, 0, newArray.Length);

                // Add the encoded Url to the output
                output += dataSectionOutput;
            }
        }

        // Log the beacon output
        System.Diagnostics.Debug.WriteLine("Output: " + output);
    }
#endif


    /// <summary>
    /// Switch the urlSchemeByte to determine the correct UrlScheme.
    /// </summary>
    /// <param name="urlSchemeByte">a byte between 0 and 3</param>
    /// <returns>The corresponding UrlScheme</returns>
    private String getUrlScheme(byte urlSchemeByte)
    {
        switch(urlSchemeByte)
        {
            case 0: return "http://www.";
            case 1: return "https://www.";
            case 2: return "http://";
            case 3: return "https://";
            default: return "http://";
        }
    }
}
