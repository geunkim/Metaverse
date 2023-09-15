using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using Microsoft.Extensions.Options;

using Hyperledger.Aries.Agents;
using Hyperledger.Aries.Configuration;
using Hyperledger.Aries.Features.DidExchange;
using Hyperledger.Aries.Storage;
using Hyperledger.Aries.Utils;
using Hyperledger.Aries;
using Hyperledger.Aries.Ledger;
using Hyperledger.Aries.Runtime;
using Hyperledger.Aries.Contracts;

using Hyperledger.Indy.WalletApi;
using Hyperledger.Indy.PoolApi;
using Hyperledger.Indy.DidApi;

public class ConnectionTest
{
    public async Task TestConnection()
    {
        string config = "{\"id\":\"myWallet\"}";
        string credentials = "{\"key\":\"myWalletKey\"}";
        await Wallet.CreateWalletAsync(config, credentials);
        Wallet wallet = await Wallet.OpenWalletAsync(config, credentials);
        CreateAndStoreMyDidResult didResult = await Did.CreateAndStoreMyDidAsync(wallet, "{\"seed\":\"issuer00000000000000000000000000\"}");
        //await Did.SetEndpointForDidAsync(wallet, didResult.Did, "{\"endpoint\":{\"endpoint\":\"http://220.68.5.139:8000\"}}", didResult.VerKey);
        Console.WriteLine("did : " + didResult.Did);
        await wallet.CloseAsync();


        IPoolService poolService = new DefaultPoolService();
        Console.WriteLine("path : " + Path.GetFullPath("genesis.txn"));
        await poolService.CreatePoolAsync("test_pool", Path.GetFullPath("genesis.txn"));
        Pool pool = await poolService.GetPoolAsync("test_pool");

        IAgentContext agentContext = new DefaultAgentContext
        {
            Wallet = await Wallet.OpenWalletAsync(config, credentials),
            Pool = PoolAwaitable.FromPool(pool),
            SupportedMessages = new List<MessageType>
            {
                new MessageType(MessageTypes.ConnectionInvitation),
                new MessageType(MessageTypes.ConnectionRequest),
                new MessageType(MessageTypes.ConnectionResponse),
                new MessageType(MessageTypes.BasicMessageType),
            }
        };

        //Console.WriteLine("agent : " + agentContext.Agent.ToString());

        //DefaultAgentProvider defaultAgentProvider = new();
        //DefaultAgent agent = new DefaultAgent();
        
        DefaultWalletRecordService defaultWalletRecordService = new();

        IOptionsFactory<AgentOptions> _options = new OptionsFactory<AgentOptions>
        (new List<IConfigureOptions<AgentOptions>>(), new List<IPostConfigureOptions<AgentOptions>>());

        Microsoft.Extensions.Options.IOptions<AgentOptions> option = 
        new Microsoft.Extensions.Options.OptionsManager<AgentOptions>(_options);
        Microsoft.Extensions.Logging.ILogger<DefaultConnectionService> logger = new Microsoft.Extensions.Logging.Abstractions.NullLogger<DefaultConnectionService>();

        DefaultWalletService defaultWalletService = new();

        // Create a new connection invitation
        IProvisioningService provisioningService = new DefaultProvisioningService(defaultWalletRecordService, 
        defaultWalletService, option);

        IEventAggregator eventAggregator = new EventAggregator();
        IConnectionService connectionService = new DefaultConnectionService(eventAggregator, 
        new DefaultWalletRecordService(), provisioningService, logger);

        try
        {
            Console.WriteLine("Connection Invitation:");
            var connectionId = Guid.NewGuid().ToString();
            var (msg, rec) = connectionService.CreateInvitationAsync(agentContext, 
            new InviteConfiguration{ ConnectionId = connectionId }).Result;

            Console.WriteLine("msg :" + msg.ToString());
            Console.WriteLine("rec :" + rec.ToString());
        }
        catch (Exception e)
        {
            Console.WriteLine("Exception : " + e.ToString());
        }
        finally
        {
            await Pool.DeletePoolLedgerConfigAsync("test_pool");
            await Wallet.DeleteWalletAsync(config, credentials);
        }


        /*
        // Create a new connection invitation
        var connectionService = agentContext.GetService<IConnectionService>();
        var connectionInvitation = await connectionService.CreateInvitationAsync(agentContext);

        // Print the invitation URL
        Console.WriteLine("Connection Invitation:");
        Console.WriteLine(connectionInvitation.Invitation.ToJson());

        // Wait for connection request
        var connectionRecord = await WaitForConnectionRequest(agentContext);

        // Accept the connection request
        await connectionService.AcceptInvitationAsync(agentContext, connectionRecord.Id);

        // Connection established
        Console.WriteLine("Connection established!");
        */
    }

    /*
    private async Task<IAgentContext> CreateAgentContext()
    {
        /*
        await Wallet.CreateWalletAsync("myWallet", "myWalletKey");

        var defaultAgentContext = new DefaultAgentContext
        {
            Wallet = await Wallet.OpenWalletAsync("myWallet", "myWalletKey"),
            Pool = new PoolAwaitable()
        };

        return defaultAgentContext;

        
        // Create a new agent configuration
        var agentOptions = new AgentOptions
        {
            WalletConfiguration = new WalletConfiguration { Id = "myWallet" },
            WalletCredentials = new WalletCredentials { Key = "myWalletKey" }
        };

        var defaultWallet = new DefaultWalletService();

        var defaultAgentContextProvider = new DefaultAgentContext();
        IServiceProvider serviceProvider = new DefaultServiceProvider();
        var defaultAgent = new DefaultAgent();

        var defaultWalletService = defaultAgent.GetService<IWalletService>();


        var agentProvider = new DefaultAgentProvider(agentOptions, );

        // Create and initialize the agent
        var agent = new DefaultAgent();
        await agent.InitializeAsync(agentOptions);

        return agent.GetContext();
        
    }

    private async Task<ConnectionRecord> WaitForConnectionRequest(IAgentContext agentContext)
    {
        /*
        var connectionService = agentContext.GetService<IConnectionService>();
        var connectionListener = agentContext.GetRequiredService<IConnectionListener>();

        while (true)
        {
            // Wait for incoming connection requests
            var connectionRequest = await connectionListener.GetNextRequestAsync();

            // Process the connection request
            var connectionRecord = await connectionService.ProcessRequestAsync(agentContext, connectionRequest);
            if (connectionRecord.State == ConnectionState.Responded)
            {
                // Connection request accepted
                return connectionRecord;
            }
        }

        
    }
    */
}

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Hello World!");

        ConnectionTest connectionTest = new ConnectionTest();
        connectionTest.TestConnection().Wait();

        Console.WriteLine("Program End!");
    }
}
