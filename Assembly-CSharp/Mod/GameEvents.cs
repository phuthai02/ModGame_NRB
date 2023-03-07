using Mod;
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
        if (ChatTextField.gI().strChat.Replace(" ", "") != "Chat" || ChatTextField.gI().tfChat.name != "chat") return;
        HistoryChat.gI.update();
    }
    public static void onPaintGameScr(mGraphics g)
    {
        Boss.Paint(g);
        BossDead.Paint(g);
        PlayerInMap.Paint(g);
        BossInMap.Paint(g);
    }
    public static bool onSendChat(string text)
    {
        HistoryChat.gI.append(text);
        bool result = true;
        return result;
    }
    public static void onPaintChatTextField(ChatTextField instance, mGraphics g)
    {
        if (instance == ChatTextField.gI() && instance.strChat.Replace(" ", "") == "Chat" && instance.tfChat.name == "chat")
            HistoryChat.gI.paint(g);
    }
    public static bool onStartChatTextField(ChatTextField sender)
    {
        if (ChatTextField.gI().strChat.Replace(" ", "") != "Chat" || ChatTextField.gI().tfChat.name != "chat") return false;
        if (sender == ChatTextField.gI())
        {
            HistoryChat.gI.show();
        }
        return false;
    }
}
