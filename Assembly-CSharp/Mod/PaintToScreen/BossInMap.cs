using Mod.Xmap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class BossInMap // Danh sách boss trong map
{
    public static bool isEnabled = true;
    static int maxLength = 0;
    static int lineHeight = 8;
    static int fontSize = 6;
    static int distanceBetweenLines = lineHeight + 1;
    static int x = 0;
    static int y = 105;

    public static List<Char> bossInMaps = new List<Char>();


    public static void Paint(mGraphics g)
    {
        if (!isEnabled)
            return;

        bossInMaps.Clear();
        for (int i = 0; i < GameScr.vCharInMap.size(); i++)
        {
            Char @char = (Char)GameScr.vCharInMap.elementAt(i);
            char name = char.Parse(@char.cName.Substring(0, 1));
            if (name >= 'A' && name <= 'Z' && !@char.cName.StartsWith("Đệ tử"))
            {
                bossInMaps.Add(@char);
            };
        }

        GUIStyle[] styles = new GUIStyle[10];
        maxLength = 0;
        for (int i = 0; i < bossInMaps.Count; i++)
        {
            Char @char = (Char)bossInMaps[i];
            styles[i] = new GUIStyle(GUI.skin.label)
            {
                alignment = TextAnchor.UpperRight,
                fontSize = fontSize * mGraphics.zoomLevel,
                fontStyle = FontStyle.Bold,
            };
            styles[i].normal.textColor = Color.yellow;

            int length = getWidth(styles[i], $"{i + 1}. {@char.cName} - {NinjaUtil.getMoneys(@char.cHP)} [{Convert.ToInt64(@char.cHP) * 100 / Convert.ToInt64(@char.cHPFull)}%]  [{(@char.cgender == 0 ? "TD" : (@char.cgender == 1 ? "NM" : "XD"))}]");
            maxLength = Math.max(length, maxLength);
        }
        int xDraw = GameCanvas.w - x - maxLength;


        for (int i = 0; i < bossInMaps.Count; i++)
        {
            Char @char = (Char)bossInMaps[i];

            int yDraw = y + distanceBetweenLines * i;

            if (GameCanvas.isMouseFocus(xDraw, yDraw, maxLength, lineHeight))
            {
                g.fillRectPT(xDraw, yDraw, maxLength, lineHeight, 0, 0.8f);
            }
            else
            {
                g.fillRectPT(xDraw, yDraw, maxLength, lineHeight, 0, 0.5f);
            }

            g.drawString($"{i + 1}. {@char.cName} - {NinjaUtil.getMoneys(@char.cHP)} [{Convert.ToInt64(@char.cHP) * 100 / Convert.ToInt64(@char.cHPFull)}%]  [{(@char.cgender == 0 ? "TD" : (@char.cgender == 1 ? "NM" : "XD"))}] ", -x, mGraphics.zoomLevel - 3 + yDraw, styles[i]);
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
        for (int i = 0; i < bossInMaps.Count; i++)
        {
            Char @char = (Char)bossInMaps[i];
            if (GameCanvas.isPointerHoldIn(GameCanvas.w - x - maxLength, y + distanceBetweenLines * i, maxLength, lineHeight))
            {
                GameCanvas.isPointerJustDown = false;
                GameScr.gI().isPointerDowning = false;
                if (GameCanvas.isPointerClick)
                {
                    Utilities.teleportMyChar(@char.cx, @char.cy);
                    Char.myCharz().charFocus = @char;
                }
                GameCanvas.clearAllPointerEvent();
                return;
            }
        }
    }
}

