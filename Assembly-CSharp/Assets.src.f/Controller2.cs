using System;
using Assets.src.g;

namespace Assets.src.f;

internal class Controller2
{
	public static void readMessage(Message msg)
	{
		try
		{
			Res.outz("cmd=" + msg.command);
			switch (msg.command)
			{
			case sbyte.MinValue:
				readInfoEffChar(msg);
				break;
			case sbyte.MaxValue:
				readInfoRada(msg);
				break;
			case 114:
				try
				{
					string text2 = msg.reader().readUTF();
					mSystem.curINAPP = msg.reader().readByte();
					mSystem.maxINAPP = msg.reader().readByte();
					break;
				}
				catch (Exception)
				{
					break;
				}
			case 113:
			{
				int loop = msg.reader().readByte();
				int layer = msg.reader().readByte();
				int id = msg.reader().readUnsignedByte();
				short x = msg.reader().readShort();
				short y = msg.reader().readShort();
				short loopCount = msg.reader().readShort();
				EffecMn.addEff(new Effect(id, x, y, layer, loop, loopCount));
				break;
			}
			case 48:
			{
				sbyte b25 = (sbyte)(ServerListScreen.ipSelect = msg.reader().readByte());
				GameCanvas.instance.doResetToLoginScr(GameCanvas.serverScreen);
				Session_ME.gI().close();
				GameCanvas.endDlg();
				ServerListScreen.waitToLogin = true;
				break;
			}
			case 31:
			{
				int num15 = msg.reader().readInt();
				sbyte b14 = msg.reader().readByte();
				if (b14 == 1)
				{
					short smallID = msg.reader().readShort();
					sbyte b15 = -1;
					int[] array2 = null;
					short wimg = 0;
					short himg = 0;
					try
					{
						b15 = msg.reader().readByte();
						if (b15 > 0)
						{
							sbyte b16 = msg.reader().readByte();
							array2 = new int[b16];
							for (int num16 = 0; num16 < b16; num16++)
							{
								array2[num16] = msg.reader().readByte();
							}
							wimg = msg.reader().readShort();
							himg = msg.reader().readShort();
						}
					}
					catch (Exception)
					{
					}
					if (num15 == Char.myCharz().charID)
					{
						Char.myCharz().petFollow = new PetFollow();
						Char.myCharz().petFollow.smallID = smallID;
						if (b15 > 0)
						{
							Char.myCharz().petFollow.SetImg(b15, array2, wimg, himg);
						}
						break;
					}
					Char char2 = GameScr.findCharInMap(num15);
					char2.petFollow = new PetFollow();
					char2.petFollow.smallID = smallID;
					if (b15 > 0)
					{
						char2.petFollow.SetImg(b15, array2, wimg, himg);
					}
				}
				else if (num15 == Char.myCharz().charID)
				{
					Char.myCharz().petFollow.remove();
					Char.myCharz().petFollow = null;
				}
				else
				{
					Char char3 = GameScr.findCharInMap(num15);
					char3.petFollow.remove();
					char3.petFollow = null;
				}
				break;
			}
			case -89:
				GameCanvas.open3Hour = msg.reader().readByte() == 1;
				break;
			case 42:
			{
				GameCanvas.endDlg();
				LoginScr.isContinueToLogin = false;
				Char.isLoadingMap = false;
				sbyte haveName = msg.reader().readByte();
				if (GameCanvas.registerScr == null)
				{
					GameCanvas.registerScr = new RegisterScreen(haveName);
				}
				GameCanvas.registerScr.switchToMe();
				break;
			}
			case 52:
			{
				sbyte b19 = msg.reader().readByte();
				if (b19 == 1)
				{
					int num21 = msg.reader().readInt();
					if (num21 == Char.myCharz().charID)
					{
						Char.myCharz().setMabuHold(m: true);
						Char.myCharz().cx = msg.reader().readShort();
						Char.myCharz().cy = msg.reader().readShort();
					}
					else
					{
						Char char4 = GameScr.findCharInMap(num21);
						if (char4 != null)
						{
							char4.setMabuHold(m: true);
							char4.cx = msg.reader().readShort();
							char4.cy = msg.reader().readShort();
						}
					}
				}
				if (b19 == 0)
				{
					int num22 = msg.reader().readInt();
					if (num22 == Char.myCharz().charID)
					{
						Char.myCharz().setMabuHold(m: false);
					}
					else
					{
						GameScr.findCharInMap(num22)?.setMabuHold(m: false);
					}
				}
				if (b19 == 2)
				{
					int charId3 = msg.reader().readInt();
					int id4 = msg.reader().readInt();
					Mabu mabu2 = (Mabu)GameScr.findCharInMap(charId3);
					mabu2.eat(id4);
				}
				if (b19 == 3)
				{
					GameScr.mabuPercent = msg.reader().readByte();
				}
				break;
			}
			case 51:
			{
				int charId2 = msg.reader().readInt();
				Mabu mabu = (Mabu)GameScr.findCharInMap(charId2);
				sbyte id3 = msg.reader().readByte();
				short x2 = msg.reader().readShort();
				short y2 = msg.reader().readShort();
				sbyte b18 = msg.reader().readByte();
				Char[] array4 = new Char[b18];
				int[] array5 = new int[b18];
				for (int num19 = 0; num19 < b18; num19++)
				{
					int num20 = msg.reader().readInt();
					Res.outz("char ID=" + num20);
					array4[num19] = null;
					if (num20 != Char.myCharz().charID)
					{
						array4[num19] = GameScr.findCharInMap(num20);
					}
					else
					{
						array4[num19] = Char.myCharz();
					}
					array5[num19] = msg.reader().readInt();
				}
				mabu.setSkill(id3, x2, y2, array4, array5);
				break;
			}
			case -127:
				readLuckyRound(msg);
				break;
			case -126:
			{
				sbyte b12 = msg.reader().readByte();
				Res.outz("type quay= " + b12);
				if (b12 == 1)
				{
					sbyte b13 = msg.reader().readByte();
					string num14 = msg.reader().readUTF();
					string finish = msg.reader().readUTF();
					GameScr.gI().showWinNumber(num14, finish);
				}
				if (b12 == 0)
				{
					GameScr.gI().showYourNumber(msg.reader().readUTF());
				}
				break;
			}
			case -122:
			{
				short id5 = msg.reader().readShort();
				Npc npc = GameScr.findNPCInMap(id5);
				sbyte b28 = msg.reader().readByte();
				npc.duahau = new int[b28];
				Res.outz("N DUA HAU= " + b28);
				for (int num37 = 0; num37 < b28; num37++)
				{
					npc.duahau[num37] = msg.reader().readShort();
				}
				npc.setStatus(msg.reader().readByte(), msg.reader().readInt());
				break;
			}
			case 102:
			{
				sbyte b29 = msg.reader().readByte();
				if (b29 == 0 || b29 == 1 || b29 == 2 || b29 == 6)
				{
					BigBoss2 bigBoss2 = Mob.getBigBoss2();
					if (bigBoss2 == null)
					{
						break;
					}
					if (b29 == 6)
					{
						bigBoss2.x = (bigBoss2.y = (bigBoss2.xTo = (bigBoss2.yTo = (bigBoss2.xFirst = (bigBoss2.yFirst = -1000)))));
						break;
					}
					sbyte b30 = msg.reader().readByte();
					Char[] array8 = new Char[b30];
					int[] array9 = new int[b30];
					for (int num39 = 0; num39 < b30; num39++)
					{
						int num40 = msg.reader().readInt();
						array8[num39] = null;
						if (num40 != Char.myCharz().charID)
						{
							array8[num39] = GameScr.findCharInMap(num40);
						}
						else
						{
							array8[num39] = Char.myCharz();
						}
						array9[num39] = msg.reader().readInt();
					}
					bigBoss2.setAttack(array8, array9, b29);
				}
				if (b29 == 3 || b29 == 4 || b29 == 5 || b29 == 7)
				{
					BachTuoc bachTuoc = Mob.getBachTuoc();
					if (bachTuoc == null)
					{
						break;
					}
					if (b29 == 7)
					{
						bachTuoc.x = (bachTuoc.y = (bachTuoc.xTo = (bachTuoc.yTo = (bachTuoc.xFirst = (bachTuoc.yFirst = -1000)))));
						break;
					}
					if (b29 == 3 || b29 == 4)
					{
						sbyte b31 = msg.reader().readByte();
						Char[] array10 = new Char[b31];
						int[] array11 = new int[b31];
						for (int num41 = 0; num41 < b31; num41++)
						{
							int num42 = msg.reader().readInt();
							array10[num41] = null;
							if (num42 != Char.myCharz().charID)
							{
								array10[num41] = GameScr.findCharInMap(num42);
							}
							else
							{
								array10[num41] = Char.myCharz();
							}
							array11[num41] = msg.reader().readInt();
						}
						bachTuoc.setAttack(array10, array11, b29);
					}
					if (b29 == 5)
					{
						short xMoveTo = msg.reader().readShort();
						bachTuoc.move(xMoveTo);
					}
				}
				if (b29 > 9 && b29 < 30)
				{
					readActionBoss(msg, b29);
				}
				break;
			}
			case 101:
			{
				Res.outz("big boss--------------------------------------------------");
				BigBoss bigBoss = Mob.getBigBoss();
				if (bigBoss == null)
				{
					break;
				}
				sbyte b26 = msg.reader().readByte();
				if (b26 == 0 || b26 == 1 || b26 == 2 || b26 == 4 || b26 == 3)
				{
					if (b26 == 3)
					{
						bigBoss.xTo = (bigBoss.xFirst = msg.reader().readShort());
						bigBoss.yTo = (bigBoss.yFirst = msg.reader().readShort());
						bigBoss.setFly();
					}
					else
					{
						sbyte b27 = msg.reader().readByte();
						Res.outz("CHUONG nChar= " + b27);
						Char[] array6 = new Char[b27];
						int[] array7 = new int[b27];
						for (int num34 = 0; num34 < b27; num34++)
						{
							int num35 = msg.reader().readInt();
							Res.outz("char ID=" + num35);
							array6[num34] = null;
							if (num35 != Char.myCharz().charID)
							{
								array6[num34] = GameScr.findCharInMap(num35);
							}
							else
							{
								array6[num34] = Char.myCharz();
							}
							array7[num34] = msg.reader().readInt();
						}
						bigBoss.setAttack(array6, array7, b26);
					}
				}
				if (b26 == 5)
				{
					bigBoss.haftBody = true;
					bigBoss.status = 2;
				}
				if (b26 == 6)
				{
					bigBoss.getDataB2();
					bigBoss.x = msg.reader().readShort();
					bigBoss.y = msg.reader().readShort();
				}
				if (b26 == 7)
				{
					bigBoss.setAttack(null, null, b26);
				}
				if (b26 == 8)
				{
					bigBoss.xTo = (bigBoss.xFirst = msg.reader().readShort());
					bigBoss.yTo = (bigBoss.yFirst = msg.reader().readShort());
					bigBoss.status = 2;
				}
				if (b26 == 9)
				{
					bigBoss.x = (bigBoss.y = (bigBoss.xTo = (bigBoss.yTo = (bigBoss.xFirst = (bigBoss.yFirst = -1000)))));
				}
				break;
			}
			case -120:
			{
				long num38 = mSystem.currentTimeMillis();
				Service.logController = num38 - Service.curCheckController;
				Service.gI().sendCheckController();
				break;
			}
			case -121:
			{
				long num31 = mSystem.currentTimeMillis();
				Service.logMap = num31 - Service.curCheckMap;
				Service.gI().sendCheckMap();
				break;
			}
			case 100:
			{
				sbyte b5 = msg.reader().readByte();
				sbyte b6 = msg.reader().readByte();
				Item item = null;
				if (b5 == 0)
				{
					item = Char.myCharz().arrItemBody[b6];
				}
				if (b5 == 1)
				{
					item = Char.myCharz().arrItemBag[b6];
				}
				short num4 = msg.reader().readShort();
				if (num4 == -1)
				{
					break;
				}
				item.template = ItemTemplates.get(num4);
				item.quantity = msg.reader().readInt();
				item.info = msg.reader().readUTF();
				item.content = msg.reader().readUTF();
				sbyte b7 = msg.reader().readByte();
				if (b7 == 0)
				{
					break;
				}
				item.itemOption = new ItemOption[b7];
				for (int l = 0; l < item.itemOption.Length; l++)
				{
					int num5 = msg.reader().readUnsignedByte();
					Res.outz("id o= " + num5);
					int param2 = msg.reader().readUnsignedShort();
					if (num5 != -1)
					{
						item.itemOption[l] = new ItemOption(num5, param2);
					}
				}
				break;
			}
			case -123:
			{
				int charId = msg.reader().readInt();
				if (GameScr.findCharInMap(charId) != null)
				{
					GameScr.findCharInMap(charId).perCentMp = msg.reader().readByte();
				}
				break;
			}
			case -119:
				Char.myCharz().rank = msg.reader().readInt();
				break;
			case -117:
				GameScr.gI().tMabuEff = 0;
				GameScr.gI().percentMabu = msg.reader().readByte();
				if (GameScr.gI().percentMabu == 100)
				{
					GameScr.gI().mabuEff = true;
				}
				if (GameScr.gI().percentMabu == 101)
				{
					Npc.mabuEff = true;
				}
				break;
			case -116:
				GameScr.canAutoPlay = msg.reader().readByte() == 1;
				break;
			case -115:
				Char.myCharz().setPowerInfo(msg.reader().readUTF(), msg.reader().readShort(), msg.reader().readShort(), msg.reader().readShort());
				break;
			case -113:
			{
				sbyte[] array = new sbyte[5];
				for (int k = 0; k < 5; k++)
				{
					array[k] = msg.reader().readByte();
					Res.outz("vlue i= " + array[k]);
				}
				GameScr.gI().onKSkill(array);
				GameScr.gI().onOSkill(array);
				GameScr.gI().onCSkill(array);
				break;
			}
			case -111:
			{
				short num32 = msg.reader().readShort();
				ImageSource.vSource = new MyVector();
				for (int num33 = 0; num33 < num32; num33++)
				{
					string iD = msg.reader().readUTF();
					sbyte version = msg.reader().readByte();
					ImageSource.vSource.addElement(new ImageSource(iD, version));
				}
				ImageSource.checkRMS();
				ImageSource.saveRMS();
				break;
			}
			case 125:
			{
				sbyte fusion = msg.reader().readByte();
				int num36 = msg.reader().readInt();
				if (num36 == Char.myCharz().charID)
				{
					Char.myCharz().setFusion(fusion);
				}
				else if (GameScr.findCharInMap(num36) != null)
				{
					GameScr.findCharInMap(num36).setFusion(fusion);
				}
				break;
			}
			case 124:
			{
				short id6 = msg.reader().readShort();
				string text4 = msg.reader().readUTF();
				Res.outz("noi chuyen = " + text4 + "npc ID= " + id6);
				GameScr.findNPCInMap(id6)?.addInfo(text4);
				break;
			}
			case 123:
			{
				Res.outz("SET POSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSss");
				int num24 = msg.reader().readInt();
				short xPos = msg.reader().readShort();
				short yPos = msg.reader().readShort();
				sbyte b22 = msg.reader().readByte();
				Char char5 = null;
				if (num24 == Char.myCharz().charID)
				{
					char5 = Char.myCharz();
				}
				else if (GameScr.findCharInMap(num24) != null)
				{
					char5 = GameScr.findCharInMap(num24);
				}
				if (char5 != null)
				{
					ServerEffect.addServerEffect((b22 != 0) ? 173 : 60, char5, 1);
					char5.setPos(xPos, yPos, b22);
				}
				break;
			}
			case 122:
			{
				short timeLogin = msg.reader().readShort();
				Res.outz("second login = " + timeLogin);
				LoginScr.timeLogin = timeLogin;
				LoginScr.currTimeLogin = (LoginScr.lastTimeLogin = mSystem.currentTimeMillis());
				GameCanvas.endDlg();
				break;
			}
			case 121:
				mSystem.publicID = msg.reader().readUTF();
				mSystem.strAdmob = msg.reader().readUTF();
				Res.outz("SHOW AD public ID= " + mSystem.publicID);
				mSystem.createAdmob();
				break;
			case -124:
			{
				sbyte b23 = msg.reader().readByte();
				sbyte b24 = msg.reader().readByte();
				if (b24 == 0)
				{
					if (b23 == 2)
					{
						int num25 = msg.reader().readInt();
						if (num25 == Char.myCharz().charID)
						{
							Char.myCharz().removeEffect();
						}
						else if (GameScr.findCharInMap(num25) != null)
						{
							GameScr.findCharInMap(num25).removeEffect();
						}
					}
					int num26 = msg.reader().readUnsignedByte();
					int num27 = msg.reader().readInt();
					if (num26 == 32)
					{
						if (b23 == 1)
						{
							int num28 = msg.reader().readInt();
							if (num27 == Char.myCharz().charID)
							{
								Char.myCharz().holdEffID = num26;
								GameScr.findCharInMap(num28).setHoldChar(Char.myCharz());
							}
							else if (GameScr.findCharInMap(num27) != null && num28 != Char.myCharz().charID)
							{
								GameScr.findCharInMap(num27).holdEffID = num26;
								GameScr.findCharInMap(num28).setHoldChar(GameScr.findCharInMap(num27));
							}
							else if (GameScr.findCharInMap(num27) != null && num28 == Char.myCharz().charID)
							{
								GameScr.findCharInMap(num27).holdEffID = num26;
								Char.myCharz().setHoldChar(GameScr.findCharInMap(num27));
							}
						}
						else if (num27 == Char.myCharz().charID)
						{
							Char.myCharz().removeHoleEff();
						}
						else if (GameScr.findCharInMap(num27) != null)
						{
							GameScr.findCharInMap(num27).removeHoleEff();
						}
					}
					if (num26 == 33)
					{
						if (b23 == 1)
						{
							if (num27 == Char.myCharz().charID)
							{
								Char.myCharz().protectEff = true;
							}
							else if (GameScr.findCharInMap(num27) != null)
							{
								GameScr.findCharInMap(num27).protectEff = true;
							}
						}
						else if (num27 == Char.myCharz().charID)
						{
							Char.myCharz().removeProtectEff();
						}
						else if (GameScr.findCharInMap(num27) != null)
						{
							GameScr.findCharInMap(num27).removeProtectEff();
						}
					}
					if (num26 == 39)
					{
						if (b23 == 1)
						{
							if (num27 == Char.myCharz().charID)
							{
								Char.myCharz().huytSao = true;
							}
							else if (GameScr.findCharInMap(num27) != null)
							{
								GameScr.findCharInMap(num27).huytSao = true;
							}
						}
						else if (num27 == Char.myCharz().charID)
						{
							Char.myCharz().removeHuytSao();
						}
						else if (GameScr.findCharInMap(num27) != null)
						{
							GameScr.findCharInMap(num27).removeHuytSao();
						}
					}
					if (num26 == 40)
					{
						if (b23 == 1)
						{
							if (num27 == Char.myCharz().charID)
							{
								Char.myCharz().blindEff = true;
							}
							else if (GameScr.findCharInMap(num27) != null)
							{
								GameScr.findCharInMap(num27).blindEff = true;
							}
						}
						else if (num27 == Char.myCharz().charID)
						{
							Char.myCharz().removeBlindEff();
						}
						else if (GameScr.findCharInMap(num27) != null)
						{
							GameScr.findCharInMap(num27).removeBlindEff();
						}
					}
					if (num26 == 41)
					{
						if (b23 == 1)
						{
							if (num27 == Char.myCharz().charID)
							{
								Char.myCharz().sleepEff = true;
							}
							else if (GameScr.findCharInMap(num27) != null)
							{
								GameScr.findCharInMap(num27).sleepEff = true;
							}
						}
						else if (num27 == Char.myCharz().charID)
						{
							Char.myCharz().removeSleepEff();
						}
						else if (GameScr.findCharInMap(num27) != null)
						{
							GameScr.findCharInMap(num27).removeSleepEff();
						}
					}
					if (num26 == 42)
					{
						if (b23 == 1)
						{
							if (num27 == Char.myCharz().charID)
							{
								Char.myCharz().stone = true;
							}
						}
						else if (num27 == Char.myCharz().charID)
						{
							Char.myCharz().stone = false;
						}
					}
				}
				if (b24 != 1)
				{
					break;
				}
				int num29 = msg.reader().readUnsignedByte();
				sbyte mobIndex = msg.reader().readByte();
				Res.outz("modbHoldID= " + mobIndex + " skillID= " + num29 + "eff ID= " + b23);
				if (num29 == 32)
				{
					if (b23 == 1)
					{
						int num30 = msg.reader().readInt();
						if (num30 == Char.myCharz().charID)
						{
							GameScr.findMobInMap(mobIndex).holdEffID = num29;
							Char.myCharz().setHoldMob(GameScr.findMobInMap(mobIndex));
						}
						else if (GameScr.findCharInMap(num30) != null)
						{
							GameScr.findMobInMap(mobIndex).holdEffID = num29;
							GameScr.findCharInMap(num30).setHoldMob(GameScr.findMobInMap(mobIndex));
						}
					}
					else
					{
						GameScr.findMobInMap(mobIndex).removeHoldEff();
					}
				}
				if (num29 == 40)
				{
					if (b23 == 1)
					{
						GameScr.findMobInMap(mobIndex).blindEff = true;
					}
					else
					{
						GameScr.findMobInMap(mobIndex).removeBlindEff();
					}
				}
				if (num29 == 41)
				{
					if (b23 == 1)
					{
						GameScr.findMobInMap(mobIndex).sleepEff = true;
					}
					else
					{
						GameScr.findMobInMap(mobIndex).removeSleepEff();
					}
				}
				break;
			}
			case -125:
			{
				ChatTextField.gI().isShow = false;
				string text3 = msg.reader().readUTF();
				Res.outz("titile= " + text3);
				sbyte b20 = msg.reader().readByte();
				ClientInput.gI().setInput(b20, text3);
				for (int num23 = 0; num23 < b20; num23++)
				{
					ClientInput.gI().tf[num23].name = msg.reader().readUTF();
					sbyte b21 = msg.reader().readByte();
					if (b21 == 0)
					{
						ClientInput.gI().tf[num23].setIputType(TField.INPUT_TYPE_NUMERIC);
					}
					if (b21 == 1)
					{
						ClientInput.gI().tf[num23].setIputType(TField.INPUT_TYPE_ANY);
					}
					if (b21 == 2)
					{
						ClientInput.gI().tf[num23].setIputType(TField.INPUT_TYPE_PASSWORD);
					}
				}
				break;
			}
			case -110:
			{
				sbyte b17 = msg.reader().readByte();
				if (b17 == 1)
				{
					int id2 = msg.reader().readInt();
					sbyte[] array3 = Rms.loadRMS(id2 + string.Empty);
					if (array3 == null)
					{
						Service.gI().sendServerData(1, -1, null);
					}
					else
					{
						Service.gI().sendServerData(1, id2, array3);
					}
				}
				if (b17 == 0)
				{
					int num17 = msg.reader().readInt();
					short num18 = msg.reader().readShort();
					sbyte[] data = new sbyte[num18];
					msg.reader().read(ref data, 0, num18);
					Rms.saveRMS(num17 + string.Empty, data);
				}
				break;
			}
			case 93:
			{
				string str = msg.reader().readUTF();
				str = Res.changeString(str);
				GameScr.gI().chatVip(str);
				break;
			}
			case -106:
			{
				short num12 = msg.reader().readShort();
				int num13 = msg.reader().readShort();
				if (ItemTime.isExistItem(num12))
				{
					ItemTime.getItemById(num12).initTime(num13);
					break;
				}
				ItemTime o = new ItemTime(num12, num13);
				Char.vItemTime.addElement(o);
				break;
			}
			case -105:
				TransportScr.gI().time = 0;
				TransportScr.gI().maxTime = msg.reader().readShort();
				TransportScr.gI().last = (TransportScr.gI().curr = mSystem.currentTimeMillis());
				TransportScr.gI().type = msg.reader().readByte();
				TransportScr.gI().switchToMe();
				break;
			case -103:
				switch (msg.reader().readByte())
				{
				case 0:
				{
					GameCanvas.panel.vFlag.removeAllElements();
					sbyte b9 = msg.reader().readByte();
					for (int num7 = 0; num7 < b9; num7++)
					{
						Item item2 = new Item();
						short num8 = msg.reader().readShort();
						if (num8 != -1)
						{
							item2.template = ItemTemplates.get(num8);
							sbyte b10 = msg.reader().readByte();
							if (b10 != -1)
							{
								item2.itemOption = new ItemOption[b10];
								for (int num9 = 0; num9 < item2.itemOption.Length; num9++)
								{
									int num10 = msg.reader().readUnsignedByte();
									int param3 = msg.reader().readUnsignedShort();
									if (num10 != -1)
									{
										item2.itemOption[num9] = new ItemOption(num10, param3);
									}
								}
							}
						}
						GameCanvas.panel.vFlag.addElement(item2);
					}
					GameCanvas.panel.setTypeFlag();
					GameCanvas.panel.show();
					break;
				}
				case 1:
				{
					int num11 = msg.reader().readInt();
					sbyte b11 = msg.reader().readByte();
					Res.outz("---------------actionFlag1:  " + num11 + " : " + b11);
					if (num11 == Char.myCharz().charID)
					{
						Char.myCharz().cFlag = b11;
					}
					else if (GameScr.findCharInMap(num11) != null)
					{
						GameScr.findCharInMap(num11).cFlag = b11;
					}
					GameScr.gI().getFlagImage(num11, b11);
					break;
				}
				case 2:
				{
					sbyte b8 = msg.reader().readByte();
					int num6 = msg.reader().readShort();
					PKFlag pKFlag = new PKFlag();
					pKFlag.cflag = b8;
					pKFlag.IDimageFlag = num6;
					GameScr.vFlag.addElement(pKFlag);
					for (int m = 0; m < GameScr.vFlag.size(); m++)
					{
						PKFlag pKFlag2 = (PKFlag)GameScr.vFlag.elementAt(m);
						Res.outz("i: " + m + "  cflag: " + pKFlag2.cflag + "   IDimageFlag: " + pKFlag2.IDimageFlag);
					}
					for (int n = 0; n < GameScr.vCharInMap.size(); n++)
					{
						Char @char = (Char)GameScr.vCharInMap.elementAt(n);
						if (@char != null && @char.cFlag == b8)
						{
							@char.flagImage = num6;
						}
					}
					if (Char.myCharz().cFlag == b8)
					{
						Char.myCharz().flagImage = num6;
					}
					break;
				}
				}
				break;
			case -102:
			{
				sbyte b4 = msg.reader().readByte();
				if (b4 != 0 && b4 == 1)
				{
					GameCanvas.loginScr.isLogin2 = false;
					Service.gI().login(Rms.loadRMSString("acc"), Rms.loadRMSString("pass"), GameMidlet.VERSION, 0);
					LoginScr.isLoggingIn = true;
				}
				break;
			}
			case -101:
			{
				GameCanvas.loginScr.isLogin2 = true;
				GameCanvas.connect();
				string text = msg.reader().readUTF();
				Rms.saveRMSString("userAo" + ServerListScreen.ipSelect, text);
				Service.gI().setClientType();
				Service.gI().login(text, string.Empty, GameMidlet.VERSION, 1);
				break;
			}
			case -100:
			{
				InfoDlg.hide();
				bool flag = false;
				if (GameCanvas.w > 2 * Panel.WIDTH_PANEL)
				{
					flag = true;
				}
				sbyte b = msg.reader().readByte();
				Res.outz("t Indxe= " + b);
				GameCanvas.panel.maxPageShop[b] = msg.reader().readByte();
				GameCanvas.panel.currPageShop[b] = msg.reader().readByte();
				Res.outz("max page= " + GameCanvas.panel.maxPageShop[b] + " curr page= " + GameCanvas.panel.currPageShop[b]);
				int num = msg.reader().readUnsignedByte();
				Char.myCharz().arrItemShop[b] = new Item[num];
				for (int i = 0; i < num; i++)
				{
					short num2 = msg.reader().readShort();
					if (num2 == -1)
					{
						continue;
					}
					Res.outz("template id= " + num2);
					Char.myCharz().arrItemShop[b][i] = new Item();
					Char.myCharz().arrItemShop[b][i].template = ItemTemplates.get(num2);
					Char.myCharz().arrItemShop[b][i].itemId = msg.reader().readShort();
					Char.myCharz().arrItemShop[b][i].buyCoin = msg.reader().readInt();
					Char.myCharz().arrItemShop[b][i].buyGold = msg.reader().readInt();
					Char.myCharz().arrItemShop[b][i].buyType = msg.reader().readByte();
					Char.myCharz().arrItemShop[b][i].quantity = msg.reader().readByte();
					Char.myCharz().arrItemShop[b][i].isMe = msg.reader().readByte();
					Panel.strWantToBuy = mResources.say_wat_do_u_want_to_buy;
					sbyte b2 = msg.reader().readByte();
					if (b2 != -1)
					{
						Char.myCharz().arrItemShop[b][i].itemOption = new ItemOption[b2];
						for (int j = 0; j < Char.myCharz().arrItemShop[b][i].itemOption.Length; j++)
						{
							int num3 = msg.reader().readUnsignedByte();
							int param = msg.reader().readUnsignedShort();
							if (num3 != -1)
							{
								Char.myCharz().arrItemShop[b][i].itemOption[j] = new ItemOption(num3, param);
								Char.myCharz().arrItemShop[b][i].compare = GameCanvas.panel.getCompare(Char.myCharz().arrItemShop[b][i]);
							}
						}
					}
					sbyte b3 = msg.reader().readByte();
					if (b3 == 1)
					{
						int headTemp = msg.reader().readShort();
						int bodyTemp = msg.reader().readShort();
						int legTemp = msg.reader().readShort();
						int bagTemp = msg.reader().readShort();
						Char.myCharz().arrItemShop[b][i].setPartTemp(headTemp, bodyTemp, legTemp, bagTemp);
					}
				}
				if (flag)
				{
					GameCanvas.panel2.setTabKiGui();
				}
				GameCanvas.panel.setTabShop();
				GameCanvas.panel.cmy = (GameCanvas.panel.cmtoY = 0);
				break;
			}
			}
		}
		catch (Exception)
		{
		}
	}

