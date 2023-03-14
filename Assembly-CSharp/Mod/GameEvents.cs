using Mod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using UnityEngine;

public class GameEvents // Sự kiện thêm vào
{
    public static void onChatVip(string chatVip)
    {
        BossAppeared.AddBoss(chatVip);
    }
    public static void onUpdateTouchGameScr()
    {
        PlayerInMap.UpdateTouch();
        BossAppeared.UpdateTouch();
        BossInMap.UpdateTouch();
        NpcInMap.UpdateTouch();
    }
    public static void onUpdateGameScr()
    {
        BossAppeared.Update();
        if (ChatTextField.gI().strChat.Replace(" ", "") != "Chat" || ChatTextField.gI().tfChat.name != "chat") return;
    }
    public static void onPaintGameScr(mGraphics g)
    {
        BossAppeared.Paint(g);
        PlayerInMap.Paint(g);
        NpcInMap.Paint(g);
        BossInMap.Paint(g);
        TopMiddleInfo.Paint(g);
        leftTopInfoOnPaintGameScr(g);
    }


    public static void leftTopInfoOnPaintGameScr(mGraphics g)
    {
        mFont.tahoma_7b_yellow.drawString(g, NinjaUtil.getMoneys(Char.myCharz().cHP), GameCanvas.w / 2 - 63 * mGraphics.zoomLevel, 4, mFont.CENTER, mFont.tahoma_7b_dark);
        mFont.tahoma_7b_yellow.drawString(g, NinjaUtil.getMoneys(Char.myCharz().cMP), GameCanvas.w / 2 - 63 * mGraphics.zoomLevel, 18, mFont.CENTER, mFont.tahoma_7b_dark);
    }




}
