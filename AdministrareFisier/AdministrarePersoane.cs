using System;
using System.Collections.Generic;

public class AdministrarePersoane
{
    private List<Persoana> persoane;

    public AdministrarePersoane()
    {
        persoane = new List<Persoana>();
    }

    public void AdaugaPersoana(string nume)
    {
        Persoana persoana = new Persoana(nume);
        persoane.Add(persoana);
    }

    public Persoana CautaPersoana(string nume)
    {
        return persoane.Find(p => p.Nume.Equals(nume, StringComparison.OrdinalIgnoreCase));
    }

    public List<Persoana> GetPersoane()
    {
        return persoane;
    }
}
