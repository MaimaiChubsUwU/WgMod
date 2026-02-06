using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using WgMod.Common.Players;

namespace WgMod.Content.Items.Armor.VacuumArmor;

[AutoloadEquip(EquipType.Legs)]
public class VacuumSkirt : ModItem
{
    float _attackSpeed;
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
        Item.defense = 32;
    }

    public override void UpdateEquip(Player player)
    {
        if (!player.TryGetModPlayer(out WgPlayer wg))
            return;
            
        float immobility = wg.Weight.ClampedImmobility;
        _attackSpeed = float.Lerp(1.06f, 1.12f, immobility);
        _health = (int)MathF.Floor((int)float.Lerp(50, 100, immobility) / 5f) * 5;
        _defense = (int)float.Lerp(8f, 16f, immobility);
        _resist = float.Lerp(0.03f, 0.06f, immobility);
        _movePenalty = float.Lerp(1.10f, 0.95f, immobility);

        player.GetAttackSpeed(DamageClass.Generic) *= _attackSpeed;
        player.statLifeMax2 += _health;
        player.statDefense += _defense;
        player.endurance += _resist;
        wg.MovementPenalty *= _movePenalty;

        player.aggro += 5;
    }

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient(ItemID.FragmentSolar, 3)
            .AddIngredient(ItemID.FragmentNebula, 3)
            .AddIngredient(ItemID.FragmentVortex, 3)
            .AddIngredient(ItemID.FragmentStardust, 3)
            .AddIngredient(ItemID.LunarBar, 12)
            .AddTile(TileID.LunarCraftingStation)
            .Register();
    }
}
