using AppBank.Controllers;
using System.Globalization;
using AppBank.Services;
using AppBank.Models.Enums;
using AppBank.Models;
using AppBank.ViewModels;

UserController _user = new UserController();
AccountController _account = new AccountController();

Menu();


void Menu()
{
    Console.WriteLine("__Welcome to the Banking System__");
    Console.WriteLine
        (
            "Which operation do you want to perform: \n"
            + "1. Access user data\n"
            + "2. Register a new user\n"
            + "3. Close the application"
        );
    Console.Write("Option: ");
    try
    {
        int option = int.Parse(Console.ReadLine()!);
        Console.Clear();
        Operation(option);
    }
    catch (Exception)
    {
        Console.WriteLine("Informe uma opção válida. Não são premitidas entrads nulas ou letras");
        Console.WriteLine("Voltando para o menu principal");
        Console.ReadKey();
        Console.Clear();
        Menu();
    }

}

void Operation(int option)
{
    switch (option)
    {
        case 1:
            Console.WriteLine("_____LOGIN_____");
            try
            {
                Console.Write("E-mail:");
                string email = Console.ReadLine()!;
                Console.Write("Password: ");
                string password = Console.ReadLine()!;

                int userId = _user.LoginUser(email, password);

                var user = _user.GetUSerInfo(userId);

                Console.Clear();

                Console.WriteLine(UserViewModel.UserData(user));

                //Acesso a conta
                Console.Write("Deseja acessar sua conta? (1. Sim/ 2. Não): ");
                int response = int.Parse(Console.ReadLine()!);
                switch (response)
                {
                    case 1:
                        AccountOperation();
                        break;
                    case 2:
                        Menu();
                        break;
                    default:
                        Console.WriteLine("Opção inválidada");
                        Console.WriteLine("Voltando para o menu principal");
                        Console.ReadKey();
                        Console.Clear();
                        Menu();
                        break;
                } 
            }
            catch (Exception)
            {
                Console.WriteLine("Usuário não encontrado. E-mail e/ou senha estão incorretos");
                Console.WriteLine("Voltando para o menu principal");
                Console.ReadKey();
                Console.Clear();
                Menu();
            }
            break;
    }

}

void AccountOperation()
{
    try
    {
        Console.Write("Account number:");
        string accountNumber = Console.ReadLine()!;
        Console.Write("Password: ");
        string password = Console.ReadLine()!;

        int accountId = _account.LoginAccount(accountNumber, password);

        var account = _account.GetAccountsInfo(accountId);

        Console.WriteLine(AccountViewModel.AccountData(account)); 
    }
    catch(Exception)
    {
        Console.WriteLine("Usuário não encontrado. E-mail e/ou senha estão incorretos");
        Console.WriteLine("Voltando para o menu principal");
        Console.ReadKey();
        Console.Clear();
        Menu();
    }
}


