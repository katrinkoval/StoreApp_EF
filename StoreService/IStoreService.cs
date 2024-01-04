using ModelsDTO;
using System;
using System.Collections.Generic;

namespace StoreService
{
    public interface IStoreService: IDisposable
    {
        void OpenConnection(string servername = null, string login = null, string password = null);

        IEnumerable<Consignment> GetConsignments();
        IEnumerable<Product> GetProducts();
        IEnumerable<Order> GetOrders();
        IEnumerable<Order> GetConsignmentsOrders(int consigmentNumber);       
        IEnumerable<string> GetProductNames();
        IEnumerable<long> GetConsignmentNumbers();
        IEnumerable<long> GetIndividualIDs();
        IEnumerable<string> GetIndividualNames();
        long GetProductID(string name);     
        string GetNameByIPN(int ipn);
        long GetIPNByName(string name);
        int AddOrder(int consNumber, long prodID, double prodAmount);
        int UpdateOrder(int consNum, long prodID, double amount, long prodtIDNew);
        int DeleteOrder(int consNumber, long prodID);
        int ProcedureExecute(bool _isUsingIPN, string commandText, Consignment cons);
        int DeleteConsignment(int consNumber);

        void CloseConnection();


    }
}
