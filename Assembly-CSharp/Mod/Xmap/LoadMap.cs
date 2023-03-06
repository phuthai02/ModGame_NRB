using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class LoadMap
{
    public static Waypoint waypointLeft;
    public static Waypoint waypointMiddle;
    public static Waypoint waypointRight;

    public static void teleportMyChar(int x, int y)
    {
        Char.myCharz().currentMovePoint = null;
        Char.myCharz().cx = x;
        Char.myCharz().cy = y;
        Service.gI().charMove();

        if (!ItemTime.isExistItem(521))
            return;

        Char.myCharz().cx = x;
        Char.myCharz().cy = y + 1;
        Service.gI().charMove();
        Char.myCharz().cx = x;
        Char.myCharz().cy = y;
        Service.gI().charMove();
        GameScr.info1.addInfo("teleportMyChar", 0);
    }

    public static void NextMap(int type)
    {
        if (type == 0) {
            changeMap(waypointLeft, false);
        } else if (type == 1)
        {
            changeMap(waypointMiddle, true);
        } else if (type == 2)
        {
            changeMap(waypointRight, false);
        }
    }


    public static string getTextPopup(PopUp popUp)
    {
        StringBuilder stringBuilder = new StringBuilder();
        for (int i = 0; i < popUp.says.Length; i++)
        {
            stringBuilder.Append(popUp.says[i]);
            stringBuilder.Append(" ");
        }
        return stringBuilder.ToString().Trim();
    }

    public static void setWaypointChangeMap(Waypoint waypoint)
    {
        int cMapID = TileMap.mapID;
        string textPopup = getTextPopup(waypoint.popup);

        if (cMapID == 53 && textPopup == "Tường thành 1")
            return;

        if (cMapID == 69 && textPopup == "Vực cấm" ||
            cMapID == 67 && textPopup == "Vực chết" ||
            cMapID == 106 && textPopup == "Rừng tuyết")
        {
            waypointLeft = waypoint;
            return;
        }

        if (((cMapID == 110 || cMapID == 110) && textPopup == "Hang băng") ||
            ((cMapID == 109 || cMapID == 109) && textPopup == "Rừng băng") ||
            (cMapID == 105 && textPopup == "Cánh đồng tuyết"))
        {
            waypointMiddle = waypoint;
            return;
        }

        if (cMapID == 71 && textPopup == "Căn cứ Raspberry")
        {
            waypointRight = waypoint;
            return;
        }

        if (waypoint.maxX < 60)
        {
            waypointLeft = waypoint;
            return;
        }

        if (waypoint.minX > TileMap.pxw - 60)
        {
            waypointRight = waypoint;
            return;
        }

        waypointMiddle = waypoint;

    }

    public static void updateWaypointChangeMap()
    {
        waypointLeft = waypointMiddle = waypointRight = null;

        var vGoSize = TileMap.vGo.size();
        for (int i = 0; i < vGoSize; i++)
        {
            Waypoint waypoint = (Waypoint)TileMap.vGo.elementAt(i);
            setWaypointChangeMap(waypoint);
        }
    }


    public static int getXWayPoint(Waypoint waypoint)
    {
        return waypoint.maxX < 60 ? 15 :
            waypoint.minX > TileMap.pxw - 60 ? TileMap.pxw - 15 :
            waypoint.minX + 30;
    }

    public static int getYWayPoint(Waypoint waypoint)
    {
        return waypoint.maxY;
    }

    public static void requestChangeMap(Waypoint waypoint)
    {
        if (waypoint.isOffline)
        {
            Service.gI().getMapOffline();
            return;
        }
        Service.gI().requestChangeMap();
    }


    public static void changeMap(Waypoint waypoint, bool isWaypointMiddle)
    {
        if (waypoint != null)
        {
            teleportMyChar(getXWayPoint(waypoint), getYWayPoint(waypoint));
            if (isWaypointMiddle)
            {
                requestChangeMap(waypoint);
            }
        }
        else
        {
            GameScr.info1.addInfo("Hết map", 0);
        }

    }

}
