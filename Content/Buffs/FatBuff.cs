using System;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace WgMod.Content.Buffs
{
    public class FatBuff : ModBuff
    {
        public const int DefenseBonus = 5;

        public override LocalizedText Description => base.Description.WithFormatArgs(DefenseBonus);

        public override void SetStaticDefaults()
        {
            Main.buffNoTimeDisplay[Type] = true;
            Main.buffNoSave[Type] = true;
            BuffID.Sets.TimeLeftDoesNotDecrease[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.statDefense += DefenseBonus;
        }
    }
}