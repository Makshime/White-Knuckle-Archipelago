using Archipelago.MultiClient.Net.BounceFeatures.DeathLink;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Security;
using System.Text;
using BepInEx;
using UnityEngine;

namespace WKRando;

internal static class DeathLinkHandler
{
    /* now, i know its not a big part of it, and this can go into the archipelagoclient.cs
    but also, that file is getting really big and annoying to navigate
    now i know its not that big
    but also like... its more navigatable to find what you want if i go to a new file??
    why am i tryna justify myself lmao, i made this decision and you can change it if you dont like it */
    
    //TODO: FIX THIS MESS!!!!!
    private static Sprite _amnestyIcon;

    public static void Awake()
    {
        _amnestyIcon = APItems.SpriteFromPath("WKRando/Assets/Archipelago_Deathlink_Receive.png");
    }
    
    public static void ProcDeathlink(DeathLink DeathlinkObject)
    {
        DeathlinkDeath(DeathlinkObject);
    }

    public static void LocalSendDeathLink(string reason)
    {

        Plugin.Logger.LogInfo("Died to " + reason + " As Player " + Plugin.ClientOptions.User);

        if (!Plugin.ClientOptions.Deathlink || ArchipelagoClient.Deathlinkservice == null || reason.IsNullOrWhiteSpace())
        {
            Plugin.Logger.LogInfo("Deathlink is disabled");
            return;
        }

        ArchipelagoClient.Deathlinkservice.SendDeathLink(
            new DeathLink(
            Plugin.ClientOptions.User.IsNullOrWhiteSpace() ? "white knuckle player" : Plugin.ClientOptions.User,
            Plugin.ClientOptions.User + ": " + reason.ToLower()
            ));
        
        Plugin.Logger.LogInfo("Successfully sent DeathLink");
    }


    
    public static bool deathlinkbusy = false;
    public static int DeathLinksSentSinceLast = 0;
    private static void DeathlinkDeath(DeathLink DeathlinkObject)
    {
        if (!Plugin.ClientOptions.Deathlink || Plugin.ClientOptions.DeathlinkAmnesty == 0)
            return;
        
        Plugin.Logger.LogInfo("Starting Deathlink");
        // deathlink that causes you to die
        // uses the amnesty option

        DeathLinksSentSinceLast++;
        if (DeathLinksSentSinceLast >= Plugin.ClientOptions.DeathlinkAmnesty)
        {
            deathlinkbusy = true; 
            if(DeathlinkObject?.Cause != null)
                ArchipelagoClient.DeathMessage = DeathlinkObject.Cause;
            Plugin.Logger.LogInfo($"Killing player because of deathlink {DeathlinkObject?.Source ?? "Some player"}, {DeathlinkObject?.Cause ?? "Unknown Cause"}");
            ENT_Player.playerObject.Kill("deathlink",  null);
            DeathLinksSentSinceLast = 0;
        }
        else
        {
            Plugin.Logger.LogInfo("Deathlink caught by amnesty");
            CL_ProgressionManager.ShowUnlockPopup(
                _amnestyIcon,
                "Deathlink Stack Received",
                DeathlinkObject?.Cause ?? "Unknown Cause",
                new Color(0.4f, 0.1f, 0.1f)
            );
        }

        deathlinkbusy = false;
    }
}

