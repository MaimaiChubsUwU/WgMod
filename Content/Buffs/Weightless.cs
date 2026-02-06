using System;
using Humanizer;
using Terraria;
using Terraria.ModLoader;
using WgMod.Common.Players;

namespace WgMod.Content.Buffs;

public class Weightless : ModBuff
{
    public const float MaxPenaltyReduction = 0.8f;
    float _movementPenalty;

    public override void SetStaticDefaults()
    {
        Main.debuff[Type] = false;
        Main.pvpBuff[Type] = true;
        Main.buffNoSave[Type] = true;
    }

    public override void Update(Player player, ref int buffIndex)
    {
        if (!player.TryGetModPlayer(out WgPlayer wg))
            return;
        float immobility = wg.Weight.ClampedImmobility;
        _movementPenalty = float.Lerp(1f, MaxPenaltyReduction, immobility);
        wg.MovementPenalty *= _movementPenalty;
    }

    public override void ModifyBuffText(ref string buffName, ref string tip, ref int rare)
    {
        tip = base.Description.Format((1f - _movementPenalty).Percent(1f - MaxPenaltyReduction));
    }
}