	private static void readLuckyRound(Message msg)
	{
		try
		{
			switch (msg.reader().readByte())
			{
			case 0:
			{
				sbyte b2 = msg.reader().readByte();
				short[] array2 = new short[b2];
				for (int j = 0; j < b2; j++)
				{
					array2[j] = msg.reader().readShort();
				}
				sbyte b3 = msg.reader().readByte();
				int price = msg.reader().readInt();
				short idTicket = msg.reader().readShort();
				CrackBallScr.gI().SetCrackBallScr(array2, (byte)b3, price, idTicket);
				break;
			}
			case 1:
			{
				sbyte b = msg.reader().readByte();
				short[] array = new short[b];
				for (int i = 0; i < b; i++)
				{
					array[i] = msg.reader().readShort();
				}
				CrackBallScr.gI().DoneCrackBallScr(array);
				break;
			}
			}
		}
		catch (Exception)
		{
		}
	}

	private static void readInfoRada(Message msg)
	{
		try
		{
			switch (msg.reader().readByte())
			{
			case 0:
			{
				RadarScr.gI();
				MyVector myVector = new MyVector(string.Empty);
				short num = msg.reader().readShort();
				int num2 = 0;
				for (int i = 0; i < num; i++)
				{
					Info_RadaScr info_RadaScr = new Info_RadaScr();
					int id2 = msg.reader().readShort();
					int no = i + 1;
					int idIcon = msg.reader().readShort();
					sbyte rank = msg.reader().readByte();
					sbyte amount2 = msg.reader().readByte();
					sbyte max_amount2 = msg.reader().readByte();
					short templateId = -1;
					Char charInfo = null;
					sbyte b = msg.reader().readByte();
					if (b == 0)
					{
						templateId = msg.reader().readShort();
					}
					else
					{
						int head = msg.reader().readShort();
						int body = msg.reader().readShort();
						int leg = msg.reader().readShort();
						int bag = msg.reader().readShort();
						charInfo = Info_RadaScr.SetCharInfo(head, body, leg, bag);
					}
					string name = msg.reader().readUTF();
					string info = msg.reader().readUTF();
					sbyte b2 = msg.reader().readByte();
					sbyte use = msg.reader().readByte();
					sbyte b3 = msg.reader().readByte();
					ItemOption[] array = null;
					if (b3 != 0)
					{
						array = new ItemOption[b3];
						for (int j = 0; j < array.Length; j++)
						{
							int num3 = msg.reader().readUnsignedByte();
							int param = msg.reader().readUnsignedShort();
							sbyte activeCard = msg.reader().readByte();
							if (num3 != -1)
							{
								array[j] = new ItemOption(num3, param);
								array[j].activeCard = activeCard;
							}
						}
					}
					info_RadaScr.SetInfo(id2, no, idIcon, rank, b, templateId, name, info, charInfo, array);
					info_RadaScr.SetLevel(b2);
					info_RadaScr.SetUse(use);
					info_RadaScr.SetAmount(amount2, max_amount2);
					myVector.addElement(info_RadaScr);
					if (b2 > 0)
					{
						num2++;
					}
				}
				RadarScr.gI().SetRadarScr(myVector, num2, num);
				RadarScr.gI().switchToMe();
				break;
			}
			case 1:
			{
				int id3 = msg.reader().readShort();
				sbyte use2 = msg.reader().readByte();
				if (Info_RadaScr.GetInfo(RadarScr.list, id3) != null)
				{
					Info_RadaScr.GetInfo(RadarScr.list, id3).SetUse(use2);
				}
				RadarScr.SetListUse();
				break;
			}
			case 2:
			{
				int num4 = msg.reader().readShort();
				sbyte level = msg.reader().readByte();
				int num5 = 0;
				for (int k = 0; k < RadarScr.list.size(); k++)
				{
					Info_RadaScr info_RadaScr2 = (Info_RadaScr)RadarScr.list.elementAt(k);
					if (info_RadaScr2 != null)
					{
						if (info_RadaScr2.id == num4)
						{
							info_RadaScr2.SetLevel(level);
						}
						if (info_RadaScr2.level > 0)
						{
							num5++;
						}
					}
				}
				RadarScr.SetNum(num5, RadarScr.list.size());
				if (Info_RadaScr.GetInfo(RadarScr.listUse, num4) != null)
				{
					Info_RadaScr.GetInfo(RadarScr.listUse, num4).SetLevel(level);
				}
				break;
			}
			case 3:
			{
				int id = msg.reader().readShort();
				sbyte amount = msg.reader().readByte();
				sbyte max_amount = msg.reader().readByte();
				if (Info_RadaScr.GetInfo(RadarScr.list, id) != null)
				{
					Info_RadaScr.GetInfo(RadarScr.list, id).SetAmount(amount, max_amount);
				}
				if (Info_RadaScr.GetInfo(RadarScr.listUse, id) != null)
				{
					Info_RadaScr.GetInfo(RadarScr.listUse, id).SetAmount(amount, max_amount);
				}
				break;
			}
			}
		}
		catch (Exception)
		{
		}
	}

