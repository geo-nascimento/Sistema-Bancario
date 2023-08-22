using AppBank.Models;
using AppBank.Repositories;
using AppBank.Controllers;

UserController usrControl = new UserController();


var users = usrControl.ListUsers();


foreach (var user in users)
{
    Console.WriteLine("Id: " + user.UserId);
    Console.WriteLine("Nome: " + user.Name);
    Console.WriteLine("Email: " + user.Email);
    Console.WriteLine("CPF: " + user.CPF);
    Console.WriteLine("Telefone: " + user.Contact!.Telephone);
    Console.WriteLine("Celular: " + user.Contact.CellPhone);
    Console.WriteLine();
}

Console.Write("Qual usuário deseja atualizar? Informe o Id:");
int id = int.Parse(Console.ReadLine()!);

Console.Clear();

var userToUpdate = usrControl.GetUser(id);

Console.WriteLine("Usuario para atualizar");
Console.WriteLine("Nome: " + userToUpdate.Name);
Console.WriteLine("Email: "+ userToUpdate.Email);
Console.WriteLine("CPF: " + userToUpdate.CPF);
Console.WriteLine("Telefone: " + userToUpdate.Contact!.Telephone);
Console.WriteLine("Celular: " + userToUpdate.Contact.CellPhone);


Console.WriteLine();
Console.WriteLine("Atualizar dados");
Console.Write("Nome: ");
string name = Console.ReadLine()!;
Console.Write("Email: ");
string email = Console.ReadLine()!;
Console.Write("CPF: ");
string cpf = Console.ReadLine()!;
Console.Write("Telefone: ");
string phone = Console.ReadLine()!;
Console.Write("Celular: ");
string cellPhone = Console.ReadLine()!;
Console.WriteLine();
usrControl.UpdateRegister(id, name, email, cpf, phone, cellPhone);

var userAtual = usrControl.GetUser(id);
Console.WriteLine("Dados atualizados");
Console.Write("Nome: " + userToUpdate.Name);
Console.Write("Email: " + userToUpdate.Email);
Console.Write("CPF: " + userToUpdate.CPF);
Console.Write("Telefone: " + userToUpdate.Contact!.Telephone);
Console.Write("Celular: " + userToUpdate.Contact.CellPhone);


//usrControl.RegistrationUser("Douglas Souza", "mana.douglas@gmail.com", "081.024.420-04", "(11) 99931-3409","(11) 98831-3855");



