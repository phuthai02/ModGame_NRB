using Mod.Xmap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace Mod.Boss
{
    public class Boss
    {
        public string name;

        public string map;

        public bool dead;

        public int mapId;

        public int zoneId = -1;

        public DateTime AppearTime;

        public static List<Boss> bosses = new List<Boss>();

        public static bool isEnabled = true;

        static int distanceBetweenLines = 8;

        static int offset = 0;

        static int x = 0;

        static int y = 70;

        static int maxLength = 0;

        static int lastBoss = -1;

        Boss(string name, string map, bool dead)
        {
            this.name = name;
            this.map = map;
            mapId = GetMapID(map, name);
            AppearTime = DateTime.Now;
            this.dead = dead;
        }

        public static void AddBoss(string chatVip)
        {
            if (!chatVip.StartsWith("BOSS"))
                return;
            chatVip = chatVip.Replace("BOSS ", "").Replace(" vừa xuất hiện tại ", "|").Replace(" appear at ", "|").Replace(" khu vực ", "|").Replace(" zone ", "|");
            string[] array = chatVip.Split('|');
            bosses.Add(new Boss(array[0].Trim(), array[1].Trim(), false));
            if (array.Length == 3)
                bosses.Last().zoneId = int.Parse(array[2].Trim());
            if (bosses.Count > 50)
                bosses.RemoveAt(0);
        }

        public override string ToString()
        {
            TimeSpan timeSpan = DateTime.Now.Subtract(AppearTime);
            string result = $"{name} - {map} [{mapId}] - ";
            if (zoneId > -1)
                result += $"khu {zoneId} - ";
            if (!dead)
            {
                int hours = (int)System.Math.Floor((decimal)timeSpan.TotalHours);
                if (hours > 0)
                    result += $"{hours}h";
                if (timeSpan.Minutes > 0)
                    result += $"{timeSpan.Minutes}p";
                result += $"{timeSpan.Seconds}s";
            }
            else
            {
                result += $"die";
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
            for (int i = start - offset; i < bosses.Count - offset; i++)
            {
                styles[i - start + offset] = new GUIStyle(GUI.skin.label)
                {
                    alignment = TextAnchor.UpperRight,
                    fontSize = 6 * mGraphics.zoomLevel,
                    fontStyle = FontStyle.Bold,
                };
                Boss boss = bosses[i];
                styles[i - start + offset].normal.textColor = Color.yellow;
                if (TileMap.mapID == boss.mapId)
                {
                    styles[i - start + offset].normal.textColor = new Color(1f, .5f, 0);
                    for (int j = 0; j < GameScr.vCharInMap.size(); j++)
                        if (((Char)GameScr.vCharInMap.elementAt(j)).cName == boss.name && !boss.dead)
                        {
                            styles[i - start + offset].normal.textColor = Color.red;

                            break;
                        }
                }
                if (boss.dead)
                {
                    styles[i - start + offset].normal.textColor = Color.green;
                }
                int length = getWidth(styles[i - start + offset], $"{i + 1}. {boss} ");
                maxLength = Math.max(length, maxLength);
            }
            int xDraw = GameCanvas.w - x - maxLength;
            for (int i = start - offset; i < bosses.Count - offset; i++)
            {
                int yDraw = y + distanceBetweenLines * (i - start + offset);
                Boss boss = bosses[i];
                g.setColor(new Color(0.2f, 0.2f, 0.2f, 0.4f));
                if (GameCanvas.isMouseFocus(xDraw, yDraw, maxLength, 7)) g.setColor(new Color(0.2f, 0.2f, 0.2f, 0.7f));
                g.fillRect(xDraw, yDraw + 1, maxLength, 7);
                g.drawString($"{i + 1}. {boss} ", -x, mGraphics.zoomLevel - 3 + yDraw, styles[i - start + offset]);
            }
            if (bosses.Count > 5)
            {
                if (offset < bosses.Count - 5)
                    g.drawRegion(Mob.imgHP, 0, 0, 9, 6, 1, GameCanvas.w - x - 9, y - 7, 0);
                if (offset > 0)
                    g.drawRegion(Mob.imgHP, 0, 0, 9, 6, 0, GameCanvas.w - x - 9, y + 2 + distanceBetweenLines * 5, 0);
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
                if (GameCanvas.isPointerHoldIn(GameCanvas.w - x - maxLength, y + 1 + distanceBetweenLines * (i - start + offset), maxLength, 7))
                {
                    GameCanvas.isPointerJustDown = false;
                    GameScr.gI().isPointerDowning = false;
                    if (GameCanvas.isPointerClick)
                    {
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
                                return;
                            }
                            if (bosses[i].zoneId != -1 && TileMap.zoneID != bosses[i].zoneId)
                            {
                                Service.gI().requestChangeZone(bosses[i].zoneId, 0);
                                return;
                            }
                        }
                        else
                            lastBoss = i;
                        if (TileMap.mapID == bosses[i].mapId)
                        {
                            int j = 0;
                            for (; j < GameScr.vCharInMap.size(); j++)
                            {
                                Char ch = GameScr.vCharInMap.elementAt(j) as Char;
                                if (ch.cName == bosses[i].name)
                                {
                                    Char.myCharz().deFocusNPC();
                                    Char.myCharz().itemFocus = null;
                                    Char.myCharz().mobFocus = null;
                                    if (Char.myCharz().charFocus != ch)
                                        Char.myCharz().charFocus = ch;
                                    else
                                        teleportMyChar(ch);
                                    break;
                                }
                            }
                            if (j == GameScr.vCharInMap.size())
                                GameScr.info1.addInfo("Boss không có trong khu!", 0);
                        }
                    }
                    GameCanvas.clearAllPointerEvent();
                    return;
                }
            }
            if (bosses.Count > 5)
            {
                if (GameCanvas.isPointerHoldIn(GameCanvas.w - x - 9, y - 7, 9, 6))
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
        public static void teleportMyChar(IMapObject obj)
        {
            LoadMap.teleportMyChar(obj.getX(), obj.getY());
        }
        public static long GetLastTimePress()
        {
            return (long)typeof(GameCanvas).GetField("lastTimePress", BindingFlags.NonPublic | BindingFlags.Static).GetValue(null);
        }
        public static void Update()
        {
            foreach (Boss boss in bosses)
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
}
