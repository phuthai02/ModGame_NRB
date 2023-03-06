using Assets.src.e;
using Mod.Boss;
using Mod.Xmap;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;

public class PhuThai
{

    public static void PhímTắt()
    {
        if (GameCanvas.keyAsciiPress == 'a')
        {
            isTuDanh = !isTuDanh;
            GameScr.info1.addInfo("Tự đánh: " + (isTuDanh ? "Bật" : "Tắt"), 0);

        }
        if (GameCanvas.keyAsciiPress == 'b')
        {
            DùngItem(454);
            GameScr.info1.addInfo("Bông tai", 0);
        }
        if (GameCanvas.keyAsciiPress == 'c')
        {
            DùngItem(194);
        }
        if (GameCanvas.keyAsciiPress == 'd')
        {
            isTheGioi = !isTheGioi;
            GameScr.info1.addInfo("Chat thế giới: " + (isTheGioi ? "Hiện" : "Ẩn"), 0);
        }
        if (GameCanvas.keyAsciiPress == 'e')
        {
            isHoiSinh = !isHoiSinh;
            GameScr.info1.addInfo("Auto hồi sinh: " + (isHoiSinh ? "Bật" : "Tắt"), 0);
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
            Boss.isEnabled = !Boss.isEnabled;
            BossDead.isEnabled = !BossDead.isEnabled;
            GameScr.info1.addInfo("Thông báo BOSS: " + (BossDead.isEnabled ? "Bật" : "Tắt"), 0);
        }

        if (GameCanvas.keyAsciiPress == 'w')
        {
            //test
            XmapController.StartRunToMapId(79);
        }

        if (GameCanvas.keyAsciiPress == 'q')
        {
            //test
            XmapController.StartRunToMapId(96);
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
            isTuDanh = isTimQuai;
            isTimQuai = !isTimQuai;
            isTuDanh = !isTuDanh;
            GameScr.info1.addInfo("Tự động luyện tập: " + (isTuDanh ? "Bật" : "Tắt"), 0);
        }

    }

    public static void getFocus()
    {

    }

    public static bool isHoiSinh = true;
    public static bool isTimQuai;
    public static bool isTheGioi = false;
    public static bool isTuDanh;





    public static void AutoĐánh()
    {
        MyVector vMob = new MyVector();
        MyVector vChar = new MyVector();

        Char myChar = Char.myCharz();
        if (myChar.mobFocus != null)
            vMob.addElement(myChar.mobFocus);
        else if (myChar.charFocus != null)
            vChar.addElement(myChar.charFocus);

        if (vMob.size() > 0 || vChar.size() > 0)
        {
            Skill myskill = myChar.myskill;
            long currentTimeMillis = mSystem.currentTimeMillis();

            if (currentTimeMillis - myskill.lastTimeUseThisSkill > myskill.coolDown)
            {
                Service.gI().sendPlayerAttack(vMob, vChar, -1);
                myskill.lastTimeUseThisSkill = currentTimeMillis;
            }
        }
    }

    public static void TìmQuái()
    {
        for (int i = 0; i < GameScr.vMob.size(); i++)
        {
            Mob quaitrongmap = (Mob)GameScr.vMob.elementAt(i);
            if (quaitrongmap != null)
            {
                if (quaitrongmap.status != 0 && quaitrongmap.status != 1 && quaitrongmap.hp > 0 && !quaitrongmap.isMobMe && !quaitrongmap.checkIsBoss())
                {
                    Char.myCharz().mobFocus = quaitrongmap;
                    XmapController.MoveMyChar(Char.myCharz().mobFocus.x, Char.myCharz().mobFocus.y);
                    break;
                }
            }

        }
    }
    public static void DùngItem(short itemId)
    {
        for (int i = 0; i < Char.myCharz().arrItemBag.Length; i++)
        {
            if (Char.myCharz().arrItemBag[i].template.id == itemId)
            {
                Service.gI().useItem(0, 1, -1, itemId);
                break;
            }
        }
    }

    public static void LoadSkill()
    {
        int gender = Char.myCharz().cgender;
        for (int i = 0; i < Char.myCharz().nClass.skillTemplates.Length; i++)
        {
            SkillTemplate skillTemplate = Char.myCharz().nClass.skillTemplates[i];
            Skill skill = Char.myCharz().getSkill(skillTemplate);
            GameScr.keySkill[i] = skill;
        }
    }
    public static void teleToNpc(Npc npc)
    {
        LoadMap.teleportMyChar(npc.cx, npc.ySd - npc.ySd % 24);
        Char.myCharz().npcFocus = npc;
    }

}


