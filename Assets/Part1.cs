using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using System.IO;
using System.Globalization;

public class Part1 : MonoBehaviour
{
    public TMP_InputField Cost;
    public TMP_InputField Deposit;
    public TMP_Text Change;
    public TMP_Text Denominations;

    public Toggle USA, UK, Euro;
    CultureInfo currentCulture = new("en-US");
    string CultInfo;

    decimal deposit;
    decimal cost;
    int totalChange;

    Dictionary<string, int> ukDictionary;
    Dictionary<string, int> euroDictionary;
    Dictionary<string, int> usDictionary;

    string[] ukCoins;
    string[] euroCoins;
    string[] usCoins;

    // Start is called before the first frame update
    void Start()
    {
        Cost.characterLimit = 6;
        Deposit.characterLimit = 6;

        ukCoins = ReadCoinsFile("UK.txt");
        ukDictionary = SetupDictionary(ukCoins);

        euroCoins = ReadCoinsFile("euros.txt");
        euroDictionary = SetupDictionary(euroCoins);

        usCoins = ReadCoinsFile("US.txt");
        usDictionary = SetupDictionary(usCoins);
    }

    string[] ReadCoinsFile(string fileName)
    {
        string coinsPath = Application.dataPath + "/Text_Files/" + fileName;
        string[] coins = File.ReadAllLines(coinsPath);
        return coins;
    }

    Dictionary<string, int> SetupDictionary(string[] coins)
    {
        int arrayLength = coins.Length / 2;
        var dictionary = new Dictionary<string, int>();

        for (int i = 0; i < arrayLength * 2; i += 2)
        {
            dictionary.Add(coins[i], Convert.ToInt32(coins[i + 1]));
        }

        return dictionary;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ChangeCalculate()
    {
        if (!string.IsNullOrEmpty(Cost.text) && !string.IsNullOrEmpty(Deposit.text))
        {
            if (USA.isOn)
            {
                CultInfo = "en-US";
                Calculations(usDictionary);
            }
            else if (Euro.isOn)
            {
                CultInfo = "fr-FR";
                Calculations(euroDictionary);
            }
            else
            {
                CultInfo = "en-GB";
                Calculations(ukDictionary);
            }
        }
        else
        {
            Denominations.text = "Please enter the cost and deposit";
        }
    }

    void Calculations(Dictionary<string, int> dictionary)
    {
        currentCulture = new CultureInfo(CultInfo);
        Denominations.text = "";

        if (Cost.text.Contains(","))
            Cost.text = Cost.text.Replace(",", ".");
        if (Deposit.text.Contains(","))
            Deposit.text = Deposit.text.Replace(",", ".");

        deposit = Convert.ToDecimal(Deposit.text);
        cost = Convert.ToDecimal(Cost.text);

        Change.text = string.Format(currentCulture, "{0:C}", (deposit - cost));

        totalChange = (int)((deposit - cost) * 100);
        int[] denominations = new int[dictionary.Count];
        string[] names = new string[dictionary.Count];

        int i = 0;
        foreach (KeyValuePair<string, int> kvp in dictionary)
        {
            names[i] = kvp.Key;
            denominations[i] = kvp.Value;
            i++;
        }

        int[] change = new int[denominations.Length];

        for (i = 0; i < denominations.Length; i++)
        {
            change[i] = totalChange / denominations[i];
            totalChange -= (change[i] * denominations[i]);
        }

        for (i=0;i<denominations.Length;i++)
        {
            Denominations.text += names[i] + ": " + change[i] + "\n";
        }
    }
}
