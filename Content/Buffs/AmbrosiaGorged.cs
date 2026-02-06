using Terraria;
using Terraria.ModLoader;
using WgMod.Common.Players;

namespace WgMod.Content.Buffs;

public class AmbrosiaGorged : ModBuff
{
    public const float MaxMoveSpeed = 1.5f;
    public const int MaxDefense = 10;
    public const int MaxRegen = 5;

    float _moveSpeed;
    int _defense;
    int _regen;

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
        _moveSpeed = float.Lerp(1.25f, MaxMoveSpeed, immobility);
        _defense = (int)float.Lerp(1f, MaxDefense, immobility);
        _regen = (int)float.Lerp(1f, MaxRegen, immobility);

        player.moveSpeed *= _moveSpeed;
        player.maxRunSpeed *= _moveSpeed;
        player.runAcceleration *= _moveSpeed;
        player.accRunSpeed *= _moveSpeed;
        player.statDefense += _defense;
        player.lifeRegen += _regen;
    }

    public override void ModifyBuffText(ref string buffName, ref string tip, ref int rare)
    {
        tip = base.Description.Format(
            (_moveSpeed - 1f).Percent(MaxMoveSpeed - 1f),
            _defense.OutOf(MaxDefense),
            _regen.OutOf(MaxRegen)
        );
    }
}
