using Mod.Xmap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using UnityEngine;

public class BossAppeared // Danh sách boss
{
    public string name;

    public string map;

    public string dead;

    public int mapId;

    public int zoneId = -1;

    public DateTime AppearTime;

    public static List<BossAppeared> bosses = new List<BossAppeared>();

    public static bool isEnabled = true;

    static int fontSize = 6;

    static int lineHeight = 8;

    static int distanceBetweenLines = lineHeight + 1;

    static int offset = 0;

    static int x = 0;

    static int y = 50;

    static int maxLength = 0;

    static int lastBoss = -1;

    static int mapIDChangZone = -1;

    static int zoneIDChangZone = -1;


    BossAppeared(string name, string map, string dead)
    {
        this.name = name;
        this.map = map;
        mapId = GetMapID(map, name);
        AppearTime = DateTime.Now;
        this.dead = dead;
    }

    public static void AddBoss(string chatVip)
    {
        if (chatVip.StartsWith("BOSS"))
        {
            chatVip = chatVip.Replace("BOSS ", "").Replace(" vừa xuất hiện tại ", "|").Replace(" appear at ", "|").Replace(" khu vực ", "|").Replace(" zone ", "|");
            string[] array = chatVip.Split('|');
            bosses.Add(new BossAppeared(array[0].Trim(), array[1].Trim(), string.Empty));
            if (array.Length == 3)
                bosses.Last().zoneId = int.Parse(array[2].Trim());
            if (bosses.Count > 50)
                bosses.RemoveAt(0);
        }
        if (chatVip.StartsWith("Người chơi"))
        {
            chatVip = chatVip.Replace("Người chơi ", "").Replace(" đã tiêu diệt BOSS ", "_").Replace(" mọi người đều ngưỡng mộ", "");
            string[] array = chatVip.Split('_');
            for (int i = 0; i < bosses.Count; i++)
            {
                BossAppeared boss = bosses[i];
                if (boss.name == array[1].Trim() && boss.dead == string.Empty)
                {
                    bosses[i].dead = array[0].Trim();
                    break;
                }
            }
        }
    }

    public override string ToString()
    {
        TimeSpan timeSpan = DateTime.Now.Subtract(AppearTime);
        string result = $"{name} - {map} [{mapId}] - ";

        if (dead == string.Empty)
        {
            if (zoneId > -1)
                result += $"khu {zoneId} - ";
            int hours = (int)System.Math.Floor((decimal)timeSpan.TotalHours);
            if (hours > 0)
                result += $"{hours}h";
            if (timeSpan.Minutes > 0)
                result += $"{timeSpan.Minutes}p";
            result += $"{timeSpan.Seconds}s";
        }
        else
        {
            result += $"{dead} đã bú";
        }
        return result;
    }

    public static void Paint(mGraphics g)
    {
        if (!isEnabled)
            return;
        int start = 0;
        if (bosses.Count > 5)
            start = bosses.Count - 5;
        GUIStyle[] styles = new GUIStyle[5];
        maxLength = 0;
        for (int i = start - offset; i < bosses.Count - offset; i++)
        {
            styles[i - start + offset] = new GUIStyle(GUI.skin.label)
            {
                alignment = TextAnchor.UpperRight,
                fontSize = fontSize * mGraphics.zoomLevel,
                fontStyle = FontStyle.Bold,
            };
            BossAppeared boss = bosses[i];
            styles[i - start + offset].normal.textColor = Color.yellow;
            if (TileMap.mapID == boss.mapId)
            {
                styles[i - start + offset].normal.textColor = new Color(1f, .5f, 0);
                for (int j = 0; j < GameScr.vCharInMap.size(); j++)
                    if (((Char)GameScr.vCharInMap.elementAt(j)).cName == boss.name && boss.dead == string.Empty)
                    {
                        styles[i - start + offset].normal.textColor = Color.red;

                        break;
                    }
            }
            if (boss.dead != string.Empty)
            {
                styles[i - start + offset].normal.textColor = Color.green;
            }
            int length = getWidth(styles[i - start + offset], $"{i + 1}. {boss}");
            maxLength = Math.max(length, maxLength);
        }
        int xDraw = GameCanvas.w - x - maxLength;
        for (int i = start - offset; i < bosses.Count - offset; i++)
        {
            int yDraw = y + distanceBetweenLines * (i - start + offset);
            BossAppeared boss = bosses[i];
            if (GameCanvas.isMouseFocus(xDraw, yDraw, maxLength, lineHeight))
            {
                g.fillRectPT(xDraw, yDraw, maxLength, lineHeight, 0, 0.8f);
            }
            else
            {
                g.fillRectPT(xDraw, yDraw, maxLength, lineHeight, 0, 0.5f);
            }
            g.drawString($"{i + 1}. {boss} ", -x, mGraphics.zoomLevel - 3 + yDraw, styles[i - start + offset]);
        }
        if (bosses.Count > 5)
        {
            if (offset < bosses.Count - 5)
                g.drawRegion(Mob.imgHP, 0, 36, 9, 6, 1, GameCanvas.w - x - 11, y - 7, 0);
            if (offset > 0)
                g.drawRegion(Mob.imgHP, 0, 36, 9, 6, 0, GameCanvas.w - x - 11, y + distanceBetweenLines * 5, 0);
        }
    }

