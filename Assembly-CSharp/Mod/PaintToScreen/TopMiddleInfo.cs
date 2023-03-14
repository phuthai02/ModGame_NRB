using Mod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using UnityEngine;

public class TopMiddleInfo
{
    public static void Paint(mGraphics g)
    {
        string nhapNhay = File.ReadAllText("Mod/Data/nhapNhay.txt");
        if (GameCanvas.gameTick / (int)Time.timeScale % 10 != 0 && GameCanvas.gameTick / (int)Time.timeScale % 10 != 1 && GameCanvas.gameTick / (int)Time.timeScale % 10 != 2 &&
        GameCanvas.gameTick / (int)Time.timeScale % 10 != 3 && GameCanvas.gameTick / (int)Time.timeScale % 10 != 4)
        {
            mFont.tahoma_7b_red.drawString(g, nhapNhay, GameCanvas.w / 2 - 20, 5, 2, mFont.tahoma_7b_blue);
        }
        else
        {
            mFont.tahoma_7b_yellow.drawString(g, nhapNhay, GameCanvas.w / 2 - 20, 5, 2, mFont.tahoma_7b_red);
        }

        int heightTop = 17;

        mFont.tahoma_7_grey.drawString(g, "Toạ độ X: " + Char.myCharz().cx + " - Y: " + Char.myCharz().cy, GameCanvas.w / 2 - 20, heightTop, 2);
        heightTop += 10;

        mFont.tahoma_7_grey.drawString(g, "Thời gian: " + DateTime.Now.ToString("HH:mm:ss tt"), GameCanvas.w / 2 - 20, heightTop, 2);
        heightTop += 10;

        mFont.tahoma_7_grey.drawString(g, TileMap.mapNames[TileMap.mapID] + " [" + TileMap.mapID + "]" + " - Khu: " + TileMap.zoneID, GameCanvas.w / 2 - 20, heightTop, 2);
        heightTop += 10;

        if (AutoSpecial.isSpecial)
        {
            mFont.tahoma_7b_red.drawString(g, "Đang mở nội tại: " + AutoSpecial.nameSpecial + " - Max: " + AutoSpecial.maxSpecial + "%", GameCanvas.w / 2 - 20, heightTop, 2);
            heightTop += 10;
        }
    }
}
