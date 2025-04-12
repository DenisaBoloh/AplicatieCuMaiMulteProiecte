namespace AdministrareFisier
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using LibrarieModele;

    public class AdministrareClienti
    {
        private List<Persoana> persoane;
        private const string numeFisierPersoane = @"..\..\..\AplicatieCuMaiMulteProiecte\bin\Debug\persoane.txt";

        public AdministrareClienti()
        {
            persoane = new List<Persoana>();
            IncarcaPersoane();
        }

        public void AdaugaPersoana(string numePersoana)
        {
            persoane.Add(new Persoana(numePersoana));
            SalveazaPersoane();
        }

        public Persoana CautaPersoana(string nume)
        {
            return persoane.FirstOrDefault(p => p.Nume.Equals(nume, StringComparison.OrdinalIgnoreCase));
        }

        public List<Persoana> GetPersoane()
        {
            return persoane;
        }

        private void IncarcaPersoane()
        {
            if (File.Exists(numeFisierPersoane))
            {
                try
                {
                    persoane = File.ReadAllLines(numeFisierPersoane)
                                  .Select(nume => new Persoana(nume))
                                  .ToList();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error reading {numeFisierPersoane}: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine($"File {numeFisierPersoane} not found.");
            }
        }


        private void SalveazaPersoane()
        {
            File.WriteAllLines(numeFisierPersoane, persoane.Select(p => p.Nume));
        }
    }
}
