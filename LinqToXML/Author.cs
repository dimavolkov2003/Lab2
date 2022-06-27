using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinqToXML
{
    public class Author
    {
        public string Name { get; private set; }
        public string Surname { get; private set; }
        public int YearOfBirth { get; private set; }
        public Author(string name, string surname, int yearOfBirth)
        {
            Name = name;
            Surname = surname;
            YearOfBirth = yearOfBirth;

        }
    }
}
