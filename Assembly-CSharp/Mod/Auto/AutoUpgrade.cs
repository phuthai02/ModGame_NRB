using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;

class AutoUpgrade : IActionListener // Auto đập đồ
{
    public static bool isDapDo = false;
    public static int soSao = 0;
    private static AutoUpgrade instance;
    public static AutoUpgrade gI()
    {
        return instance == null ? instance = new AutoUpgrade() : instance;
    }
    public static void dapDo()
    {
        while (true)
        {
            try
            {
                if (isDapDo && GameCanvas.panel.isDoneCombine)
                {
                    Service.gI().combine(1, GameCanvas.panel.vItemCombine);
                    Thread.Sleep(200);
                    Service.gI().confirmMenu(21, 0);
                    Thread.Sleep(500);
                    if (getMaxStar((Item)GameCanvas.panel.vItemCombine.elementAt(0)) == soSao)
                    {
                        isDapDo = false;
                        break;
                    }
                }
                Thread.Sleep(200);
            }
            catch (Exception e) { }
        }
    }
    public static int getMaxStar(Item item)
    {
        int maxStar = 0;
        for (int i = 0; i < item.itemOption.Length; i++)
        {
            if (item.itemOption[i].optionTemplate.id == 107)
            {
                maxStar = item.itemOption[i].param;
            }
        }
        return maxStar;
    }
    //public static int getStar(Item item)
    //{
    //    int star = 0;
    //    for (int i = 0; i < item.itemOption.Length; i++)
    //    {
    //        if (item.itemOption[i].optionTemplate.id == 102)
    //        {
    //            star = item.itemOption[i].param;
    //        }
    //    }
    //    return star;
    //}
    public static void paintMenuSao(int start, int idAction)
    {
        MyVector vector = new MyVector();
        for (int i = start + 1; i < 9; i++)
        {
            vector.addElement(new Command(i + " sao", AutoUpgrade.gI(), i + idAction * 10, null));
        }
        GameCanvas.menu.startAt(vector, 3);
    }


    public static string topInfo;


    public static void paintMenuCustom()
    {
        string itemEpSao = "14,15,16,17,18,19,20,441,442,443,444,445,446,447,";
        string itemNhapNr = "15,16,17,18,19,20,";
        string itemNangCap = "220,221,222,223,224,";

        MyVector vector = new MyVector();
        Item item1 = (Item)GameCanvas.panel.vItemCombine.elementAt(0);
        Item item2 = (Item)GameCanvas.panel.vItemCombine.elementAt(1);

        if (TileMap.mapID == 5)
        {
            if (topInfo.Contains("mạnh mẽ") && GameCanvas.panel.vItemCombine.size() == 2  && (item1.isTypeBody() || item2.isTypeBody())  && (itemEpSao.Contains(item1.template.id + ",") || itemEpSao.Contains(item2.template.id + ",")))
            {
                vector.addElement(new Command(ResourcesCustom.RC_EP_SAO, AutoUpgrade.gI(), 2, null));
                vector.addElement(CommandCustom.CC_UPGRAGE_BINHTHUONG);
            }
            else if (topInfo.Contains("pha lê") && GameCanvas.panel.vItemCombine.size() == 1 && item1.isTypeBody())
            {
                vector.addElement(new Command(ResourcesCustom.RC_DUC_SAO, AutoUpgrade.gI(), 1, null));
                vector.addElement(CommandCustom.CC_UPGRAGE_BINHTHUONG);
            }
        }

        if (TileMap.mapID == 44 || TileMap.mapID == 42 || TileMap.mapID == 43 || TileMap.mapID == 84)
        {
            if (topInfo.Contains("Ngọc Rồng") && GameCanvas.panel.vItemCombine.size() == 1
             && itemNhapNr.Contains(item1.template.id + ",")
            )
            {
                vector.removeAllElements();
                vector.addElement(new Command(ResourcesCustom.RC_NHAP_NR, AutoUpgrade.gI(), 3, null));
                vector.addElement(CommandCustom.CC_UPGRAGE_BINHTHUONG);
            }
            else if (topInfo.Contains("mạnh mẽ") && GameCanvas.panel.vItemCombine.size() == 2
                && (item1.isTypeBody() || item2.isTypeBody())
                && (itemNangCap.Contains(item1.template.id + ",") || itemNangCap.Contains(item2.template.id + ","))
                )
            {
                vector.removeAllElements();
                vector.addElement(new Command(ResourcesCustom.RC_NANG_CAP, AutoUpgrade.gI(), 4, null));
                vector.addElement(CommandCustom.CC_UPGRAGE_BINHTHUONG);
            }
        }

        GameCanvas.menu.startAt(vector, 3);
    }

    public void perform(int idAction, Object p)
    {
        switch (idAction)
        {
            case 1:
                paintMenuSao(getMaxStar((Item)GameCanvas.panel.vItemCombine.elementAt(0)), 1);
                break;

            case 10:
                Service.gI().combine(1, GameCanvas.panel.vItemCombine);
                break;
        }
    }

}

