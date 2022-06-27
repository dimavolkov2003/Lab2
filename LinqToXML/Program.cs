using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace LinqToXML
{
    class Program
    {
        static void Main(string[] args)
        {

            Console.OutputEncoding = Encoding.UTF8;

            List<Book> books = new List<Book>
            {
                new Book("Замок", "Франц", "Кафка", 350, new DateTime(2015,7,20,18,00,00)),
                new Book("Отелло", "Вільям", "Шекспір", 200, new DateTime(2016,6,24,2,30,00)),
                new Book("Король Лір", "Вільям", "Шекспір", 600, new DateTime(2016,1,2,22,15,00)),
                new Book("Гамлет", "Вільям", "Шекспір", 400, new DateTime(2016,11,3,9,30,00)),
                new Book("Сон", "Тарас", "Шевченко", 1000, new DateTime(2017,7,31,00,45,00))
            };

            List<Author> authors = new List<Author>
            {
                new Author("Франц","Кафка", 1883),
                new Author("Вільям","Шекспір", 1564),
                new Author("Тарас","Шевченко", 1814),
                new Author("Ліна","Костенко", 1930)
            };

            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            using (XmlWriter writer = XmlWriter.Create("books.xml", settings))
            {
                writer.WriteStartElement("books");
                foreach (var book in books)
                {
                    writer.WriteStartElement("book");
                    writer.WriteAttributeString("title", book.Title);
                    writer.WriteElementString("nameOfAuthor", book.NameOfAuthor);
                    writer.WriteElementString("surnameOfAuthor", book.SurnameOfAuthor);
                    writer.WriteElementString("price", book.Price.ToString());
                    writer.WriteElementString("dateOfPublication", book.DateOfPublication.ToString("yyyy.MM.dd"));
                    writer.WriteEndElement();
                }
                writer.WriteEndElement();
            }
            XDocument xmlDoc = XDocument.Load("books.xml");

            using (XmlWriter writer2 = XmlWriter.Create("authors.xml", settings))
            {
                writer2.WriteStartElement("authors");
                foreach (var author in authors)
                {
                    writer2.WriteStartElement("author");
                    writer2.WriteElementString("name", author.Name);
                    writer2.WriteElementString("surname", author.Surname);
                    writer2.WriteElementString("yearOfBirth", author.YearOfBirth.ToString());
                    writer2.WriteEndElement();
                }
                writer2.WriteEndElement();
            }
            XDocument xmlDoc2 = XDocument.Load("authors.xml");

            Console.WriteLine("task 1");
            task1(xmlDoc);
            Console.WriteLine("task 2");
            task2(xmlDoc);
            Console.WriteLine("task 3");
            task3(xmlDoc, xmlDoc2);
            Console.WriteLine("task 4");
            task4(xmlDoc);
            Console.WriteLine("task 5");
            task5(xmlDoc);
            Console.WriteLine("task 6");
            task6(xmlDoc);
            Console.WriteLine("task 7");
            task7(xmlDoc);
            Console.WriteLine("task 8");
            task8(xmlDoc2);
            Console.WriteLine("task 9");
            task9(xmlDoc);
            Console.WriteLine("task 10");
            task10(xmlDoc);
            Console.WriteLine("task 11");
            task11(xmlDoc);
            Console.WriteLine("task 12");
            task12(xmlDoc);

        }
        static void task1(XDocument xmlDoc)
        {
            var task1 = xmlDoc.Descendants("book")
                .OrderBy(book => book.Element("price").Value);
        }
        static void task2(XDocument xmlDoc)
        {
            var task2 = xmlDoc.Descendants("book")
                .GroupBy(book => book.Element("nameOfAuthor").Value)
                .Select(g => new { Key = g.Key, Count = g.Count() });
        }
        static void task3(XDocument xmlDoc, XDocument xmlDoc2)
        {
            var task3 =
                from book in xmlDoc.Element("books").Elements("book")
                join author in xmlDoc2.Element("authors").Elements("author")
                    on book.Element("nameOfAuthor").Value equals
                    author.Element("name").Value
                group new
                {
                    Title = book.Attribute("title").Value,
                    YearOfBirth = author.Element("yearOfBirth").Value
                } by author.Element("surname").Value;
        }
        static void task4(XDocument xmlDoc)
        {
            var task4 = from book in xmlDoc.Root.Elements("book")
                        where Int32.Parse(book.Element("price").Value) < 400
                        select new
                        {
                            Title = book.Attribute("name").Value,
                            Price = book.Element("price").Value
                        };
        }
        static void task5(XDocument xmlDoc)
        {
            var task5 = xmlDoc.Descendants("book")
                .Where(p => Int32.Parse(p.Element("price").Value) < 1000
                    && Int32.Parse(p.Element("price").Value) > 200)
                .Average(value => (int)value.Element("price"));

            Console.WriteLine("Average price : {0}\n", task5);
        }
        static void task6(XDocument xmlDoc)
        {
            var task6 = xmlDoc.Descendants("book")
                .OrderBy(book => book.Element("price").Value);

            var task62 = task6.SkipWhile(p => Int32.Parse(p.Element("price").Value) < 200);
        }
        static void task7(XDocument xmlDoc)
        {
            var youngest = xmlDoc.Descendants("book").Min(y => (DateTime)y.Element("dateOfPublication")).Year;
            var oldest = xmlDoc.Descendants("book").Max(y => (DateTime)y.Element("dateOfPublication")).Year;
            var task7 = oldest - youngest;
            Console.WriteLine("difference between the youngest and oldest books: {0}\n", task7);
        }
        static void task8(XDocument xmlDoc2)
        {
            var task8 = xmlDoc2.Descendants("author")
                .OrderByDescending(y => y.Element("yearOfBirth").Value)
                .Select(s => s.Element("surname").Value);
        }
        static void task9(XDocument xmlDoc)
        {
            var task9 = xmlDoc.Descendants("book")
                .Any(y => (DateTime)y.Element("dateOfPublication") < new DateTime(2000, 1, 1));

            if (task9)
            {
                var task = xmlDoc.Root.Elements("book")
                    .Where(y => (DateTime)y.Element("dateOfPublication") < new DateTime(2000, 1, 1))
                    .Select(b => new { Name = b.Attribute("title").Value });
            }
        }
        static void task10(XDocument xmlDoc)
        {
            var task10 = from book in xmlDoc.Root.Elements("book")
                         group new Book
                         (
                             book.Attribute("title").Value,
                             book.Element("nameOfAuthor").Value,
                             book.Element("surnameOfAuthor").Value,
                             int.Parse(book.Element("price").Value),
                             (DateTime)book.Element("dateOfPublication")
                         ) by book.Element("nameOfAuthor").Value;

            foreach (var g in task10)
            {
                Console.WriteLine("id: {0}", g.Key);
            }
            Console.ReadKey();
        }
        static void task11(XDocument xmlDoc)
        {
            XElement root = xmlDoc.Element("books");

            if (root != null)
            {
                root.Add(new XElement("book",
                            new XAttribute("title", "Apple"),
                            new XElement("nameOfAuthor", "Alex"),
                            new XElement("surnameOfAuthor", "Kapinus"),
                            new XElement("price", "280"),
                            new XElement("dateOfPublication", (new DateTime(2017, 7, 31, 00, 45, 00)).ToString("yyyy.MM.dd"))));

                xmlDoc.Save("books.xml");
            }
            Console.WriteLine(xmlDoc);
        }
        static void task12(XDocument xmlDoc)
        {
            var apple = xmlDoc.Root.DescendantsAndSelf("book").Where(t => t.Attribute("title").Value == "Apple");

            if (apple != null)
            {
                apple.Remove();
                xmlDoc.Save("books.xml");
            }
            Console.WriteLine(xmlDoc);
        }
    }
}
