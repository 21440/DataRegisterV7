using System;
using System.Collections.Generic;

public interface IArraySet
{

    bool Contains(Person p);

    bool Add(Person p);

    bool Remove(Person p);

    bool Edit(Person pOld, Person pNew);

    Person[] toSorted();

}

public class ArraySet : IArraySet
{

    public Person[][] ArrSet { get; set; }

    public int Buckets { get; }

    public ArraySet(List<Person> people, int buckets)
    {

        Buckets = buckets;

        ArrSet = new Person[Buckets][];

        for (int i = 0; i < Buckets; i++) ArrSet[i] = new Person[2];

        ToArray(people);

    }

    private bool Room(Person p)
    {

        int buckets = Math.Abs(p.GetHashCode()) % Buckets;

        foreach (var item in ArrSet[buckets])
        {

            if (item == null) return true;

        }

        return false;

    }

    private void Prolong(Person p)
    {

        int buckets = Math.Abs(p.GetHashCode()) % Buckets;

        int max = Convert.ToInt32(ArrSet[buckets].Length * 1.5);

        ArrSet[buckets] = new Person[max];

    }

    private void ToArray(List<Person> people)
    {

        foreach (var p in people)
        {

            if (Room(p))
            {

                if (!Contains(p))
                {

                    int buckets = Math.Abs(p.GetHashCode()) % Buckets;

                    for (int i = 0; i < ArrSet[buckets].Length; i++)
                    {

                        if (ArrSet[buckets][i] == null)
                        {

                            ArrSet[buckets][i] = p;
                            break;

                        }

                    }

                }

            }
            
            else
            {

                if (!Contains(p))
                {

                    int buckets = Math.Abs(p.GetHashCode()) % Buckets;

                    Person[] memory = ArrSet[buckets];

                    Prolong(p);

                    for (int i = 0; i < memory.Length; i++) ArrSet[buckets][i] = memory[i];

                    for (int i = 0; i < ArrSet[buckets].Length; i++)
                    {

                        if (ArrSet[buckets][i] == null)
                        {

                            ArrSet[buckets][i] = p;
                            break;

                        }

                    }

                }

            }

        }

    }

    private bool RoomSort(Person[] array)
    {

        foreach (var item in array) if (item == null) return true;

        return false;

    }

    public Person[] toSorted()
    {

        Person[] order = new Person[1];

        foreach (var bucket in ArrSet)
        {

            foreach (var person in bucket)
            {

                if (RoomSort(order))
                {

                    if (person != null)
                    {

                        for (int i = 0; i < order.Length; i++)
                        {

                            if (order[i] == null)
                            {

                                order[i] = person;
                                break;

                            }

                        }

                    }

                }

                else
                {

                    if (person != null)
                    {

                        Person[] memory = order;

                        int max = Convert.ToInt32(order.Length * 1.5);
                        
                        order = new Person[max];

                        for (int i = 0; i < memory.Length; i++) order[i] = memory[i];

                        for (int i = 0; i < order.Length; i++)
                        {

                            if (order[i] == null)
                            {

                                order[i] = person;
                                break;

                            }

                        }

                    }

                }

            }

        }
        
        if (order.Length == 1) return order;

        Array.Sort(order);
        return order;

    }

    public bool Contains(Person p)
    {

        int buckets = Math.Abs(p.GetHashCode()) % Buckets;

        int min = 0, max = ArrSet[buckets].Length - 1;

        while (min <= max)
        {

            int mean = (min + max) / 2;

            if (ArrSet[buckets][mean] != null)
            {

                if (ArrSet[buckets][mean].Equals(p)) return true;

                else if (p.CompareTo(ArrSet[buckets][mean]) == -1) max = mean - 1;

                else min = mean + 1;

            }

            else max -= 1;

        }

        return false;

    }

    private int Detect(Person p)
    {
        int buckets = Math.Abs(p.GetHashCode()) % Buckets;

        int min = 0, max = ArrSet[buckets].Length - 1;

        while (min <= max)
        {

            int mean = (min + max) / 2;

            if (ArrSet[buckets][mean] != null)
            {

                if (p.CompareTo(ArrSet[buckets][max]) == 1) return max;

                else if (p.CompareTo(ArrSet[buckets][0]) == -1) return 0;

                else if (p.CompareTo(ArrSet[buckets][mean]) == 1 && p.CompareTo(ArrSet[buckets][++mean]) == -1) return mean++;

                else if (p.CompareTo(ArrSet[buckets][mean]) == 1 && p.CompareTo(ArrSet[buckets][--mean]) == -1) return mean++;

                else if (p.CompareTo(ArrSet[buckets][mean]) == -1) max = mean - 1;

                else min = mean + 1;

            }
            
            else max -= 1;

        }

        return 0;

    }

    public bool Add(Person p)
    {
        if (!Contains(p))
        {

            if (Room(p))
            {

                int buckets = Math.Abs(p.GetHashCode()) % Buckets;

                int pos = Detect(p);

                for (int i = ArrSet[buckets].Length - 1; i > pos; i--) ArrSet[buckets][i] = ArrSet[buckets][i - 1];
                
                ArrSet[buckets][pos] = p;

                return Contains(p);

            }
            
            else
            {

                int buckets = Math.Abs(p.GetHashCode()) % Buckets;

                Person[] memory = ArrSet[buckets];
                
                Prolong(p);

                for (int i = 0; i < memory.Length; i++) ArrSet[buckets][i] = memory[i];

                int pos = Detect(p);

                for (int i = ArrSet[buckets].Length - 1; i > pos; i++) ArrSet[buckets][i] = ArrSet[buckets][i - 1];
                
                ArrSet[buckets][pos] = p;

                return Contains(p);

            }
        }

        return false;
    }

    public bool Edit(Person pOld, Person pNew)
    {

        if (Contains(pOld))
        {

            if (pOld.Equals(pNew)) if (Remove(pOld)) return Add(pNew);
            
            else if (!Contains(pNew)) if (Remove(pOld)) return Add(pNew);

        }

        return false;

    }

    public bool Remove(Person p)
    {

        if (Contains(p))
        {

            int pos = BinarySearch(p);

            int buckets = Math.Abs(p.GetHashCode()) % Buckets;

            ArrSet[buckets][pos] = null;

            for (int i = pos; i < ArrSet[buckets].Length - 1; i++)
            {

                if (ArrSet[buckets][i + 1] != null) ArrSet[buckets][i] = ArrSet[buckets][i + 1];

                else if (ArrSet[buckets][i + 1] == null) ArrSet[buckets][i] = null;

                if (ArrSet[buckets][i] == ArrSet[buckets][i + 1]) ArrSet[buckets][i + 1] = null;

            }

            return (!Contains(p));

        }

        return false;

    }

    public int BinarySearch(Person p)
    {

        int buckets = Math.Abs(p.GetHashCode()) % Buckets;

        int min = 0, max = ArrSet[buckets].Length - 1;

        while (min <= max)
        {

            int mean = (min + max) / 2;

            if (ArrSet[buckets][mean] != null)
            {

                if (ArrSet[buckets][mean].Equals(p)) return mean++;

                else if (p.CompareTo(ArrSet[buckets][mean]) == -1) max = mean - 1;

                else min = mean + 1;

            }
            
            else max -= 1;

        }

        return 0;
    }

}