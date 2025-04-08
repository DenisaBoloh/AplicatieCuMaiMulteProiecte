using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class AdministrareClienti
{
    private List<Persoana> persoane;
    private const string numeFisierPersoane = "persoane.txt";

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
            persoane = File.ReadAllLines(numeFisierPersoane)
                          .Select(nume => new Persoana(nume))
                          .ToList();
        }
    }

    private void SalveazaPersoane()
    {
        File.WriteAllLines(numeFisierPersoane, persoane.Select(p => p.Nume));
    }
}