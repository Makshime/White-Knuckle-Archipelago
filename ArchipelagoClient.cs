using System;
using System.Threading;
using Archipelago.MultiClient.Net;
using Archipelago.MultiClient.Net.Enums;
using System.Threading.Tasks;
    
namespace WKRando;

public class ArchipelagoClient
{
    private static ArchipelagoSession _session = ArchipelagoSessionFactory.CreateSession("localhost", 38281);
    private static string _servername = "localhost:38281";
    private static string _username = string.Empty;
    private static string _password = string.Empty;
    

    private static void NewSession(string host, int port)
    {
        _session = ArchipelagoSessionFactory.CreateSession(host, port);
        _servername = $"{host}:{port}";
    }
    private static void NewSession(string server)
    {
        _session = ArchipelagoSessionFactory.CreateSession(server);
        _servername = server;
    }
    
    //Standard connection procedure for Archipelago.MultiClient.Net
    public static void Connect(string server = null, string user = null, string pass = null)
    {
        Plugin.Logger.LogInfo("Connecting to " + server);

        if (server != null && server != _servername)
        {
            NewSession(server);
        }
        if (user != null && user != _username) 
            _username = user;
        if (pass != null && pass != _password) 
            _password = pass;
        
        
        LoginResult result;

        try
        {
            result = _session.TryConnectAndLogin("APQuest", user, ItemsHandlingFlags.AllItems);
        }
        catch (Exception e)
        {
            result = new LoginFailure(e.GetBaseException().Message);
        }

        if (!result.Successful)
        {
            LoginFailure failure = (LoginFailure)result;
            CommandConsole.Log($"Failed to Connect to {_servername} as {user}:");
            foreach (string error in failure.Errors)
            {
                CommandConsole.Log($"    {error}");
            }

            foreach (ConnectionRefusedError error in failure.ErrorCodes)
            {
                CommandConsole.Log($"    {error}");
            }
            
            

            return;
        }
        
        var loginSuccess = (LoginSuccessful)result;

        CommandConsole.Log($"Successfully connected to {_servername} as {user}!");
        CommandConsole.Log($"   Slot Number: {loginSuccess.Slot}");


    }

    //Main update loop for checking for checks
    
    
    public void OnItemReceieve(object se)
    {
        
    }
    
    

}