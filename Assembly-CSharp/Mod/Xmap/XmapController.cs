using System.Collections.Generic;

namespace Mod.Xmap
{
    public class XmapController : IActionListener
    {
        private const int TIME_DELAY_NEXTMAP = 200;
        private const int TIME_DELAY_RENEXTMAP = 500;
        private const int ID_ITEM_CAPSULE_VIP = 194;
        private const int ID_ITEM_CAPSULE = 193;
        private const int ID_ICON_ITEM_TDLT = 4387;

        private static readonly XmapController _Instance = new XmapController();

        private static int IdMapEnd;
        private static List<int> WayXmap;
        private static int IndexWay;
        private static bool IsNextMapFailed;
        private static bool IsWait;
        private static long TimeStartWait;
        private static long TimeWait;
        private static bool IsWaitNextMap;

        public static void Update()
        {
            if (IsWaiting())
                return;

            if (XmapData.Instance().IsLoading)
                return;

            if (IsWaitNextMap)
            {
                Wait(TIME_DELAY_NEXTMAP);
                IsWaitNextMap = false;
                return;
            }

            if (IsNextMapFailed)
            {
                XmapData.Instance().MyLinkMaps = null;
                WayXmap = null;
                IsNextMapFailed = false;
                return;
            }

            if (WayXmap == null)
            {
                GameScr.info1.addInfo("Chạy qua: " + TileMap.mapNames[IdMapEnd], 0);
                if (XmapData.Instance().MyLinkMaps == null)
                {
                    XmapData.Instance().LoadLinkMaps();
                    return;
                }
                WayXmap = XmapAlgorithm.FindWay(TileMap.mapID, IdMapEnd);
                IndexWay = 0;
                if (WayXmap == null)
                {
                    GameScr.info1.addInfo("Không thể tìm thấy đường đi", 0);
                    FinishXmap();
                    return;
                }
            }

            if (TileMap.mapID == WayXmap[WayXmap.Count - 1] && !XmapData.IsMyCharDie() /*|| TileMap.mapID ==158 && !XmapData.IsMyCharDie()*/)
            {
                GameScr.info1.addInfo("Hết xăng", 0);
                FinishXmap();
                return;
            }

            if (TileMap.mapID == WayXmap[IndexWay])
            {
                if (XmapData.IsMyCharDie())
                {
                    Service.gI().returnTownFromDead();
                    IsWaitNextMap = IsNextMapFailed = true;
                }
                else if (XmapData.CanNextMap())
                {
                    NextMap(WayXmap[IndexWay + 1]);
                    IsWaitNextMap = true;
                }
                Wait(TIME_DELAY_RENEXTMAP);
                return;
            }

            if (TileMap.mapID == WayXmap[IndexWay + 1])
            {
                IndexWay++;
                return;
            }

            IsNextMapFailed = true;
        }

        public void perform(int idAction, object p)
        {
            switch (idAction)
            {
                case 1:
                    List<int> idMaps = (List<int>)p;
                    ShowPanelXmap(idMaps);
                    break;
            }
            Char.chatPopup = null;
        }

        private static void Wait(int time)
        {
            IsWait = true;
            TimeStartWait = mSystem.currentTimeMillis();
            TimeWait = time;
        }

        private static bool IsWaiting()
        {
            if (IsWait && mSystem.currentTimeMillis() - TimeStartWait >= TimeWait)
                IsWait = false;
            return IsWait;
        }

        #region Thao tác của xmap
        public static void ShowXmapMenu()
        {
            XmapData.Instance().LoadGroupMapsFromFile("Mod\\TextData\\GroupMapsXmap.txt");
            MyVector myVector = new MyVector();
            foreach (var groupMap in XmapData.Instance().GroupMaps)
                myVector.addElement(new Command(groupMap.NameGroup, _Instance, 1, groupMap.IdMaps));
            GameCanvas.menu.startAt(myVector, 3);
            //ChatPopup.addChatPopup($"XmapNRO by Phucprotein\nMap hiện tại: {TileMap.mapName}, ID: {TileMap.mapID}\nVui lòng chọn nơi muốn đến", 100000, new Npc(5, 0, -100, 100, 5, Utilities.ID_NPC_MOD_FACE));
        }

