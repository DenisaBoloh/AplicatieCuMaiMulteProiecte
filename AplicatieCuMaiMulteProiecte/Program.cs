using System;
using AdministrareFisier;

class Program
{
    static void Main(string[] args)
    {
        var adminClienti = new AdministrareClienti();
        var adminTaskuri = new AdministrareTaskuri(adminClienti);

        while (true)
        {
            Console.WriteLine("\nMeniu Principal:");
            Console.WriteLine("1. Adauga persoana");
            Console.WriteLine("2. Adauga task");
            Console.WriteLine("3. Marcheaza task ca finalizat");
            Console.WriteLine("4. Afiseaza taskurile unei persoane");
            Console.WriteLine("5. Iesire");
            Console.Write("Alegeti o optiune: ");

            var optiune = Console.ReadLine();

            switch (optiune)
            {
                case "1":
                    Console.Write("Introduceti numele persoanei: ");
                    var numePersoana = Console.ReadLine();
                    adminClienti.AdaugaPersoana(numePersoana);
                    break;

                case "2":
                    Console.Write("Introduceti numele persoanei: ");
                    var numePentruTask = Console.ReadLine();
                    Console.Write("Introduceti descrierea taskului: ");
                    var descriereTask = Console.ReadLine();
                    adminTaskuri.AdaugaTask(numePentruTask, descriereTask);
                    break;

                case "3":
                    Console.Write("Introduceti numele persoanei: ");
                    var numePentruFinalizare = Console.ReadLine();
                    Console.Write("Introduceti descrierea taskului de marcat: ");
                    var taskDeFinalizat = Console.ReadLine();
                    adminTaskuri.MarcheazaTaskCaFinalizat(numePentruFinalizare, taskDeFinalizat);
                    break;

                case "4":
                    Console.Write("Introduceti numele persoanei: ");
                    var numePentruAfisare = Console.ReadLine();
                    adminTaskuri.AfiseazaTaskuri(numePentruAfisare);
                    break;

                case "5":
                    return;

                default:
                    Console.WriteLine("Optiune invalida!");
                    break;
            }
        }
    }
}