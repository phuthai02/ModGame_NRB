using Assets.src.e;
using Mod.Xmap;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using static System.Collections.Specialized.BitVector32;

public class Utilities // Hỗ trợ
{

    public static int charId = -1;



    public static void findAndUseItemByItemID(short itemId)
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


    public static int LấyIndexYardrat()
    {
        for (int i = 0; i < Char.myCharz().arrItemBag.Length; i++)
        {
            if (Char.myCharz().arrItemBag[i].template.name.Contains("Yardrat"))
            {
                return i;
            }
        }
        return -1;
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
        teleportMyChar(npc.cx, npc.ySd - npc.ySd % 24);
        Char.myCharz().npcFocus = npc;
    }



    public static void teleToCharVip(int CharID)
    {
        charId = CharID;
        bool flag = false;
        MyVector vFriend = GameCanvas.panel.vFriend;
        for (int i = 0; i < vFriend.size(); i++)
        {
            InfoItem @char = (InfoItem)vFriend.elementAt(i);
            if (@char.charInfo.charID == charId)
            {
                flag = true;
            }
        }
        if (!flag)
        {
            Service.gI().friend(1, charId);
            Thread.Sleep(700);
            GameCanvas.gI().keyPressedz(-5);
        }
        Thread.Sleep(700);

        int y = LấyIndexYardrat();

        Item[] arrItemBody = Char.myCharz().arrItemBody;
        if (arrItemBody[5] == null)
        {
            Service.gI().getItem(4, (sbyte)y);
            Service.gI().gotoPlayer(charId);
            Service.gI().getItem(5, 5);
        }
        else if (arrItemBody[5].template.name.Contains("Yardrat"))
        {
            Service.gI().gotoPlayer(charId);
        }
        else if (!arrItemBody[5].template.name.Contains("Yardrat"))
        {
            Service.gI().getItem(4, (sbyte)y);
            Service.gI().gotoPlayer(charId);
            Service.gI().getItem(4, (sbyte)y);
        }


    }
}


