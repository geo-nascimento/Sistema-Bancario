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
    int option = int.Parse(Console.ReadLine()!);
    Console.Clear();
    Operations(option);
}

void Operations(int option)
{
    switch (option)
    {
        case 1://fazer a autenticação do usuário coletando os dados de email e senha
            Console.WriteLine("__Login__");
            try
            {
                Console.Write("Email: ");
                string email = Console.ReadLine()!;
                Console.Write("Password: ");
                string password = Console.ReadLine()!;
                
                Console.Clear() ;

                int userId = _account.UserLogin(email, password);

                var user = _account.GetAccountsInfo(userId);

                Console.WriteLine(UserViewModel.UserData(user));
                Console.WriteLine();
                Console.WriteLine("Do you want to access an account? (1. Yes/2. No/3. Return to main menu)");
                int response = int.Parse(Console.ReadLine()!);
                
                OperationAccount(response);
            }
            catch (ArgumentNullException)
            {
                Console.WriteLine("Entrada de dados inválida");
                Console.ReadKey();
                Console.Clear();
                Menu();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                Console.WriteLine("Voltando para o menu inicial");
                Console.ReadKey();
                Console.Clear();
                Menu();
            }
            break;
        case 2:
            Console.WriteLine("__Register a new user__");

            break;
        case 3:
            Console.WriteLine("Closing the application");
            Console.ReadKey();
            Environment.Exit(0);
            break;
        default:
            Console.WriteLine("Invalid Operation");
            Console.Clear();
            Menu();
            break;
    }
}

void OperationAccount(int response)
{
    switch (response)
    {
        case 1:

            break;
        case 2:

            break; 
        
        case 3:

            break;

        default:

            break;
    }
}



