using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using WgMod.Common.Players;

namespace WgMod.Content.Items.Armor.VacuumArmor;

[AutoloadEquip(EquipType.Body)]
public class VacuumCrop : ModItem
{
    float _attack;
    int _health;
    int _defense;
    float _resist;
    float _movePenalty;

    public override void SetDefaults()
    {
        Item.width = 18;
        Item.height = 18;
        Item.value = Item.sellPrice(gold: 1);
        Item.rare = ItemRarityID.Red;
        Item.defense = 46;
    }

    public override void UpdateEquip(Player player)
    {
        if (!player.TryGetModPlayer(out WgPlayer wg))
            return;

        float immobility = wg.Weight.ClampedImmobility;
        _attack = float.Lerp(0.06f, 0.12f, immobility);
        _health = (int)MathF.Floor((int)float.Lerp(100f, 200f, immobility) / 5f) * 5;
        _defense = (int)float.Lerp(12f, 24f, immobility);
        _resist = float.Lerp(0.03f, 0.06f, immobility);
        _movePenalty = float.Lerp(1.15f, 0.9f, immobility);

        player.GetDamage(DamageClass.Generic) += _attack;
        player.statLifeMax2 += _health;
        player.statDefense += _defense;
        player.endurance += _resist;
        wg.MovementPenalty *= _movePenalty;

        player.aggro += 5;
    }

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient(ItemID.FragmentSolar, 5)
            .AddIngredient(ItemID.FragmentNebula, 5)
            .AddIngredient(ItemID.FragmentVortex, 5)
            .AddIngredient(ItemID.FragmentStardust, 5)
            .AddIngredient(ItemID.LunarBar, 16)
            .AddTile(TileID.LunarCraftingStation)
            .Register();
    }
}
