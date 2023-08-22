using AppBank.Models;
using AppBank.Repositories;

UserRepository _usrDb = new UserRepository();
AccountRepository _accDb = new AccountRepository();

User user = _accDb.GetUserAccounts(3);

Console.WriteLine(user.UserId);
Console.WriteLine(user.Name);
Console.WriteLine(user.Email);
Console.WriteLine(user.CPF);

Console.WriteLine();
foreach (var account in user.Accounts!)
{
    Console.WriteLine("Id da conta: " + account.AccountId);
    Console.WriteLine("Número da conta: " + account.AccountNumber);
    Console.WriteLine("Tipo de conta: " + account.AccountType.ToString());
    Console.WriteLine("Saldo: " + account.Balance);
    Console.WriteLine();

}

Console.WriteLine();

var users = _usrDb.GetUsers();

foreach(var usr in users)
{
    Console.WriteLine("id: " + usr.UserId);
    Console.WriteLine("Nome: " + usr.Name);
    Console.WriteLine("E-mail: " + usr.Email);
    Console.WriteLine("CPF: " + usr.CPF);
    Console.WriteLine("Data de Cadastro: " + usr.RegistrationDate.ToString("dd/MM/yyyy HH:mm zzz"));
    Console.WriteLine();
}

