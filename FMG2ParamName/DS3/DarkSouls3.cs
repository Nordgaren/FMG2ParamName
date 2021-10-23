using SoulsFormats;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

namespace FMG2ParamName
{
    class DarkSouls3
    {
        private List<FMG> ItemFMGS { get; set; }
        private List<FMG> MenuFMGS { get; set; }

        public void PatchFiles(string exeDir)
        {
#if DEBUG
            exeDir = @"F:\Steam\steamapps\common\DARK SOULS III\Game\Mod";
#endif

            //var paramdefFolder = @"F:\Steam\steamapps\common\DARK SOULS III\Game\paramdefs";
            //var compression = DCX.Type.DCX_DFLT_10000_44_9;
            //Utility.MakeParamDefBnd(paramdefFolder, compression);
            var patchItemFMGs = true;
            var patchMenuFMGs = true;;


            var gameParamFile = $@"F:\Steam\steamapps\common\DARK SOULS III\Game\Mod\param\gameparam\gameparam_dlc2.parambnd.dcx";
            var paramDefFile = Utility.GetEmbededResource("FMG2ParamName.DS3.paramdef.paramdefbnd.dcx");
            var itemFMGFile = $@"{exeDir}\msg\engus\item_dlc2.msgbnd.dcx";
            if (!File.Exists(itemFMGFile))
            {
                patchItemFMGs = false;
                itemFMGFile = $@"{exeDir}..\..\msg\engus\item_dlc2.msgbnd.dcx";
            }
            var menuFMGFile = $@"{exeDir}\msg\engus\menu_dlc2.msgbnd.dcx";
            if (!File.Exists(menuFMGFile))
            {
                patchMenuFMGs = false;
                menuFMGFile = $@"{exeDir}..\..\msg\engus\menu_dlc2.msgbnd.dcx";
            }
            var paramBND = BND4.Read(gameParamFile);
            var paramDefBND = BND4.Read(paramDefFile);
            var itemFMGBND = BND4.Read(itemFMGFile);
            var menuFMGBND = BND4.Read(menuFMGFile);
            var paramDefs = new List<PARAMDEF>();
            var paramList = new List<PARAM>();

            Console.WriteLine("Backing up GameParam file");
            if (!File.Exists($@"{gameParamFile}.FMG2PBak"))
                File.Copy(gameParamFile, $@"{gameParamFile}.FMG2PBak");

            if (patchItemFMGs || patchMenuFMGs)
            {
                Console.WriteLine("Backing up msgbnd files");
                if (patchItemFMGs)
                    if (!File.Exists($@"{itemFMGFile}.FMG2PBak"))
                        File.Copy(itemFMGFile, $@"{itemFMGFile}.FMG2PBak");

                if (patchMenuFMGs)
                    if (!File.Exists($@"{menuFMGFile}.FMG2PBak"))
                        File.Copy(menuFMGFile, $@"{menuFMGFile}.FMG2PBak");
            }

            ReadFMGs(itemFMGBND, menuFMGBND);
            if (patchItemFMGs || patchMenuFMGs)
            {
                Console.WriteLine("Renaming FMG files");
                RenameFMGs(itemFMGBND, menuFMGBND);
            }

            Console.WriteLine("Renaming Param Rows from FMG files");
            ReadParams(paramBND, paramDefBND, paramDefs, paramList);
            WriteParamBytes(paramBND);

            if (patchItemFMGs || patchMenuFMGs)
            {
                Console.WriteLine("Writing FMGs");
                if (patchItemFMGs)
                    itemFMGBND.Write(itemFMGFile);

                if (patchMenuFMGs)
                    menuFMGBND.Write(menuFMGFile);
            }

            Console.WriteLine("Writing GameParam File");
            paramBND.Write(gameParamFile);
        }

        

