using Archipelago.MultiClient.Net.BounceFeatures.DeathLink;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace WKRando
{
    internal class Deathlink
    {
        /* now, i know its not a big part of it, and this can go into the archipelagoclient.cs
        but also, that file is getting really big and annoying to navigate
        now i know its not that big
        but also like... its more navigatable to find what you want if i go to a new file??
        why am i tryna justify myself lmao, i made this decision and you can change it if you dont like it */

        public static void ProcDeathlink(DeathLink DeathlinkObject)
        {
            DeathlinkDeath(DeathlinkObject);
        }

        public static bool deathlinkbusy = false;
        public static int DeathLinksSentSinceLast = 0;
        private static void DeathlinkDeath(DeathLink DeathlinkObject)
        {
            // deathlink that causes you to die
            // uses the amnesty option

            DeathLinksSentSinceLast++;
            if (DeathLinksSentSinceLast >= Plugin.ClientOptions.deathlink_amnesty)
            {
                deathlinkbusy = true;
                ArchipelagoClient.DeathMessage = DeathlinkObject.Cause;
                Plugin.Logger.LogInfo($"Killing player because of deathlink {DeathlinkObject.Source}, {DeathlinkObject.Cause}");
                ENT_Player.playerObject.Kill("deathlink");
            }
            else
            {
                CL_ProgressionManager.ShowUnlockPopup(APItems.SpriteFromPath("WKRando/Assets/Archipelago_Icon.png"),
                    "Deathlink Stack Received Due To",
                    DeathlinkObject.Cause,
                    new Color(0.4f, 0.1f, 0.1f),
                    addToSession: false);
            }
        }
    }
}
