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
}