        private void ReadFMGs(IBinder itemFMGBND, IBinder menuFMGBND)
        {
            ItemFMGS = new List<FMG>();
            MenuFMGS = new List<FMG>();
            foreach (var item in itemFMGBND.Files)
            {
                ItemFMGS.Add(FMG.Read(item.Bytes));
            }

            foreach (var item in menuFMGBND.Files)
            {
                MenuFMGS.Add(FMG.Read(item.Bytes));
            }
        }

        private void RenameFMGs(IBinder itemFMGBND, IBinder menuFMGBND)
        {
            var path = Path.GetDirectoryName(itemFMGBND.Files[0].Name);

            itemFMGBND.Files[0].Name = $@"{path}\Title - Goods.fmg";
            itemFMGBND.Files[1].Name = $@"{path}\Title - Weapons.fmg";
            itemFMGBND.Files[2].Name = $@"{path}\Title - Armor.fmg";
            itemFMGBND.Files[3].Name = $@"{path}\Title - Rings.fmg";
            itemFMGBND.Files[4].Name = $@"{path}\Title - Spells.fmg";
            itemFMGBND.Files[5].Name = $@"{path}\Title - Characters.fmg";
            itemFMGBND.Files[6].Name = $@"{path}\Title - Locations.fmg";
            itemFMGBND.Files[7].Name = $@"{path}\Summary - Goods.fmg";
            itemFMGBND.Files[8].Name = $@"{path}\Summary - Weapons.fmg";
            itemFMGBND.Files[9].Name = $@"{path}\Summary - Armor.fmg";
            itemFMGBND.Files[10].Name = $@"{path}\Summary - Rings.fmg";
            itemFMGBND.Files[11].Name = $@"{path}\Description - Goods.fmg";
            itemFMGBND.Files[12].Name = $@"{path}\Description - Weapons.fmg";
            itemFMGBND.Files[13].Name = $@"{path}\Description - Armor.fmg";
            itemFMGBND.Files[14].Name = $@"{path}\Description - Rings.fmg";
            itemFMGBND.Files[15].Name = $@"{path}\Summary - Spells.fmg";
            itemFMGBND.Files[16].Name = $@"{path}\Description - Spells.fmg";
            itemFMGBND.Files[17].Name = $@"{path}\Title - Goods - DLC1.fmg";
            itemFMGBND.Files[18].Name = $@"{path}\Title - Weapons - DLC1.fmg";
            itemFMGBND.Files[19].Name = $@"{path}\Title - Armor - DLC1.fmg";
            itemFMGBND.Files[20].Name = $@"{path}\Title - Rings - DLC1.fmg";
            itemFMGBND.Files[21].Name = $@"{path}\Title - Spells - DLC1.fmg";
            itemFMGBND.Files[22].Name = $@"{path}\Title - Characters - DLC1.fmg";
            itemFMGBND.Files[23].Name = $@"{path}\Title - Locations - DLC1.fmg";
            itemFMGBND.Files[24].Name = $@"{path}\Summary - Goods - DLC1.fmg";
            itemFMGBND.Files[25].Name = $@"{path}\Summary - Rings - DLC1.fmg";
            itemFMGBND.Files[26].Name = $@"{path}\Description - Goods - DLC1.fmg";
            itemFMGBND.Files[27].Name = $@"{path}\Description - Weapons - DLC1.fmg";
            itemFMGBND.Files[28].Name = $@"{path}\Description - Armor - DLC1.fmg";
            itemFMGBND.Files[29].Name = $@"{path}\Description - Rings - DLC1.fmg";
            itemFMGBND.Files[30].Name = $@"{path}\Summary - Spells - DLC1.fmg";
            itemFMGBND.Files[31].Name = $@"{path}\Description - Spells - DLC1.fmg";
            itemFMGBND.Files[32].Name = $@"{path}\Title - Goods - DLC2.fmg";
            itemFMGBND.Files[33].Name = $@"{path}\Title - Weapons - DLC2.fmg";
            itemFMGBND.Files[34].Name = $@"{path}\Title - Armor - DLC2.fmg";
            itemFMGBND.Files[35].Name = $@"{path}\Title - Rings - DLC2.fmg";
            itemFMGBND.Files[36].Name = $@"{path}\Title - Spells - DLC2.fmg";
            itemFMGBND.Files[37].Name = $@"{path}\Title - Characters - DLC2.fmg";
            itemFMGBND.Files[38].Name = $@"{path}\Title - Locations - DLC2.fmg";
            itemFMGBND.Files[39].Name = $@"{path}\Summary - Goods - DLC2.fmg";
            itemFMGBND.Files[40].Name = $@"{path}\Summary - Rings - DLC2.fmg";
            itemFMGBND.Files[41].Name = $@"{path}\Description - Goods - DLC2.fmg";
            itemFMGBND.Files[42].Name = $@"{path}\Description - Weapons - DLC2.fmg";
            itemFMGBND.Files[43].Name = $@"{path}\Description - Armor - DLC2.fmg";
            itemFMGBND.Files[44].Name = $@"{path}\Description - Rings - DLC2.fmg";
            itemFMGBND.Files[45].Name = $@"{path}\Summary - Spells - DLC2.fmg";
            itemFMGBND.Files[46].Name = $@"{path}\Description - Spells - DLC2.fmg";

            menuFMGBND.Files[0].Name = $@"{path}\Dialogue.fmg";
            menuFMGBND.Files[1].Name = $@"{path}\Messages.fmg";
            menuFMGBND.Files[2].Name = $@"{path}\Subtitles.fmg";
            menuFMGBND.Files[3].Name = $@"{path}\Menu.fmg";
            menuFMGBND.Files[4].Name = $@"{path}\Titles.fmg";
            menuFMGBND.Files[5].Name = $@"{path}\Unk 3.fmg";
            menuFMGBND.Files[6].Name = $@"{path}\Unk 2.fmg";
            menuFMGBND.Files[7].Name = $@"{path}\Alerts.fmg";
            menuFMGBND.Files[8].Name = $@"{path}\Prompts.fmg";
            menuFMGBND.Files[9].Name = $@"{path}\Unk 4.fmg";
            menuFMGBND.Files[10].Name = $@"{path}\Unk 5.fmg";
            menuFMGBND.Files[11].Name = $@"{path}\Unk 1.fmg";
            menuFMGBND.Files[12].Name = $@"{path}\FDP - Text 2.fmg";
            menuFMGBND.Files[13].Name = $@"{path}\FDP - Text 3.fmg";
            menuFMGBND.Files[14].Name = $@"{path}\FDP - Text.fmg";
            menuFMGBND.Files[15].Name = $@"{path}\FDP - Text - PC.fmg";
            menuFMGBND.Files[16].Name = $@"{path}\FDP - Menu.fmg";
            menuFMGBND.Files[17].Name = $@"{path}\FDP - Text - PS4.fmg";
            menuFMGBND.Files[18].Name = $@"{path}\FDP - Text - XB1.fmg";
            menuFMGBND.Files[19].Name = $@"{path}\Dialogue - DLC1.fmg";
            menuFMGBND.Files[20].Name = $@"{path}\Menu - DLC1.fmg";
            menuFMGBND.Files[21].Name = $@"{path}\FDP - Text 2 - DLC1.fmg";
            menuFMGBND.Files[22].Name = $@"{path}\FDP - Text 3 - DLC1.fmg";
            menuFMGBND.Files[23].Name = $@"{path}\FDP - Text - DLC1 - PC.fmg";
            menuFMGBND.Files[24].Name = $@"{path}\FDP - Menu - DLC1.fmg";
            menuFMGBND.Files[25].Name = $@"{path}\FDP - Text - DLC1 - PS4.fmg";
            menuFMGBND.Files[26].Name = $@"{path}\FDP - Text - DLC1 - XB1.fmg";
            menuFMGBND.Files[27].Name = $@"{path}\Messages - DLC1.fmg";
            menuFMGBND.Files[28].Name = $@"{path}\Dialogue - DLC2.fmg";
            menuFMGBND.Files[29].Name = $@"{path}\Menu - DLC2.fmg";
            menuFMGBND.Files[30].Name = $@"{path}\FDP - Text 2 - DLC2.fmg";
            menuFMGBND.Files[31].Name = $@"{path}\FDP - Text 3 - DLC2.fmg";
            menuFMGBND.Files[32].Name = $@"{path}\FDP - Text - DLC2 - PC.fmg";
            menuFMGBND.Files[33].Name = $@"{path}\FDP - Menu - DLC2.fmg";
            menuFMGBND.Files[34].Name = $@"{path}\FDP - Text - DLC2 - PS4.fmg";
            menuFMGBND.Files[35].Name = $@"{path}\FDP - Text - DLC2 - XB1.fmg";
            menuFMGBND.Files[36].Name = $@"{path}\Messages - DLC2.fmg";
        }

