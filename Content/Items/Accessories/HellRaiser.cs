using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using WgMod.Common.Players;

namespace WgMod.Content.Items.Accessories;

public class HellRaiser : ModItem
{
    public const int MinionCount = 3;

    float _minionDamage;
    float _whipSpeed;

    public override void SetDefaults()
    {
        Item.width = 26;
        Item.height = 32;

        Item.accessory = true;
        Item.rare = ItemRarityID.LightRed;
        Item.value = Item.buyPrice(gold: 1);
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        if (!player.TryGetModPlayer(out WgPlayer wg))
            return;
            
        float immobility = wg.Weight.ClampedImmobility;
        _minionDamage = float.Lerp(-0.1f, 0.1f, immobility);
        _whipSpeed = float.Lerp(0.9f, 0.8f, immobility);

        player.maxMinions += MinionCount;
        player.GetDamage(DamageClass.Summon) += _minionDamage;
        player.GetAttackSpeed(DamageClass.SummonMeleeSpeed) *= _whipSpeed;
    }

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient(ItemID.HellstoneBar, 12)
            .AddIngredient(ItemID.SoulofNight, 6)
            .AddTile(TileID.MythrilAnvil)
            .Register();
    }
}
