using Mod.Boss;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class GameEvents
{
    public static void onChatVip(string chatVip)
    {
        Boss.AddBoss(chatVip);
        BossDead.AddBossDead(chatVip);
    }
    public static void onUpdateTouchGameScr()
    {
        PlayerInMap.UpdateTouch();
        Boss.UpdateTouch();
        BossDead.UpdateTouch();
        BossInMap.UpdateTouch();
    }
    public static void onUpdateGameScr()
    {
        Boss.Update();
        BossDead.Update();
    }
    public static void onPaintGameScr(mGraphics g)
    {
        Boss.Paint(g);
        BossDead.Paint(g);
        PlayerInMap.Paint(g);
        BossInMap.Paint(g);
    }
}
