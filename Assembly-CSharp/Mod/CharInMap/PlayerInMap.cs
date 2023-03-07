using Mod.Boss;
using Mod.Xmap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class PlayerInMap

{
    public static bool isEnabled = true;
    static int maxLength = 0;
    static int distanceBetweenLines = 8;
    static int x = 0;
    static int y = 83;

    public static void Paint(mGraphics g)
    {
        if (!isEnabled || GameScr.vCharInMap.size() < 1)
            return;

        GUIStyle[] styles = new GUIStyle[30];

        int count = 0;
        for (int i = 0; i < GameScr.vCharInMap.size(); i++)
        {
            Char @char = (Char)GameScr.vCharInMap.elementAt(i);
            if (@char != null && @char.cTypePk == 5)
            {
                break;
            };
            styles[i] = new GUIStyle(GUI.skin.label)
            {
                alignment = TextAnchor.UpperLeft,
                fontSize = 6 * mGraphics.zoomLevel,
                fontStyle = FontStyle.Bold,
            };

            styles[i].normal.textColor = Color.yellow;
            if (Char.myCharz().charFocus != null && Char.myCharz().charFocus.charID == @char.charID)
            {
                styles[i].normal.textColor = Color.red;
            }
            int length = getWidth(styles[i], $"{count}. {@char.cName} - {NinjaUtil.getMoneys(@char.cHP)} [{Convert.ToInt64(@char.cHP) * 100 / Convert.ToInt64(@char.cHPFull)}%]  [{(@char.cgender == 0 ? "TD" : (@char.cgender == 1 ? "NM" : "XD"))}]");
            maxLength = Math.max(length, maxLength);
            count++;
        }
        int xDraw = 0;

        int count2 = 1;
        for (int i = 0; i < GameScr.vCharInMap.size(); i++)
        {
            Char @char = (Char)GameScr.vCharInMap.elementAt(i);
            if (@char != null && @char.cTypePk == 5)
            {
                break;
            };
            int yDraw = y + distanceBetweenLines * i;
            g.setColor(new Color(0.2f, 0.2f, 0.2f, 0.1f));
            if (GameCanvas.isMouseFocus(0, yDraw, maxLength, 7)) g.setColor(new Color(0.2f, 0.2f, 0.2f, 0.1f));
            g.fillRect(xDraw, yDraw + 1, maxLength, 7);
            g.drawString($" {count2}. {@char.cName} - {NinjaUtil.getMoneys(@char.cHP)} [{Convert.ToInt64(@char.cHP) * 100 / Convert.ToInt64(@char.cHPFull)}%]  [{(@char.cgender == 0 ? "TD" : (@char.cgender == 1 ? "NM" : "XD"))}]", xDraw, mGraphics.zoomLevel - 3 + yDraw, styles[i]);
            count2++;
        }
    }

    internal static int getWidth(GUIStyle gUIStyle, string s)
    {
        return (int)(gUIStyle.CalcSize(new GUIContent(s)).x * 1.05f / mGraphics.zoomLevel);
    }

    public static void UpdateTouch()
    {

        if (!isEnabled)
            return;
        if (!GameCanvas.isTouch || ChatTextField.gI().isShow || GameCanvas.menu.showMenu)
            return;

        for (int i = 0; i < GameScr.vCharInMap.size(); i++)
        {
            Char @char = (Char)GameScr.vCharInMap.elementAt(i);
            if (@char != null && @char.cTypePk == 5)
            {
                break;
            };
            if (GameCanvas.isPointerHoldIn(0, y + 1 + distanceBetweenLines * i, maxLength, 7))
            {
                GameCanvas.isPointerJustDown = false;
                GameScr.gI().isPointerDowning = false;
                if (GameCanvas.isPointerClick)
                {
                    LoadMap.teleportMyChar(@char.cx, @char.cy);
                    Char.myCharz().charFocus = @char;
                }
                GameCanvas.clearAllPointerEvent();
                return;
            }
        }
    }

}