        PARAM EQUIP_PARAM_PROTECTOR_ST;
        PARAM EQUIP_PARAM_WEAPON_ST;
        PARAM EQUIP_PARAM_GOODS_ST;
        PARAM MAGIC_PARAM_ST;
        PARAM EQUIP_PARAM_ACCESSORY_ST;
        PARAM TALK_PARAM_ST;

        private void ReadParams(IBinder paramBND, IBinder paramDefBND, List<PARAMDEF> paramDefs, List<PARAM> paramList)
        {
            foreach (var item in paramBND.Files)
            {
                paramList.Add(PARAM.Read(item.Bytes));
            }

            foreach (var item in paramDefBND.Files)
            {
                paramDefs.Add(PARAMDEF.Read(item.Bytes));
            }

            foreach (var param in paramList)
            {
                var result = param.ApplyParamdefCarefully(paramDefs);
                if (!result)
                    foreach (var paramDef in paramDefs)
                        if (paramDef.ParamType == param.ParamType && (param.DetectedSize == -1 || param.DetectedSize == paramDef.GetRowSize()))
                            param.ApplyParamdef(paramDef);
            }

            foreach (var param in paramList)
            {
                switch (param.ParamType)
                {
                    case "EQUIP_PARAM_PROTECTOR_ST":
                        EQUIP_PARAM_PROTECTOR_ST = param;
                        SortArmors(EQUIP_PARAM_PROTECTOR_ST);
                        break;
                    case "EQUIP_PARAM_WEAPON_ST":
                        EQUIP_PARAM_WEAPON_ST = param;
                        SortWeapons(EQUIP_PARAM_WEAPON_ST);
                        break;
                    case "EQUIP_PARAM_GOODS_ST":
                        EQUIP_PARAM_GOODS_ST = param;
                        SortItems(EQUIP_PARAM_GOODS_ST);
                        break;
                    case "MAGIC_PARAM_ST":
                        MAGIC_PARAM_ST = param;
                        SortSpells(MAGIC_PARAM_ST);
                        break;
                    case "EQUIP_PARAM_ACCESSORY_ST":
                        EQUIP_PARAM_ACCESSORY_ST = param;
                        SortRings(EQUIP_PARAM_ACCESSORY_ST);
                        break;
                    case "TALK_PARAM_ST":
                        TALK_PARAM_ST = param;
                        SortTalk(TALK_PARAM_ST);
                        break;
                    default:
                        break;
                }
            }
        }