        public static string get_map_name(int id)
        {
            try
            {
                switch (id)
                {
                    case 92:
                        return "Thành phố phía đông";
                    case 93:
                        return "Thành phố phía nam";
                    case 94:
                        return "Đảo Balê";
                    case 96:
                        return "Cao nguyên";
                    case 97:
                        return "Thành phố phía bắc";
                    case 98:
                        return "Ngọn núi phía bắc";
                    case 99:
                        return "Thung lũng phía bắc";
                    case 100:
                        return "Thị trấn Ginder";
                    case 102:
                        return "Nhà Bunma";
                    case 103:
                        return "Võ đài Xên bọ hung";

                    case 105:
                        return "Cánh đồng tuyết";
                    case 106:
                        return "Rừng tuyết";
                    case 107:
                        return "Núi tuyết";
                    case 108:
                        return "Dòng sông băng";
                    case 109:
                        return "Rừng băng";
                    case 110:
                        return "Hang băng";





                    default:
                        return TileMap.mapNames[id];
                }
            }
            catch
            {
                return "";
            }
        }




        public static void ShowPanelXmap(List<int> idMaps)
        {
            Xmappp.IsMapTransAsXmap = true;
            int len = idMaps.Count;
            GameCanvas.panel.mapNames = new string[len];
            GameCanvas.panel.planetNames = new string[len];
            for (int i = 0; i < len; i++)
            {
                string nameMap = get_map_name(idMaps[i]);
                GameCanvas.panel.mapNames[i] = idMaps[i] + ": " + nameMap;
                //GameCanvas.panel.planetNames[i] /*= TileMap.mapName + " " + TileMap.mapID;*/
            }
            GameCanvas.panel.setTypeMapTrans();
            GameCanvas.panel.show();
        }

        public static void StartRunToMapId(int idMap)
        {
            IdMapEnd = idMap;
            Xmappp.IsXmapRunning = true;
        }

        public static void FinishXmap()
        {
            Xmappp.IsXmapRunning = false;
            IsNextMapFailed = false;
            XmapData.Instance().MyLinkMaps = null;
            WayXmap = null;
        }

        public static void SaveIdMapCapsuleReturn()
        {
            Xmappp.IdMapCapsuleReturn = TileMap.mapID;
        }

        private static void NextMap(int idMapNext)
        {
            List<MapNext> mapNexts = XmapData.Instance().GetMapNexts(TileMap.mapID);
            if (mapNexts != null)
            {
                foreach (MapNext mapNext in mapNexts)
                {
                    if (mapNext.MapID == idMapNext)
                    {
                        NextMap(mapNext);
                        return;
                    }
                }
            }
            GameScr.info1.addInfo("Lỗi tại dữ liệu", 0);
        }

        private static void NextMap(MapNext mapNext)
        {
            switch (mapNext.Type)
            {
                case TypeMapNext.AutoWaypoint:
                    NextMapAutoWaypoint(mapNext);
                    break;
                case TypeMapNext.NpcMenu:
                    NextMapNpcMenu(mapNext);
                    break;
                case TypeMapNext.NpcPanel:
                    NextMapNpcPanel(mapNext);
                    break;
                case TypeMapNext.Position:
                    NextMapPosition(mapNext);
                    break;
                case TypeMapNext.Capsule:
                    NextMapCapsule(mapNext);
                    break;
            }
        }
        private static void Move(int x, int y)
        {
            Char.myCharz().currentMovePoint = new MovePoint(x, y);
        }
        private static void NextMapAutoWaypoint(MapNext mapNext)
        {
            Waypoint waypoint = XmapData.FindWaypoint(mapNext.MapID);
            if (waypoint != null)
            {
                int xx = waypoint.popup.cx;
                //int xy = waypoint.popup.cy;
                //int x = XmapData.GetPosWaypointX(waypoint);
                int y = XmapData.GetPosWaypointY(waypoint);
                //int y = waypoint.popup.cy;
                MoveMyChar(xx, y);
                Move(xx, y);
                if (mapNext.MapID != 158 && mapNext.MapID != 156 && mapNext.MapID != 157)
                {
                    RequestChangeMap(waypoint);
                }

            }
        }

