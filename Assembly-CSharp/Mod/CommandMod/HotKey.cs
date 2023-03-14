
using Mod.Xmap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class HotKey  // Phím tắt
{
    public static void addHotKey()
    {

        if (GameCanvas.keyAsciiPress == 'z')
        {
            PlayerInMap.isEnabled = !PlayerInMap.isEnabled;
            NpcInMap.isEnabled = !NpcInMap.isEnabled;   
            GameScr.info1.addInfo("Danh sách người chơi và NPC: " + (PlayerInMap.isEnabled ? "Bật" : "Tắt"), 0);
        }

        if (GameCanvas.keyAsciiPress == 'a')
        {
            AutoAttack.isAttack = !AutoAttack.isAttack;
            GameScr.info1.addInfo("Tự đánh: " + (AutoAttack.isAttack ? "Bật" : "Tắt"), 0);

        }
        if (GameCanvas.keyAsciiPress == 'b')
        {
            Utilities.findAndUseItemByItemID(454);
            GameScr.info1.addInfo("Bông tai", 0);
        }
        if (GameCanvas.keyAsciiPress == 'c')
        {
            Utilities.findAndUseItemByItemID(194);
        }
        if (GameCanvas.keyAsciiPress == 'd')
        {
            Message message = null;
            try
            {
                message = new Message((sbyte)(-103));
                message.writer().writeByte(0);
                Session_ME.gI().sendMessage(message);
            }
            catch (Exception ex)
            {
                Cout.println(ex.Message + ex.StackTrace);
            }
            finally
            {
                message.cleanup();
            }
        }
        if (GameCanvas.keyAsciiPress == 'e')
        {
            AutoWakeUp.isWakeUp = !AutoWakeUp.isWakeUp;
            GameScr.info1.addInfo("Auto hồi sinh: " + (AutoWakeUp.isWakeUp ? "Bật" : "Tắt"), 0);
        }
        if (GameCanvas.keyAsciiPress == 'f')
        {
            Service.gI().openUIZone();
        }
        if (GameCanvas.keyAsciiPress == 'g')
        {
            try
            {
                int idChar = Char.myCharz().charFocus.charID;
                Service.gI().giaodich(0, idChar, -1, -1);
                GameScr.info1.addInfo("Đã mời giao dịch với " + Char.myCharz().charFocus.cName, 0);
            }
            catch
            {
                GameScr.info1.addInfo("Vui lòng chọn người chơi", 0);
            }
        }
        if (GameCanvas.keyAsciiPress == 'h')
        {
            BossAppeared.isEnabled = !BossAppeared.isEnabled;
            GameScr.info1.addInfo("Thông báo boss: " + (BossAppeared.isEnabled ? "Bật" : "Tắt"), 0);
        }

        if (GameCanvas.keyAsciiPress == 'q')
        {

            AutoSpecial.isSpecial = false;
            GameScr.info1.addInfo("Sờ tốp", 0);
        }

        if (GameCanvas.keyAsciiPress == 's')
        {
            Message message = null;
            try
            {
                message = new Message((sbyte)(-80));
                message.writer().writeByte(0);
                Session_ME.gI().sendMessage(message);
            }
            catch (Exception ex)
            {
                Cout.println(ex.Message + ex.StackTrace);
            }
            finally
            {
                message.cleanup();
            }
        }

        if (GameCanvas.keyAsciiPress == 'l')
        {
            LoadMap.NextMap(2);
        }
        if (GameCanvas.keyAsciiPress == 'k')
        {
            LoadMap.NextMap(1);
        }
        if (GameCanvas.keyAsciiPress == 'j')
        {
            LoadMap.NextMap(0);
        }
        if (GameCanvas.keyAsciiPress == 'x')
        {
            Xmappp.Chat("xmp");
        }
        if (GameCanvas.keyAsciiPress == 't')
        {
            AutoAttack.isAttack = AutoAttack.isFindMob;
            AutoAttack.isFindMob = !AutoAttack.isFindMob;
            AutoAttack.isAttack = !AutoAttack.isAttack;
            GameScr.info1.addInfo("Tự động luyện tập: " + (AutoAttack.isAttack ? "Bật" : "Tắt"), 0);
        }

    }
}
