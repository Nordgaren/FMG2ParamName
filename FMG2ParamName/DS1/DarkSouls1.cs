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

            //Read Data
            ReadFMGs(itemFMGBND, menuFMGBND);
            ReadParams(paramBND, paramDefBND, paramDefs, paramList);

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
                    case "CharaInitParam.param":
                        param.Bytes = CHARACTER_INIT_PARAM.Write();
                        break;
                    default:
                        break;
                }
            }

            paramBND.Write(gameParamFile);
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
        PARAM CHARACTER_INIT_PARAM;

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
                    armor.Name = armorNames[armor.ID];
            }
        }

        private void SortWeapons(PARAM equipWepParam)
        {
            //Make weaponNames dictionary
            var weaponNames = ItemFMGS[1].Entries.GroupBy(x => x.ID).Select(x => x.First()).ToDictionary(x => x.ID, x => x.Text);
            foreach (var item in MenuFMGS[29].Entries)
            {
                if (!weaponNames.ContainsKey(item.ID))
                    weaponNames.Add(item.ID, item.Text);
                else if (string.IsNullOrWhiteSpace(weaponNames[item.ID]))
                    weaponNames[item.ID] = item.Text;
            }

            //Add Weapons to WeaponList
            foreach (var weapon in equipWepParam.Rows)
            {
                if (weaponNames.ContainsKey(weapon.ID))
                    weapon.Name = weaponNames[weapon.ID];
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
                    item.Name = itemNames[item.ID];
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
                    spell.Name = spellNames[spell.ID];
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
                    ring.Name = ringNames[ring.ID];
            }
        }

        private void SortClasses(PARAM charInitParam)
        {
            var classNames = MenuFMGS[6].Entries.GroupBy(x => x.ID).Select(x => x.First()).ToDictionary(x => x.ID, x => x.Text);

            foreach (var param in charInitParam.Rows)
            {
                switch (param.ID)
                {
                    case 3000:
                        param.Name = classNames[132020];
                        break;
                    case 3001:
                        param.Name = classNames[132021];
                        break;
                    case 3002:
                        param.Name = classNames[132022];
                        break;
                    case 3003:
                        param.Name = classNames[132023];
                        break;
                    case 3004:
                        param.Name = classNames[132024];
                        break;
                    case 3005:
                        param.Name = classNames[132025];
                        break;
                    case 3006:
                        param.Name = classNames[132026];
                        break;
                    case 3007:
                        param.Name = classNames[132027];
                        break;
                    case 3008:
                        param.Name = classNames[132028];
                        break;
                    case 3009:
                        param.Name = classNames[132029];
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
