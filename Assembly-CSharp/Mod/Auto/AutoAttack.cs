using Mod.Xmap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class AutoAttack // Auto đánh
{
    public static bool isFindMob;
    public static bool isAttack;
    public static void attackMob()
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
    public static void findMob()
    {
        for (int i = 0; i < GameScr.vMob.size(); i++)
        {
            Mob mobInMap = (Mob)GameScr.vMob.elementAt(i);
            if (mobInMap != null)
            {
                if (mobInMap.status != 0 && mobInMap.status != 1 && mobInMap.hp > 0 && !mobInMap.isMobMe && !mobInMap.checkIsBoss())
                {
                    Char.myCharz().mobFocus = mobInMap;
                    XmapController.MoveMyChar(Char.myCharz().mobFocus.x, Char.myCharz().mobFocus.y);
                    break;
                }
            }

        }
    }
}

