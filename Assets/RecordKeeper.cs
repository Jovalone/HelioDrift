using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecordKeeper : MonoBehaviour
{
    public bool Instance;
    public Dictionary<string, int> statistics;
    public static RecordKeeper Record;

    // Start is called before the first frame update
    void Start()
    {
        if (Instance)
        {
            Record = this;
        }
        statistics = new Dictionary<string, int>();

                //Totals

        //Monster Kills
        statistics.Add("SquidsKilledTotal", 0);
        statistics.Add("SharksKilledTotal", 0);
        statistics.Add("SnakesKilledTotal", 0);

        //Ship Kills
        statistics.Add("Faction1FighterKillsTotal", 0);
        statistics.Add("Faction1TankKillsTotal", 0);

        statistics.Add("Faction2FighterKillsTotal", 0);
        statistics.Add("Faction2TankKillsTotal", 0);

        //Bullet Shots
        statistics.Add("Bullet0Shots", 0);
        statistics.Add("Bullet1Shots", 0);
        statistics.Add("Bullet2Shots", 0);
        statistics.Add("Bullet3Shots", 0);
        statistics.Add("Bullet4Shots", 0);
        statistics.Add("Bullet5Shots", 0);

        //Bullet Kills
        statistics.Add("Bullet6Kills", 0);
        statistics.Add("Bullet1Kills", 0);
        statistics.Add("Bullet2Kills", 0);
        statistics.Add("Bullet3Kills", 0);
        statistics.Add("Bullet4Kills", 0);
        statistics.Add("Bullet5Kills", 0);

                //Temporary values

        //Monster Kills
        statistics.Add("SquidKillsRecent", 0);
        statistics.Add("SharkKillsRecent", 0);
        statistics.Add("SnakeKillsRecent", 0);

        //Ship Kills
        statistics.Add("Faction1FighterKillsRecent", 0);
        statistics.Add("Faction1TankKillsRecent", 0);

        statistics.Add("Faction2FighterKillsRecent", 0);
        statistics.Add("Faction2TankKillsRecent", 0);

        //Sightings
        statistics.Add("MantaRaySightings", 0);

    }

    public void saveInfo()
    {
        //Monster Kills
        statistics["SquidsKilledTotal"] += statistics["SquidKillsRecent"];
        statistics["SharksKilledTotal"] += statistics["SharkKillsRecent"];
        statistics["SnakesKilledTotal"] += statistics["SnakeKillsRecent"];

        //Ship Kills
        statistics["Faction1FighterKillsTotal"] += statistics["Faction1FighterKillsRecent"];
        statistics["Faction1TankKillsTotal"] = statistics["Faction1TankKillsRecent"];

        statistics["Faction2FighterKillsTotal"] += statistics["Faction2FighterKillsRecent"];
        statistics["Faction2TankKillsTotal"] += statistics["Faction2TankKillsRecent"];

        //reset temp info
        //Monster Kills
        statistics["SquidKillsRecent"] = 0;
        statistics["SharkKillsRecent"] = 0;
        statistics["SnakeKillsRecent"] = 0;

        //Ship Kills
        statistics["Faction1FighterKillsRecent"] = 0;
        statistics["Faction1TankKillsRecent"] = 0;

        statistics["Faction2FighterKillsRecent"] = 0;
        statistics["Faction2TankKillsRecent"] = 0;

        //Sightings
        statistics["MantaRaySightings"] = 0;

        foreach (var stat in statistics)
        {
            Debug.Log("key: " + stat.Key + " Quantity: " + stat.Value + "\n");
        }
    }
}