        private void SortArmors(PARAM equipProParam)
        {
            var armorNames = ItemFMGS[2].Entries.GroupBy(x => x.ID).Select(x => x.First()).ToDictionary(x => x.ID, x => x.Text);

            foreach (var item in MenuFMGS[19].Entries)
            {
                if (!armorNames.ContainsKey(item.ID))
                    armorNames.Add(item.ID, item.Text);
                else if (string.IsNullOrWhiteSpace(armorNames[item.ID]))
                    armorNames[item.ID] = item.Text;
            }

            foreach (var item in MenuFMGS[34].Entries)
            {
                if (!armorNames.ContainsKey(item.ID))
                    armorNames.Add(item.ID, item.Text);
                else if (string.IsNullOrWhiteSpace(armorNames[item.ID]))
                    armorNames[item.ID] = item.Text;
            }

            foreach (var armor in equipProParam.Rows)
            {
                if (armorNames.ContainsKey(armor.ID))
                {
                    if (string.IsNullOrWhiteSpace(armorNames[armor.ID]))
                        continue;
                    armor.Name = armorNames[armor.ID];
                }
            }
        }

        private void SortWeapons(PARAM equipWepParam)
        {
            var weaponNames = ItemFMGS[1].Entries.GroupBy(x => x.ID).Select(x => x.First()).ToDictionary(x => x.ID, x => x.Text);

            foreach (var item in MenuFMGS[18].Entries)
            {
                if (!weaponNames.ContainsKey(item.ID))
                    weaponNames.Add(item.ID, item.Text);
                else if (string.IsNullOrWhiteSpace(weaponNames[item.ID]))
                    weaponNames[item.ID] = item.Text;
            }

            foreach (var item in MenuFMGS[33].Entries)
            {
                if (!weaponNames.ContainsKey(item.ID))
                    weaponNames.Add(item.ID, item.Text);
                else if (string.IsNullOrWhiteSpace(weaponNames[item.ID]))
                    weaponNames[item.ID] = item.Text;
            }

            foreach (var weapon in equipWepParam.Rows)
            {
                if (weaponNames.ContainsKey(weapon.ID))
                {
                    if (string.IsNullOrWhiteSpace(weaponNames[weapon.ID]))
                        continue;

                    weapon.Name = weaponNames[weapon.ID];
                }
            }
        }