    static int GetMapID(string mapName, string bossName)
    {
        if (mapName.Contains("Xayda") && bossName.Contains("Thần"))
        {
            return 158;
        }
        if (mapName.Contains("Trái đất"))
        {
            return 156;
        }
        if (mapName.Contains("Namek"))
        {
            return 157;
        }
        if (bossName == "Rambo" && mapName.Equals("Rừng đá"))
        {
            return 77;
        }
        if (mapName.Equals("Sân sau siêu thị"))
        {
            return 104;
        }
        for (int i = 0; i < TileMap.mapNames.Length; i++)
        {
            if (TileMap.mapNames[i].Equals(mapName))
                return i;
        }

        return -1;
    }

    internal static int getWidth(GUIStyle gUIStyle, string s)
    {
        return (int)(gUIStyle.CalcSize(new GUIContent(s)).x * 1.05f / mGraphics.zoomLevel);
    }

    public static void UpdateTouch()
    {
        if (lastBoss != -1 && mSystem.currentTimeMillis() - GetLastTimePress() > 200)
            lastBoss = -1;
        if (!isEnabled)
            return;
        if (!GameCanvas.isTouch || ChatTextField.gI().isShow || GameCanvas.menu.showMenu)
            return;
        int start = 0;
        if (bosses.Count > 5)
            start = bosses.Count - 5;
        for (int i = start - offset; i < bosses.Count - offset; i++)
        {
            if (GameCanvas.isPointerHoldIn(GameCanvas.w - x - maxLength, y + distanceBetweenLines * (i - start + offset), maxLength, lineHeight))
            {
                GameCanvas.isPointerJustDown = false;
                GameScr.gI().isPointerDowning = false;
                //if (GameCanvas.isPointerClick) //double click
                //{
                if (lastBoss == i && mSystem.currentTimeMillis() - GetLastTimePress() <= 200)
                {
                    if (TileMap.mapID != bosses[i].mapId)
                    {
                        if (bosses[i].mapId == 95)
                        {
                            bosses[i].mapId = 96;
                        }
                        if (bosses[i].mapId == 78)
                        {
                            bosses[i].mapId = 79;
                        }
                        XmapController.StartRunToMapId(bosses[i].mapId);
                        lastBoss = -1;

                        mapIDChangZone = bosses[i].mapId;
                        zoneIDChangZone = bosses[i].zoneId;

                        if (zoneIDChangZone == -1 || TileMap.mapID == mapIDChangZone && TileMap.zoneID == zoneIDChangZone)
                        {
                            return;
                        }
                        new Thread(new ThreadStart(changeToZoneIDBoss)).Start();
                    }
                    else
                    {
                        GameScr.info1.addInfo("Đã đến map boss !", 0);
                    }
                }
                else
                    lastBoss = i;
                //}
                GameCanvas.clearAllPointerEvent();
                return;
            }
        }
        if (bosses.Count > 5)
        {
            if (GameCanvas.isPointerHoldIn(GameCanvas.w - x - 11, y - 7, 9, 6))
            {
                GameCanvas.isPointerJustDown = false;
                GameScr.gI().isPointerDowning = false;
                if (GameCanvas.isPointerClick)
                {
                    if (offset < bosses.Count - 5)
                        offset++;
                }
                GameCanvas.clearAllPointerEvent();
                return;
            }
            if (GameCanvas.isPointerHoldIn(GameCanvas.w - x - 11, y + 2 + distanceBetweenLines * 5, 9, 6))
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

    public static void changeToZoneIDBoss()
    {
        while (true)
        {
            if (TileMap.mapID == mapIDChangZone && TileMap.zoneID == zoneIDChangZone)
            {
                break;
            }
            if (TileMap.mapID == mapIDChangZone && zoneIDChangZone != -1 && TileMap.zoneID != zoneIDChangZone)
            {
                Service.gI().requestChangeZone(zoneIDChangZone, 0);
                Thread.Sleep(500);
            }
        }
        return;
    }


    public static void teleportMyChar(IMapObject obj)
    {
        Utilities.teleportMyChar(obj.getX(), obj.getY());
    }
    public static long GetLastTimePress()
    {
        return (long)typeof(GameCanvas).GetField("lastTimePress", BindingFlags.NonPublic | BindingFlags.Static).GetValue(null);
    }
    public static void Update()
    {
        foreach (BossAppeared boss in bosses)
        {
            if (boss.zoneId != -1)
                continue;
            for (int i = 0; i < GameScr.vCharInMap.size(); i++)
            {
                Char ch = GameScr.vCharInMap.elementAt(i) as Char;
                if (ch.cName == boss.name)
                {
                    boss.zoneId = TileMap.zoneID;
                    break;
                }
            }
            if (boss.zoneId == TileMap.zoneID)
                break;
        }
        if (isEnabled && GameCanvas.isMouseFocus(GameCanvas.w - x - maxLength, y + 1, maxLength, 8 * 5))
        {
            if (GameCanvas.pXYScrollMouse > 0)
                if (offset < bosses.Count - 5)
                    offset++;
            if (GameCanvas.pXYScrollMouse < 0)
                if (offset > 0)
                    offset--;
        }
    }

}