	private static void readInfoEffChar(Message msg)
	{
		try
		{
			sbyte b = msg.reader().readByte();
			int num = msg.reader().readInt();
			Char @char = null;
			@char = ((num != Char.myCharz().charID) ? GameScr.findCharInMap(num) : Char.myCharz());
			switch (b)
			{
			case 0:
			{
				int id = msg.reader().readShort();
				int layer = msg.reader().readByte();
				int loop = msg.reader().readByte();
				short loopCount = msg.reader().readShort();
				sbyte isStand = msg.reader().readByte();
				@char?.addEffChar(new Effect(id, @char, layer, loop, loopCount, isStand));
				break;
			}
			case 1:
			{
				int id2 = msg.reader().readShort();
				@char?.removeEffChar(0, id2);
				break;
			}
			case 2:
				@char?.removeEffChar(-1, 0);
				break;
			}
		}
		catch (Exception)
		{
		}
	}

	private static void readActionBoss(Message msg, int actionBoss)
	{
		try
		{
			sbyte idBoss = msg.reader().readByte();
			NewBoss newBoss = Mob.getNewBoss(idBoss);
			if (newBoss == null)
			{
				return;
			}
			if (actionBoss == 10)
			{
				short xMoveTo = msg.reader().readShort();
				short yMoveTo = msg.reader().readShort();
				newBoss.move(xMoveTo, yMoveTo);
			}
			if (actionBoss >= 11 && actionBoss <= 20)
			{
				sbyte b = msg.reader().readByte();
				Char[] array = new Char[b];
				int[] array2 = new int[b];
				for (int i = 0; i < b; i++)
				{
					int num = msg.reader().readInt();
					array[i] = null;
					if (num != Char.myCharz().charID)
					{
						array[i] = GameScr.findCharInMap(num);
					}
					else
					{
						array[i] = Char.myCharz();
					}
					array2[i] = msg.reader().readInt();
				}
				sbyte dir = msg.reader().readByte();
				newBoss.setAttack(array, array2, (sbyte)(actionBoss - 10), dir);
			}
			if (actionBoss == 21)
			{
				newBoss.xTo = msg.reader().readShort();
				newBoss.yTo = msg.reader().readShort();
				newBoss.setFly();
			}
			if (actionBoss == 22)
			{
			}
			if (actionBoss == 23)
			{
				newBoss.setDie();
			}
		}
		catch (Exception)
		{
		}
	}
}