        private void SortItems(PARAM goodsParam)
        {
            var itemNames = ItemFMGS[0].Entries.GroupBy(x => x.ID).Select(x => x.First()).ToDictionary(x => x.ID, x => x.Text);

            foreach (var item in MenuFMGS[17].Entries)
            {
                if (!itemNames.ContainsKey(item.ID))
                    itemNames.Add(item.ID, item.Text);
                else if (string.IsNullOrWhiteSpace(itemNames[item.ID]))
                    itemNames[item.ID] = item.Text;
            }

            foreach (var item in MenuFMGS[26].Entries)
            {
                if (!itemNames.ContainsKey(item.ID))
                    itemNames.Add(item.ID, item.Text);
                else if (string.IsNullOrWhiteSpace(itemNames[item.ID]))
                    itemNames[item.ID] = item.Text;
            }

            foreach (var item in goodsParam.Rows)
            {
                if (itemNames.ContainsKey(item.ID))
                {
                    if (string.IsNullOrWhiteSpace(itemNames[item.ID]))
                        continue;

                    item.Name = itemNames[item.ID];
                }
            }
        }

        private void SortSpells(PARAM magicParam)
        {
            var spellNames = ItemFMGS[4].Entries.GroupBy(x => x.ID).Select(x => x.First()).ToDictionary(x => x.ID, x => x.Text);

            foreach (var item in MenuFMGS[21].Entries)
            {
                if (!spellNames.ContainsKey(item.ID))
                    spellNames.Add(item.ID, item.Text);
                else if (string.IsNullOrWhiteSpace(spellNames[item.ID]))
                    spellNames[item.ID] = item.Text;
            }

            foreach (var item in MenuFMGS[36].Entries)
            {
                if (!spellNames.ContainsKey(item.ID))
                    spellNames.Add(item.ID, item.Text);
                else if (string.IsNullOrWhiteSpace(spellNames[item.ID]))
                    spellNames[item.ID] = item.Text;
            }

            //var npcSpellNames = new Dictionary<int, string>();
            //foreach (var kvp in spellNames)
            //{
            //    npcSpellNames.Add(kvp.Key + 4000000, kvp.Value);
            //}

            foreach (var spell in magicParam.Rows)
            {
                if (spellNames.ContainsKey(spell.ID))
                {
                    if (string.IsNullOrWhiteSpace(spellNames[spell.ID]))
                        continue;
                    spell.Name = spellNames[spell.ID];
                    continue;
                }

                //if (npcSpellNames.ContainsKey(spell.ID))
                //{
                //    if (string.IsNullOrWhiteSpace(npcSpellNames[spell.ID]))
                //        continue;
                //    spell.Name = $"NPC {npcSpellNames[spell.ID]}";
                //    continue;
                //}
            }
        }

