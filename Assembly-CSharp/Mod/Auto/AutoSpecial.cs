using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Xml.Linq;


class AutoSpecial : IActionListener // Auto mở nội tại
{
    public static string nameSpecial;
    public static string maxSpecial;
    public static bool isSpecial = false;
    private static AutoSpecial instance;
    public static AutoSpecial gI()
    {
        return instance == null ? instance = new AutoSpecial() : instance;
    }

    public static void open()
    {
        if (Char.myCharz().cPower < 10000000000)
        {
            GameScr.info1.addInfo("Chưa đủ sức mạnh 10 tỷ !", 0);
            return;
        }
        do
        {
            Service.gI().speacialSkill(0);
            if (Panel.specialInfo.Contains(nameSpecial) && Panel.specialInfo.Contains(maxSpecial + "%"))
            {
                isSpecial = false;
                break;
            }
            Thread.Sleep(200);
            Service.gI().confirmMenu(5, 2);
            Thread.Sleep(200);
            Service.gI().confirmMenu(5, 0);
            Thread.Sleep(200);
        } while (isSpecial);

    }
    public void perform(int idAction, Object p)
    {
        string name = (string)p;
        nameSpecial = name.Substring(0, name.IndexOf("%") - subStrIndex(name)).Trim();
        maxSpecial = name.Substring(name.LastIndexOf("%") - 3, 3).Trim();
        isSpecial = true;
        GameCanvas.panel.hide();
        new Thread(new ThreadStart(open)).Start();
    }
    public int subStrIndex(string text)
    {
        string check = text.Substring(text.IndexOf("%") - 4, 1);
        if (check != " " || text.Contains("Chí mạng liên tục"))
        {
            return 3;
        }
        return 4;
    }
}


