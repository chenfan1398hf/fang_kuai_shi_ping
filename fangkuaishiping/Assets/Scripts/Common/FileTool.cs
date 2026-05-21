using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

public class FileTool
{
    public static PlayerData PlayerData { get { return GameManager.instance.playerData; } }
    public static string Read_Txt(string filename)
    {
        string path = string.Empty;
        if (PlayerData.playerLocalizeState == 1)
        {
            path = "CN/";
        }
        else if (PlayerData.playerLocalizeState == 2)
        {
            path = "EN/";
        }
        
        TextAsset ts = Resources.Load<TextAsset>("GameCfg/"+ path + filename);
        if (ts != null) return ts.text;
        return null;
    }
}