        private void SortRings(PARAM accessoryParam)
        {
            var ringNames = ItemFMGS[3].Entries.GroupBy(x => x.ID).Select(x => x.First()).ToDictionary(x => x.ID, x => x.Text);

            foreach (var item in MenuFMGS[20].Entries)
            {
                if (!ringNames.ContainsKey(item.ID))
                    ringNames.Add(item.ID, item.Text);
                else if (string.IsNullOrWhiteSpace(ringNames[item.ID]))
                    ringNames[item.ID] = item.Text;

            }

            foreach (var item in MenuFMGS[35].Entries)
            {
                if (!ringNames.ContainsKey(item.ID))
                    ringNames.Add(item.ID, item.Text);
                else if (string.IsNullOrWhiteSpace(ringNames[item.ID]))
                    ringNames[item.ID] = item.Text;

            }

            foreach (var ring in accessoryParam.Rows)
            {
                if (ringNames.ContainsKey(ring.ID))
                {
                    if (string.IsNullOrWhiteSpace(ringNames[ring.ID]))
                        continue;
                    ring.Name = ringNames[ring.ID];
                }
            }
        }

        private void SortTalk(PARAM talkParam)
        {
            var talkNames = MenuFMGS[0].Entries.GroupBy(x => x.ID).Select(x => x.First()).ToDictionary(x => x.ID, x => x.Text);

            foreach (var item in MenuFMGS[19].Entries)
            {
                if (!talkNames.ContainsKey(item.ID))
                    talkNames.Add(item.ID, item.Text);
                else if (string.IsNullOrWhiteSpace(talkNames[item.ID]))
                    talkNames[item.ID] = item.Text;
            }

            foreach (var item in MenuFMGS[28].Entries)
            {
                if (!talkNames.ContainsKey(item.ID))
                    talkNames.Add(item.ID, item.Text);
                else if (string.IsNullOrWhiteSpace(talkNames[item.ID]))
                    talkNames[item.ID] = item.Text;
            }

            foreach (var armor in talkParam.Rows)
            {
                if (talkNames.ContainsKey(armor.ID))
                {
                    if (string.IsNullOrWhiteSpace(talkNames[armor.ID]))
                        continue;

                    armor.Name = talkNames[armor.ID];
                }
            }
        }

        private void WriteParamBytes(IBinder paramBND)
        {
            foreach (var param in paramBND.Files)
            {
                var paramName = Path.GetFileName(param.Name);

                switch (paramName)
                {
                    case "EquipParamProtector.param":
                        param.Bytes = EQUIP_PARAM_PROTECTOR_ST.Write();
                        break;
                    case "EquipParamWeapon.param":
                        param.Bytes = EQUIP_PARAM_WEAPON_ST.Write();
                        break;
                    case "EquipParamAccessory.param":
                        param.Bytes = EQUIP_PARAM_ACCESSORY_ST.Write();
                        break;
                    case "EquipParamGoods.param":
                        param.Bytes = EQUIP_PARAM_GOODS_ST.Write();
                        break;
                    case "Magic.param":
                        param.Bytes = MAGIC_PARAM_ST.Write();
                        break;
                    case "TalkParam.param":
                        param.Bytes = TALK_PARAM_ST.Write();
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
