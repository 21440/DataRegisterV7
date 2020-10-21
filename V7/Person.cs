using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

public class Person : IComparable<Person>
{

    public static List<Person> People = new List<Person>();

    public string Id { get; set; }

    public string Name { get; }

    public string LastName { get; }

    public string FullName => $"{Name} {LastName}";

    public double Savings;
    
    public string Password { get; }

    private int Data = 0;

    public int Age => Data >> 4;
    
    public Sex Sex => (Sex) ((Data & 15) >> 3);

    public MaritalStatus MaritalStatus => (MaritalStatus) ((Data & 7) >> 2);

    public Grade Grade => (Grade) (Data & 3);


    public Person(in string id, in string name, in string lastname, in double savings, in string pw, in int data)
    {

        Id = id;

        Name = name;

        LastName = lastname;

        Savings = savings;

        Password = pw;

        Data = data;

    }

    internal static Person FromCsvFile(string line)
    {
        
        string[] token = line.Split(',');

        return new Person(token[0], token[1], token[2], double.Parse(token[3]), token[4], int.Parse(token[5]));

    }

    internal static void SaveToCsv(ArraySet arrset)
    {

        Person[] sorted = arrset.toSorted();

        File.Delete(V7.Program.filepath);

        foreach (var being in sorted)
        {

            if (being != null)
            {

                StreamWriter write = File.AppendText(V7.Program.filepath);
                
                write.WriteLine($"{being.Id},{being.Name},{being.LastName},{being.Savings},{being.Password},{being.Data}");

                write.Close();
                
            }

        }

        Console.WriteLine("The Record's File has been updated.");

    }

    public int CompareTo(Person other)
    {

        return this.Id.CompareTo(other.Id);

    }

    public override bool Equals(object obj)
    {

        if (obj is Person other)
        {

            return Id.Equals(other.Id);

        }

        return false;

    }

    public override string ToString()
    {

        return $"ID: {Id}; FullName: {FullName}; Age: {Age}; Sex: {Sex}; MaritalStatus: {MaritalStatus}; Grade: {Grade}; Savings: {Savings}; Password: {Password}";
    
    }

    public override int GetHashCode()
    {

        return Id[0].GetHashCode();

    }

}

public enum Sex
{

    Female = 0,

    Male = 1

}

public enum MaritalStatus
{

    Single = 0,

    Married = 1

}

public enum Grade
{

    Initial = 0,

    Medium = 1,

    Grade = 2,

    Postgrade = 3

}