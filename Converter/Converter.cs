using BE;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Converter
{
    class Converter
    {
        string path = @"C:\Users\KristofHorvath\Desktop\Folder";

        public void Convert()
        {
            ServiceGateway sg = new ServiceGateway();
            sg.postMany(ReadFiles());
        }

        private string[]  GetFiles()
        {
            string[] files = System.IO.Directory.GetFiles(path, "*.txt");

            return files;
        }

        private List<Customer> ReadFiles()
        {
            List<Customer> lst = new List<Customer>();
            string[] files = GetFiles();
            string line;
            for (int i = 0; i < files.Length; i++)
            {
                int counter = 0;
                Customer c = new Customer();
                StreamReader file = new StreamReader(files[i]);
                while ((line = file.ReadLine()) != null)
                {
                    string l = line.Substring(11);
                    if (counter == 0)
                    {
                        c.Name = l;
                    }
                    if (counter == 1)
                    {
                        c.BirthDate = DateTime.ParseExact(l, "d", null);
                    }
                    if (counter == 2)
                    {
                        c.Address = l;
                    }
                    if (counter == 3)
                    {
                        c.PhoneNumber = Int32.Parse(l);
                    }
                    counter++;
                }
                lst.Add(c);
            }
            return lst;
        }
    }
}
