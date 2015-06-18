using System.Collections.Generic;
using System.Linq;

using DigicoinOMS.Core.Model.Participants;

namespace DigicoinOMS.Core.DataAccess
{
    public class InMemoryClientManager: IClientManager
    {
        private readonly List<Client> clients = new List<Client>();

        private int clientCounter = 0;

        public Client GetClientByName(string clientName)
        {
            try
            {
                return this.clients.Single(cl => cl.ClientName == clientName);
            }
            catch
            {
                return null;
            }
        }

        public Client GetClientById(string clientId)
        {
            return this.clients.Single(cl => cl.ClientId == clientId);
        }

        public Client RegisterClient(string clientName)
        {
            var client = new Client { ClientId = this.clientCounter++.ToString(), ClientName = clientName };
            this.clients.Add(client);
            return client;
        }

        public List<Client> GetClients()
        {
            return this.clients;
        }
    }
}