        private static void NextMapNpcMenu(MapNext mapNext)
        {
            int idNpc = mapNext.Info[0];
            Service.gI().openMenu(idNpc);
            if (TileMap.mapID == 28)
            {
                Service.gI().confirmMenu((short)idNpc, 0);
                Service.gI().transportNow();
            }
            else
            {
                Service.gI().transportNow();
                for (int i = 1; i < mapNext.Info.Length; i++)
                {
                    Service.gI().transportNow();
                    int select = mapNext.Info[i];
                    Service.gI().confirmMenu((short)idNpc, (sbyte)select);

                }
            }


            //if (mewcon.npc1lan)
            //{
            //    Service.gI().confirmMenu((short)idNpc, (sbyte)0);
            //    Service.gI().transportNow();
            //    //TransportScr.gI().isSpeed = true;
            //    //GameCanvas.endDlg();
            //    //TransportScr.gI().center = null;
            //    mewcon.npc1lan = false;

            //}

            //for (int i = 1; i < mapNext.Info.Length; i++)
            //{
            //    int select = mapNext.Info[i];
            //    Service.gI().confirmMenu((short)idNpc, (sbyte)select);
            //}

        }

        private static void NextMapNpcPanel(MapNext mapNext)
        {
            int idNpc = mapNext.Info[0];
            int selectMenu = mapNext.Info[1];
            int selectPanel = mapNext.Info[2];
            Service.gI().openMenu(idNpc);
            Service.gI().confirmMenu((short)idNpc, (sbyte)selectMenu);
            Service.gI().requestMapSelect(selectPanel);
        }

        private static void NextMapPosition(MapNext mapNext)
        {
            int xPos = mapNext.Info[0];
            int yPos = mapNext.Info[1];
            MoveMyChar(xPos, yPos);
            Service.gI().requestChangeMap();
            Service.gI().getMapOffline();
        }

        private static void NextMapCapsule(MapNext mapNext)
        {
            SaveIdMapCapsuleReturn();
            int index = mapNext.Info[0];
            Service.gI().requestMapSelect(index);
        }
        #endregion

        #region Thao tác với game
        public static void UseCapsuleNormal()
        {
            Xmappp.IsShowPanelMapTrans = false;
            Service.gI().useItem(0, 1, -1, ID_ITEM_CAPSULE);
        }

        public static void UseCapsuleVip()
        {
            Xmappp.IsShowPanelMapTrans = false;
            Service.gI().useItem(0, 1, -1, ID_ITEM_CAPSULE_VIP);
        }

        public static void HideInfoDlg()
        {
            InfoDlg.hide();
        }

        public static void MoveMyChar(int x, int y)
        {
            Char.myCharz().cx = x;
            Char.myCharz().cy = y;
            Service.gI().charMove();

            if (ItemTime.isExistItem(ID_ICON_ITEM_TDLT))
                return;

            Char.myCharz().cx = x;
            Char.myCharz().cy = y + 1;
            Service.gI().charMove();
            Char.myCharz().cx = x;
            Char.myCharz().cy = y;
            Service.gI().charMove();
        }

        public static void RequestChangeMap(Waypoint waypoint)
        {
            if (waypoint.isOffline)
            {
                Service.gI().getMapOffline();
                return;
            }
            Service.gI().requestChangeMap();
        }
        #endregion
    }
}
