using SoulsFormats;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace FMG2ParamName
{
    class DarkSouls1
    {
        public string ExeDir = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);
        public List<FMG> ItemFMGS { get; private set; }
        public List<FMG> MenuFMGS { get; private set; }

        public void RenameParamRows()
        {
#if DEBUG
            ExeDir = @"F:\Dark Souls Mod Stuff\Vanilla PTDE Translated\DATA";
#endif
            var gameParamFile = $@"{ExeDir}\param\GameParam\GameParam.parambnd";
            var paramDefFile = $@"{ExeDir}\paramdef\paramdef.paramdefbnd";
            var itemFMGFile = $@"{ExeDir}\msg\ENGLISH\item.msgbnd";
            var menuFMGFile = $@"{ExeDir}\msg\ENGLISH\menu.msgbnd";
            var paramBND = BND3.Read(gameParamFile);
            var paramDefBND = BND3.Read(paramDefFile);
            var itemFMGBND = BND3.Read(itemFMGFile);
            var menuFMGBND = BND3.Read(menuFMGFile);
            var paramDefs = new List<PARAMDEF>();
            var paramList = new List<PARAM>();

            if (!File.Exists($@"{gameParamFile}.bak"))
                File.Copy(gameParamFile, $@"{gameParamFile}.bak");

            ReadFMGs(itemFMGBND, menuFMGBND);
            RenameFMGs(itemFMGBND, menuFMGBND);
            ReadParams(paramBND, paramDefBND, paramDefs, paramList);
            WriteParamBytes(paramBND);

            itemFMGBND.Write(itemFMGFile);
            menuFMGBND.Write(menuFMGFile);
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

        private void RenameFMGs(BND3 itemFMGBND, BND3 menuFMGBND)
        {
            var path = Path.GetDirectoryName(itemFMGBND.Files[0].Name);

            itemFMGBND.Files[0].Name = $@"{path}\Goods name.fmg";
            itemFMGBND.Files[1].Name = $@"{path}\Weapon name.fmg";
            itemFMGBND.Files[2].Name = $@"{path}\Armor name.fmg";
            itemFMGBND.Files[3].Name = $@"{path}\Accessory name.fmg";
            itemFMGBND.Files[4].Name = $@"{path}\Magic name.fmg";
            itemFMGBND.Files[5].Name = $@"{path}\Test.fmg";
            itemFMGBND.Files[6].Name = $@"{path}\For Test.fmg";
            itemFMGBND.Files[7].Name = $@"{path}\For Test 2.fmg";
            itemFMGBND.Files[8].Name = $@"{path}\NPC name.fmg";
            itemFMGBND.Files[9].Name = $@"{path}\Area name.fmg";
            itemFMGBND.Files[10].Name = $@"{path}\Goods short description.fmg";
            itemFMGBND.Files[11].Name = $@"{path}\Weapon Categories.fmg";
            itemFMGBND.Files[12].Name = $@"{path}\Weapon and Armor blank category entries.fmg";
            itemFMGBND.Files[13].Name = $@"{path}\Accessory short description.fmg";
            itemFMGBND.Files[14].Name = $@"{path}\Goods long description.fmg";
            itemFMGBND.Files[15].Name = $@"{path}\Weapon long description.fmg";
            itemFMGBND.Files[16].Name = $@"{path}\Armor long description.fmg";
            itemFMGBND.Files[17].Name = $@"{path}\Accessory long description.fmg";
            itemFMGBND.Files[18].Name = $@"{path}\Magic short description.fmg";
            itemFMGBND.Files[19].Name = $@"{path}\Magic long description.fmg";


            menuFMGBND.Files[0].Name = $@"{path}\NPC dialogue.fmg";
            menuFMGBND.Files[1].Name = $@"{path}\Soapstone guidance message.fmg";
            menuFMGBND.Files[2].Name = $@"{path}\Intro cinematic subtitle.fmg";
            menuFMGBND.Files[3].Name = $@"{path}\In-game prompts and NPC menu text.fmg";
            menuFMGBND.Files[4].Name = $@"{path}\Menu labels and Emote name.fmg";
            menuFMGBND.Files[5].Name = $@"{path}\Menu text 1.fmg";
            menuFMGBND.Files[6].Name = $@"{path}\Menu text 2.fmg";
            menuFMGBND.Files[7].Name = $@"{path}\Menu text 3.fmg";
            menuFMGBND.Files[8].Name = $@"{path}\Unknown.fmg";
            menuFMGBND.Files[9].Name = $@"{path}\Menu prompt.fmg";
            menuFMGBND.Files[10].Name = $@"{path}\Unknown Japanese.fmg";
            menuFMGBND.Files[11].Name = $@"{path}\Unknown misc entries.fmg";
            menuFMGBND.Files[12].Name = $@"{path}\Unknown steam stuff.fmg";
            menuFMGBND.Files[13].Name = $@"{path}\Menu warnings and text.fmg";
            menuFMGBND.Files[14].Name = $@"{path}\Online and DLC item description.fmg";
            menuFMGBND.Files[15].Name = $@"{path}\Online message text.fmg";
            menuFMGBND.Files[16].Name = $@"{path}\Menu text 4.fmg";
            menuFMGBND.Files[17].Name = $@"{path}\Menu text 5.fmg";
            menuFMGBND.Files[18].Name = $@"{path}\NPC dialogue 2.fmg";
            menuFMGBND.Files[19].Name = $@"{path}\DLC spell description.fmg";
            menuFMGBND.Files[20].Name = $@"{path}\DLC seapon description.fmg";
            menuFMGBND.Files[21].Name = $@"{path}\DLC multiplayer message.fmg";
            menuFMGBND.Files[22].Name = $@"{path}\DLC armor description.fmg";
            menuFMGBND.Files[23].Name = $@"{path}\DLC ring description.fmg";
            menuFMGBND.Files[24].Name = $@"{path}\DLC item description.fmg";
            menuFMGBND.Files[25].Name = $@"{path}\DLC item name.fmg";
            menuFMGBND.Files[26].Name = $@"{path}\Effect decription.fmg";
            menuFMGBND.Files[27].Name = $@"{path}\DLC ring name.fmg";
            menuFMGBND.Files[28].Name = $@"{path}\DLCand boss weapon category.fmg";
            menuFMGBND.Files[29].Name = $@"{path}\DLC and boss weapon name.fmg";
            menuFMGBND.Files[30].Name = $@"{path}\DLC null entries.fmg";
            menuFMGBND.Files[31].Name = $@"{path}\DLC armor name.fmg";
            menuFMGBND.Files[32].Name = $@"{path}\DLC magic name.fmg";
            menuFMGBND.Files[33].Name = $@"{path}\DLC NPC name.fmg";
            menuFMGBND.Files[34].Name = $@"{path}\DLC location name.fmg";
            menuFMGBND.Files[35].Name = $@"{path}\Menu text 6.fmg";
            menuFMGBND.Files[36].Name = $@"{path}\Menu text 7.fmg";
            menuFMGBND.Files[37].Name = $@"{path}\Menu text 8.fmg";
            menuFMGBND.Files[38].Name = $@"{path}\Menu text 9.fmg";
        }

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
                    Debug.WriteLine($"{param.ParamType} Did not apply!");
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
                    case "CHARACTER_INIT_PARAM":
                        CHARACTER_INIT_PARAM = param;
                        SortClasses(CHARACTER_INIT_PARAM);
                        break;
                    default:
                        break;
                }
            }
        }

        

            PARAM EQUIP_PARAM_PROTECTOR_ST;
        PARAM EQUIP_PARAM_WEAPON_ST;
        PARAM EQUIP_PARAM_GOODS_ST;
        PARAM MAGIC_PARAM_ST;
        PARAM EQUIP_PARAM_ACCESSORY_ST;
        PARAM TALK_PARAM_ST;
        PARAM CHARACTER_INIT_PARAM;

        

        private void SortArmors(PARAM equipProParam)
        {
            var armorNames = ItemFMGS[2].Entries.GroupBy(x => x.ID).Select(x => x.First()).ToDictionary(x => x.ID, x => x.Text);

            foreach (var item in MenuFMGS[31].Entries)
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
            foreach (var item in MenuFMGS[29].Entries)
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

            foreach (var item in MenuFMGS[25].Entries)
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

            foreach (var item in MenuFMGS[25].Entries)
            {
                if (!spellNames.ContainsKey(item.ID))
                    spellNames.Add(item.ID, item.Text);
                else if (string.IsNullOrWhiteSpace(spellNames[item.ID]))
                    spellNames[item.ID] = item.Text;
            }

            foreach (var spell in magicParam.Rows)
            {
                if (spellNames.ContainsKey(spell.ID))
                {
                    if (string.IsNullOrWhiteSpace(spellNames[spell.ID]))
                        continue;
                    spell.Name = spellNames[spell.ID];
                }
            }
        }

        private void SortRings(PARAM accessoryParam)
        {
            var ringNames = ItemFMGS[3].Entries.GroupBy(x => x.ID).Select(x => x.First()).ToDictionary(x => x.ID, x => x.Text);

            foreach (var item in MenuFMGS[27].Entries)
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

            foreach (var item in MenuFMGS[18].Entries)
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

        private void SortClasses(PARAM charInitParam)
        {
            var classNames = MenuFMGS[6].Entries.GroupBy(x => x.ID).Select(x => x.First()).ToDictionary(x => x.ID, x => x.Text);

            foreach (var param in charInitParam.Rows)
            {
                switch (param.ID)
                {
                    case 2000:
                        param.Name = $"Starting Gear: {classNames[132020]}";
                        break;
                    case 2001:
                        param.Name = $"Starting Gear: {classNames[132021]}";
                        break;
                    case 2002:
                        param.Name = $"Starting Gear: {classNames[132022]}";
                        break;
                    case 2003:
                        param.Name = $"Starting Gear: {classNames[132023]}";
                        break;
                    case 2004:
                        param.Name = $"Starting Gear: {classNames[132024]}";
                        break;
                    case 2005:
                        param.Name = $"Starting Gear: {classNames[132025]}";
                        break;
                    case 2006:
                        param.Name = $"Starting Gear: {classNames[132026]}";
                        break;
                    case 2007:
                        param.Name = $"Starting Gear: {classNames[132027]}";
                        break;
                    case 2008:
                        param.Name = $"Starting Gear: {classNames[132028]}";
                        break;
                    case 2009:
                        param.Name = $"Starting Gear: {classNames[132029]}";
                        break;
                    case 3000:
                        param.Name = $"Starting Display: {classNames[132020]}";
                        break;
                    case 3001:
                        param.Name = $"Starting Display: {classNames[132021]}";
                        break;
                    case 3002:
                        param.Name = $"Starting Display: {classNames[132022]}";
                        break;
                    case 3003:
                        param.Name = $"Starting Display: {classNames[132023]}";
                        break;
                    case 3004:
                        param.Name = $"Starting Display: {classNames[132024]}";
                        break;
                    case 3005:
                        param.Name = $"Starting Display: {classNames[132025]}";
                        break;
                    case 3006:
                        param.Name = $"Starting Display: {classNames[132026]}";
                        break;
                    case 3007:
                        param.Name = $"Starting Display: {classNames[132027]}";
                        break;
                    case 3008:
                        param.Name = $"Starting Display: {classNames[132028]}";
                        break;
                    case 3009:
                        param.Name = $"Starting Display: {classNames[132029]}";
                        break;
                    default:
                        break;
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
                    case "CharaInitParam.param":
                        param.Bytes = CHARACTER_INIT_PARAM.Write();
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
