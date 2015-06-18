using System.Collections.Generic;

using DigicoinOMS.Core.Model.Participants;

namespace DigicoinOMS.Core.DataAccess
{
    public interface IClientManager
    {
        Client GetClientByName(string clientName);
        
        Client GetClientById(string clientId);

        Client RegisterClient(string clientName);

        List<Client> GetClients();
    }
}
