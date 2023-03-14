using Mod.Xmap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class NpcInMap
{
    public static bool isEnabled = true;
    static int maxLength = 0;
    static int lineHeight = 8;
    static int fontSize = 6;
    static int distanceBetweenLines = lineHeight + 1;
    static int x = 0;
    static int y = 165;

    public static void Paint(mGraphics g)
    {
        if (!isEnabled)
            return;
        GUIStyle[] styles = new GUIStyle[10];
        maxLength = 0;
        for (int i = 0; i < GameScr.vNpc.size(); i++)
        {
            Npc npc = (Npc)GameScr.vNpc.elementAt(i);
            styles[i] = new GUIStyle(GUI.skin.label)
            {
                alignment = TextAnchor.UpperRight,
                fontSize = fontSize * mGraphics.zoomLevel,
                fontStyle = FontStyle.Bold,
            };
            styles[i].normal.textColor = Color.yellow;
            if (Char.myCharz().npcFocus != null && Char.myCharz().npcFocus.template.npcTemplateId == npc.template.npcTemplateId)
            {
                styles[i].normal.textColor = Color.red;
            }
            int length = getWidth(styles[i], $" {i + 1}. {npc.template.name} [{npc.template.npcTemplateId}] ");
            maxLength = Math.max(length, maxLength);

        }
        int xDraw = GameCanvas.w - x - maxLength;

        for (int i = 0; i < GameScr.vNpc.size(); i++)
        {
            Npc npc = (Npc)GameScr.vNpc.elementAt(i);

            int yDraw = y + distanceBetweenLines * i;
            if (GameCanvas.isMouseFocus(xDraw, yDraw, maxLength, lineHeight))
            {
                g.fillRectPT(xDraw, yDraw, maxLength, lineHeight, 0, 0.8f);
            }
            else
            {
                g.fillRectPT(xDraw, yDraw, maxLength, lineHeight, 0, 0.5f);
            }
            g.drawString($"{i + 1}. {npc.template.name} [{npc.template.npcTemplateId}] ", -x, mGraphics.zoomLevel - 3 + yDraw, styles[i]);
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

        for (int i = 0; i < GameScr.vNpc.size(); i++)
        {
            Npc npc = (Npc)GameScr.vNpc.elementAt(i);

            if (GameCanvas.isPointerHoldIn(GameCanvas.w - x - maxLength, y + distanceBetweenLines * i, maxLength, lineHeight))
            {
                GameCanvas.isPointerJustDown = false;
                GameScr.gI().isPointerDowning = false;
                if (GameCanvas.isPointerClick)
                {
                    Utilities.teleToNpc(npc);
                    Char.myCharz().npcFocus = npc;
                }
                GameCanvas.clearAllPointerEvent();
                return;
            }
        }
    }
}