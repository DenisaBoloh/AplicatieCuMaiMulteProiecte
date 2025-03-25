using System;

class Program
{
    static void Main(string[] args)
    {
        string numeFisier = "persoane.txt";
        AdministratorClienti adminClienti = new AdministratorClienti(numeFisier);

        bool running = true;
        while (running)
        {
            Console.WriteLine("Meniu:");
            Console.WriteLine("1. Adauga o persoana");
            Console.WriteLine("2. Cauta o persoana");
            Console.WriteLine("3. Adauga un task");
            Console.WriteLine("4. Marcheaza task ca finalizat");
            Console.WriteLine("5. Afiseaza taskuri");
            Console.WriteLine("6. Iesire");
            Console.Write("Alege o optiune: ");
            int optiune = int.Parse(Console.ReadLine());

            switch (optiune)
            {
                case 1:
                    Console.Write("Introduceti numele persoanei: ");
                    string numePersoana = Console.ReadLine();
                    adminClienti.AdaugaPersoana(numePersoana);
                    Console.WriteLine("Persoana a fost adaugata!");
                    break;

                case 2:
                    Console.Write("Introduceti numele persoanei de cautat: ");
                    string numeDeCautat = Console.ReadLine();
                    var persoanaGasita = adminClienti.CautaPersoana(numeDeCautat);
                    if (persoanaGasita != null)
                    {
                        Console.WriteLine($"Persoana gasita: {persoanaGasita.Nume}");
                    }
                    else
                    {
                        Console.WriteLine("Persoana nu a fost gasita.");
                    }
                    break;

                case 3:
                    Console.Write("Introduceti numele persoanei: ");
                    string numeTaskPersoana = Console.ReadLine();
                    Console.Write("Introduceti task-ul: ");
                    string task = Console.ReadLine();
                    adminClienti.AdaugaTask(numeTaskPersoana, task);
                    Console.WriteLine("Task-ul a fost adaugat!");
                    break;

                case 4:
                    Console.Write("Introduceti numele persoanei: ");
                    string numePersoanaFinalizare = Console.ReadLine();
                    Console.Write("Introduceti task-ul de marcat ca finalizat: ");
                    string taskFinalizat = Console.ReadLine();
                    adminClienti.MarcheazaTaskCaFinalizat(numePersoanaFinalizare, taskFinalizat);
                    Console.WriteLine("Task-ul a fost marcat ca finalizat!");
                    break;

                case 5:
                    Console.Write("Introduceti numele persoanei pentru a afisa taskurile: ");
                    string numeAfisare = Console.ReadLine();
                    adminClienti.AfiseazaTaskuri(numeAfisare);
                    break;

                case 6:
                    running = false;
                    break;

                default:
                    Console.WriteLine("Optiune invalida.");
                    break;
            }
        }
    }
}