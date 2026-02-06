using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using WgMod.Common.Players;
using WgMod.Content.Buffs;

namespace WgMod;

public partial class WgMod
{
    public static void RegisterHooks()
    {
        On_Player.AddBuff += OnPlayer_AddBuff;
        On_Player.DelBuff += OnPlayer_DelBuff;
        On_Player.GetItemGrabRange += OnPlayer_GetItemGrabRange;
        On_PlayerSittingHelper.UpdateSitting += OnUpdateSitting;
        On_Mount.Draw += OnMount_Draw;
        On_Main.GetPlayerArmPosition += OnMain_GetPlayerArmPosition;
    }

    static void UnregisterHooks()
    {
        On_Player.AddBuff -= OnPlayer_AddBuff;
        On_Player.DelBuff -= OnPlayer_DelBuff;
        On_Player.GetItemGrabRange -= OnPlayer_GetItemGrabRange;
        On_PlayerSittingHelper.UpdateSitting -= OnUpdateSitting;
        On_Mount.Draw -= OnMount_Draw;
        On_Main.GetPlayerArmPosition -= OnMain_GetPlayerArmPosition;
    }

    static void OnPlayer_AddBuff(On_Player.orig_AddBuff orig, Player self, int type, int timeToAdd, bool quiet, bool foodHack)
    {
        if (!self.TryGetModPlayer(out WgPlayer wg))
        {
            orig(self, type, timeToAdd, quiet, foodHack);
            return;
        }

        int previousTime = int.MinValue;
        if (self.HasBuff(type))
            previousTime = self.buffTime[self.FindBuffIndex(type)];
        orig(self, type, timeToAdd, quiet, foodHack);
        if (!self.HasBuff(type))
            return;

        int index = self.FindBuffIndex(type);
        wg.BuffDuration[index] = timeToAdd;

        if (wg._ignoreWgBuffTimer > 0)
            return;

        if (_buffTable.TryGetValue(type, out var gain))
        {
            if (gain.IsInstant)
            {
                if (previousTime < timeToAdd - 2) // Apply once (2 ticks of leeway)
                    wg.SetWeight(wg.Weight + gain.TotalGain);
            }
            else if (!self.HasBuff<GainingBuff>())
                GainingBuff.AddBuff(wg, gain);
        }
    }

    static void OnPlayer_DelBuff(On_Player.orig_DelBuff orig, Player self, int index)
    {
        if (self.TryGetModPlayer(out WgPlayer wg))
        {
            wg.BuffDuration[index] = 0;
            int num = 0;
            for (int i = 0; i < wg.BuffDuration.Length - 1; i++)
            {
                if (wg.BuffDuration[i] != 0)
                {
                    if (num < i)
                    {
                        wg.BuffDuration[num] = wg.BuffDuration[i];
                        wg.BuffDuration[i] = 0;
                    }
                    num++;
                }
            }
        }
        orig(self, index);
    }

    static Vector2 OnMain_GetPlayerArmPosition(On_Main.orig_GetPlayerArmPosition orig, Projectile proj)
    {
        Player player = Main.player[proj.owner];
        Vector2 vector = Main.OffsetsPlayerOnhand[player.bodyFrame.Y / 56] * 2f;
        if (player.direction != 1)
            vector.X = player.bodyFrame.Width - vector.X;
        if (player.gravDir != 1f)
            vector.Y = player.bodyFrame.Height - vector.Y;
        vector -= new Vector2(player.bodyFrame.Width - player.width, player.bodyFrame.Height - player.height) / 2f;
        Vector2 pos = player.MountedCenter - new Vector2(player.width, player.height) / 2f + vector + Vector2.UnitY * player.gfxOffY;
        if (player.mount.Active && player.mount.Type == MountID.Wolf)
        {
            pos.Y -= player.mount.PlayerOffsetHitbox;
            pos += new Vector2(12 * player.direction, -12f);
        }
        return player.RotatedRelativePoint(pos, false, true);
    }

    // This is needed for BottomlessAppetite item pickup range changes!
    static int OnPlayer_GetItemGrabRange(On_Player.orig_GetItemGrabRange orig, Player self, Item item)
    {
        int num = Player.defaultItemGrabRange;
        if (self.goldRing && item.IsACoin)
            num += Item.coinGrabRange;
        if (self.manaMagnet && (item.type == ItemID.Star || item.type == ItemID.SoulCake || item.type == ItemID.SugarPlum))
            num += Item.manaGrabRange;
        if (item.type == ItemID.ManaCloakStar)
            num += Item.manaGrabRange;
        if (self.lifeMagnet && (item.type == ItemID.Heart || item.type == ItemID.CandyApple || item.type == ItemID.CandyCane))
            num += Item.lifeGrabRange;
        if (self.treasureMagnet)
            num += Item.treasureGrabRange;
        if (item.type == ItemID.DD2EnergyCrystal)
            num += 50;
        if (ItemID.Sets.NebulaPickup[item.type])
            num += 100;
        if (self.difficulty == PlayerDifficultyID.Creative && CreativePowerManager.Instance.GetPower<CreativePowers.FarPlacementRangePower>().IsEnabledForPlayer(self.whoAmI))
            num += 240;
        if (self.TryGetModPlayer(out WgPlayer wg))
        {
            if (wg._bottomlessAppetite && item.type != ItemID.FallenStar)
                num *= wg._bottomlessAppetiteGrabRange;
        }
        ItemLoader.GrabRange(item, self, ref num);
        return num;
    }

    static void OnUpdateSitting(On_PlayerSittingHelper.orig_UpdateSitting orig, ref PlayerSittingHelper self, Player player)
    {
        if (!player.TryGetModPlayer(out TreadmillPlayer tp) || !tp._onTreadmill)
        {
            orig(ref self, player);
            return;
        }
        bool left = player.controlLeft;
        bool right = player.controlRight;
        player.controlLeft = false;
        player.controlRight = false;
        orig(ref self, player);
        player.controlLeft = left;
        player.controlRight = right;
    }

    static void OnMount_Draw(On_Mount.orig_Draw orig, Mount self, List<DrawData> playerDrawData, int drawType, Player drawPlayer, Vector2 Position, Color drawColor, SpriteEffects playerEffect, float shadow)
    {
        if (drawPlayer.TryGetModPlayer(out WgPlayer wg) && self.Active)
            Position.Y += WeightValues.DrawOffsetY(wg.Weight.GetStage());
        orig(self, playerDrawData, drawType, drawPlayer, Position, drawColor, playerEffect, shadow);
    }
}
