using AdminAuth.Models.AccountModel;
using Newtonsoft.Json;

namespace AdminAuth.Repositories
{
    public class AccountRepository
    {
        public List<Account> accounts;

        public AccountRepository()
        {
            LoadAccountsFromJson();
        }

        //this method loads accounts from the json file
        private void LoadAccountsFromJson()
        {
            string jsonFilePath = "Models/AccountModel/accounts.json"; 
            string jsonData = File.ReadAllText(jsonFilePath);
            accounts = JsonConvert.DeserializeObject<List<Account>>(jsonData);
        }

        //a method of type Account which fetches accounts
        public Account GetAccountByAccountNumber(string accountNumber)
        {
            return accounts.FirstOrDefault(a => a.AccountNumber == accountNumber);
        }

        //a method which updates the account balance after a txn is made
        public void UpdateAccountBalance(string accountNumber, decimal newBalance)
        {
            var accountToUpdate = accounts.FirstOrDefault(a => a.AccountNumber == accountNumber);
            if (accountToUpdate != null)
            {
                accountToUpdate.Balance = newBalance;
                SaveChangesToJson();
            }
        }

        //this method saves the changes back to the json
        private void SaveChangesToJson()
        {
            string jsonFilePath = "Models/AccountModel/accounts.json";
            string jsonData = JsonConvert.SerializeObject(accounts, Formatting.Indented);
            File.WriteAllText(jsonFilePath, jsonData);
        }


    }
}
