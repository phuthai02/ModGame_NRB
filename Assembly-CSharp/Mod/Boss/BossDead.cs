using Mod.Boss;
using Mod.Xmap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;


public class BossDead
{
    public string bossName;
    public string charName;
    public static bool isEnabled = true;
    static int offset = 0;
    static int maxLength = 0;
    static int distanceBetweenLines = 8;
    static int x = 0;
    static int y = 128;
  
    BossDead(string bossName, string charName)
    {
        this.bossName = bossName;
        this.charName = charName;
    }
    public static List<BossDead> bossdeads = new List<BossDead>();


    public static void AddBossDead(string chatVip)
    {
        if (!chatVip.StartsWith("Người chơi"))
            return;
        chatVip = chatVip.Replace("Người chơi ", "").Replace(" đã tiêu diệt BOSS ", "_").Replace(" mọi người đều ngưỡng mộ", "");
        string[] array = chatVip.Split('_');
        bossdeads.Add(new BossDead(array[1].Trim(), array[0].Trim()));
        for(int i = 0;i< Boss.bosses.Count;i++)
        {
            Boss boss = Boss.bosses[i];
            if(boss.name == array[1].Trim() && !boss.dead)
            {
                Boss.bosses[i].dead = true;
                break;
            }
        }
        if (bossdeads.Count > 50)
            bossdeads.RemoveAt(0);
    }

    public static void Paint(mGraphics g)
    {
        if (!isEnabled)
            return;
        int start = 0;
        if (bossdeads.Count > 5)
            start = bossdeads.Count - 5;

        GUIStyle[] styles = new GUIStyle[5];
        for (int i = start - offset; i < bossdeads.Count - offset; i++)
        {
            styles[i - start + offset] = new GUIStyle(GUI.skin.label)
            {
                alignment = TextAnchor.UpperRight,
                fontSize = 6 * mGraphics.zoomLevel,
                fontStyle = FontStyle.Bold,
            };
            BossDead boss = bossdeads[i];

            styles[i - start + offset].normal.textColor = Color.yellow;

            int length = getWidth(styles[i - start + offset], $"{boss.charName} đã bú {boss.bossName} ");
            maxLength = Math.max(length, maxLength);
        }
        int xDraw = GameCanvas.w - x - maxLength;

        for (int i = start - offset; i < bossdeads.Count - offset; i++)
        {
            int yDraw = y + distanceBetweenLines * (i - start + offset);
            BossDead boss = bossdeads[i];
            g.setColor(new Color(0.2f, 0.2f, 0.2f, 0.1f));
            if (GameCanvas.isMouseFocus(xDraw, yDraw, maxLength, 7)) g.setColor(new Color(0.2f, 0.2f, 0.2f, 0.1f));
            g.fillRect(xDraw, yDraw + 1, maxLength, 7);
            g.drawString($"{boss.charName} đã bú {boss.bossName} ", -x, mGraphics.zoomLevel - 3 + yDraw, styles[i - start + offset]);
        }
        if (bossdeads.Count > 5)
        {
            if (offset < bossdeads.Count - 5)
                g.drawRegion(Mob.imgHP, 0, 0, 9, 6, 1, GameCanvas.w - x - 9, y - 7, 0);
            if (offset > 0)
                g.drawRegion(Mob.imgHP, 0, 0, 9, 6, 0, GameCanvas.w - x - 9, y + 2 + distanceBetweenLines * 5, 0);
        }
    }

    internal static int getWidth(GUIStyle gUIStyle, string s)
    {
        return (int)(gUIStyle.CalcSize(new GUIContent(s)).x * 1.05f / mGraphics.zoomLevel);
    }

    public static void Update()
    {
        if (isEnabled && GameCanvas.isMouseFocus(GameCanvas.w - x - maxLength, y + 1, maxLength, 8 * 5))
        {
            if (GameCanvas.pXYScrollMouse > 0)
                if (offset < bossdeads.Count - 5)
                    offset++;
            if (GameCanvas.pXYScrollMouse < 0)
                if (offset > 0)
                    offset--;
        }
    }

    public static void UpdateTouch()
    { 
        if (!isEnabled)
            return;
        if (!GameCanvas.isTouch || ChatTextField.gI().isShow || GameCanvas.menu.showMenu)
            return;
        int start = 0;
        if (bossdeads.Count > 5)
            start = bossdeads.Count - 5;
       
        if (bossdeads.Count > 5)
        {
            if (GameCanvas.isPointerHoldIn(GameCanvas.w - x - 9, y - 7, 9, 6))
            {
                GameCanvas.isPointerJustDown = false;
                GameScr.gI().isPointerDowning = false;
                if (GameCanvas.isPointerClick)
                {
                    if (offset < bossdeads.Count - 5)
                        offset++;
                }
                GameCanvas.clearAllPointerEvent();
                return;
            }
            if (GameCanvas.isPointerHoldIn(GameCanvas.w - x - 9, y + 2 + distanceBetweenLines * 5, 9, 6))
            {
                GameCanvas.isPointerJustDown = false;
                GameScr.gI().isPointerDowning = false;
                if (GameCanvas.isPointerClick)
                {
                    if (offset > 0)
                        offset--;
                }
                GameCanvas.clearAllPointerEvent();
                return;
            }
        }
    }
}

