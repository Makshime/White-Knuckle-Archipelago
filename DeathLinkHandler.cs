using Archipelago.MultiClient.Net.BounceFeatures.DeathLink;
using System;
using System.Collections.Generic;
using System.Net.Security;
using System.Text;
using BepInEx;
using UnityEngine;

namespace WKRando;

internal class DeathLinkHandler
{
    /* now, i know its not a big part of it, and this can go into the archipelagoclient.cs
    but also, that file is getting really big and annoying to navigate
    now i know its not that big
    but also like... its more navigatable to find what you want if i go to a new file??
    why am i tryna justify myself lmao, i made this decision and you can change it if you dont like it */
    
    //TODO: FIX THIS MESS!!!!!
    
    public static void ProcDeathlink(DeathLink DeathlinkObject)
    {
        DeathlinkDeath(DeathlinkObject);
    }

    public static void LocalSendDeathLink(string reason)
    {

        Plugin.Logger.LogInfo("Died to " + reason + " As Player" + Plugin.ClientOptions.User);

        if (!Plugin.ClientOptions.Deathlink || reason.IsNullOrWhiteSpace())
        {
            Plugin.Logger.LogInfo("Deathlink is disabled");
            return;
        }

        ArchipelagoClient.Deathlinkservice.SendDeathLink(
            new DeathLink(
            Plugin.ClientOptions.User.IsNullOrWhiteSpace() ? "white knuckle player" : Plugin.ClientOptions.User,
            "was " + reason
            ));
        
        Plugin.Logger.LogInfo("Successfully sent DeathLink");
    }
    
    public static bool deathlinkbusy = false;
    public static int DeathLinksSentSinceLast = 0;
    private static void DeathlinkDeath(DeathLink DeathlinkObject)
    {
        // deathlink that causes you to die
        // uses the amnesty option

        DeathLinksSentSinceLast++;
        if (DeathLinksSentSinceLast >= Plugin.ClientOptions.DeathlinkAmnesty  && Plugin.ClientOptions.DeathlinkAmnesty != 0)
        {
            deathlinkbusy = true;
            ArchipelagoClient.DeathMessage = DeathlinkObject.Cause;
            Plugin.Logger.LogInfo($"Killing player because of deathlink {DeathlinkObject.Source}, {DeathlinkObject.Cause}");
            ENT_Player.playerObject.Kill("deathlink");
        }
        else
        {
            CL_ProgressionManager.ShowUnlockPopup(APItems.SpriteFromPath("WKRando/Assets/Archipelago_Deathlink_Receive.png"),
                "Deathlink Stack Received Due To",
                DeathlinkObject.Cause,
                new Color(0.4f, 0.1f, 0.1f),
                addToSession: false);
        }
    }
}

