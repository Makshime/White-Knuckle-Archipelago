using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static BuffContainer;

namespace WKRando;

public class CustomPerks
{

    private static Perk _apDebuff;
    private static Perk _apBuff;

    public static Perk ApBuff(int stacks = 1)
    {
        _apBuff = ScriptableObject.CreateInstance<Perk>();
        _apBuff.id = "archipelago_buff";
        _apBuff.name = "perk_ap_buff";
        _apBuff.title = "Archipelago Buff";
        _apBuff.description = "You feel the pull of other worlds taking you higher";
        _apBuff.canStack = true;
        _apBuff.stackMax = 100;
        _apBuff.useBuff = true;
        _apBuff.buffMultiplier = 1;
        _apBuff.multiplierCurve = AnimationCurve.Constant(0, 1, 1);
        _apBuff.modules = new List<PerkModule>();
        _apBuff.stackAmount = stacks;
        _apBuff.buff = new BuffContainer()
        {
            id = "archipelago_buff",
            desc = "",
            loseOverTime = false,
            loseRate = 0,
            buffTime = 1,
            multiplier = 1,
            loseRateEffectedByPerks = false,
            buffs = new List<Buff>{
                 NewBuff("addGravity", -0.03f, -0.03f),
                 NewBuff("addStaminaRegen", 0.04f, 0.04f),
                 NewBuff("addStamina",  0.05f, 0.05f),
                 NewBuff("addClimb", 0.06f, 0.06f),
                 NewBuff("addJump", 0.05f, 0.05f),
                 NewBuff("addSpeed", 0.06f, 0.06f)
            }
        };
        Texture2D texture = new Texture2D(256, 256);
        var p = Path.Combine(BepInEx.Paths.PluginPath, "WKRando\\Assets\\Archipelago_Perk.png");
        Plugin.Logger.LogInfo($"Filepath: {p}");
        texture.LoadImage(File.ReadAllBytes(p));
        _apBuff.icon = Sprite.Create(
            texture,
            new Rect(0, 0, texture.width, texture.height),
            new Vector2(0.5f, 0.5f),
            pixelsPerUnit: 900f
            );
        _apBuff.iconMat = CL_AssetManager.baseDatabase.perkAssets[62].iconMat;
        return  _apBuff;
    }
    public static Perk ApDebuff(int stacks = 1)
    {
        _apDebuff = ScriptableObject.CreateInstance<Perk>();
        _apDebuff.id = "archipelago_debuff";
        _apDebuff.name = "perk_ap_debuff";
        _apDebuff.title = "Archipelago Debuff";
        _apDebuff.description = "The grasp of other worlds weighs you down";
        _apDebuff.flavorText = "Debuffs to all stats";
        _apDebuff.stackMax = 100;
        _apDebuff.useBuff = true;
        _apDebuff.buffMultiplier = 1;
        _apDebuff.multiplierCurve = AnimationCurve.Constant(0, 1, 1);
        _apDebuff.modules = new List<PerkModule>();
        _apDebuff.stackAmount = stacks;
        _apDebuff.buff = new BuffContainer
        {
            id = "archipelago_debuff",
            desc = "",
            loseOverTime = false,
            loseRate = 0,
            buffTime = 1,
            multiplier = 1,
            loseRateEffectedByPerks = false,
            buffs = new List<Buff>{
                NewBuff("addGravity", 0.05f, 0.05f),
                NewBuff("addStaminaRegen", -0.01f, -0.01f),
                NewBuff("addStamina",  -0.05f, -0.05f),
                //The climb and jump debuffs make it even worse than maso!
                NewBuff("addClimb", -0.005f, -0.005f),
                NewBuff("addJump", -0.005f, -0.005f),
                NewBuff("addSpeed", -0.03f, -0.03f)
            }
        };
        Texture2D texture1 = new Texture2D(256, 256);
        texture1.LoadImage(File.ReadAllBytes(Path.Combine(BepInEx.Paths.PluginPath, "WKRando\\Assets\\Broken_Archipelago_Perk.png")));
        _apDebuff.icon = Sprite.Create(
            texture1,
            new Rect(0, 0, texture1.width, texture1.height),
            new Vector2(0.5f, 0.5f),
            pixelsPerUnit: 900f
        );
        _apDebuff.iconMat = CL_AssetManager.baseDatabase.perkAssets[10].iconMat;
        return  _apDebuff;
    }

    private static Buff NewBuff(string id, float amount, float maxAmount)
    {
        Buff buff = new Buff();
        buff.id = id;
        buff.amount = amount;
        buff.maxAmount = maxAmount;
        return buff;
    }

    
    //Was for an idea of messing around with how the debuffs/buffs are translated, stays here in case I still want it for some reason
    private static AnimationCurve CreateExponentialDecayCurve(float decay, int keyFrameCount = 100)
    {
        Keyframe[] keyframes = new Keyframe[keyFrameCount];
        float value = 1f;
        
        for (int i = 0; i < keyFrameCount; i++)
        {
            keyframes[i] = new Keyframe((float)i / (keyFrameCount - 1), value);
            value *= decay;
        }
        
        return new AnimationCurve(keyframes);
    }
}