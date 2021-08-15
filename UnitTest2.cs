using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
namespace TestProject02
{
    [TestClass]
    public class UnitTest2
    {
        const string _phoneBook = "/+1-541-754-3010 156 Alphand_St. <J Steeve>\n 133, Green, Rd. <E Kustur> NY-56423 ;+1-541-914-3010\n"
                    + "+1-541-984-3012 <P Reed> /PO Box 530; Pollocksville, NC-28573\n :+1-321-512-2222 <Paul Dive> Sequoia Alley PQ-67209\n"
                    + "+1-741-984-3090 <Peter Reedgrave> _Chicago\n :+1-921-333-2222 <Anna Stevens> Haramburu_Street AA-67209\n"
                    + "+1-111-544-8973 <Peter Pan> LA\n +1-921-512-2222 <Wilfrid Stevens> Wild Street AA-67209\n"
                    + "<Peter Gone> LA ?+1-121-544-8974 \n <R Steell> Quora Street AB-47209 +1-481-512-2222\n"
                    + "<Arthur Clarke> San Antonio $+1-121-504-8974 TT-45120\n <Ray Chandler> Teliman Pk. !+1-681-512-2222! AB-47209,\n"
                    + "<Sophia Loren> +1-421-674-8974 Bern TP-46017\n <Peter O'Brien> High Street +1-908-512-2222; CC-47209\n"
                    + "<Anastasia> +48-421-674-8974 Via Quirinal Roma\n <P Salinger> Main Street, +1-098-512-2222, Denver\n"
                    + "<C Powel> *+19-421-674-8974 Chateau des Fosses Strasbourg F-68000\n <Bernard Deltheil> +1-498-512-2222; Mount Av.Eldorado\n"
                    + "+1-099-500-8000 <Peter Crush> Labrador Bd.\n +1-931-512-4855 <William Saurin> Bison Street CQ-23071\n"
                    + "<P Salinge> Main Street, +1-098-512-2222, Denve\n";

        [TestMethod]
        public void TestMethod1()
        {
            var x = phone(_phoneBook, "48-421-674-8974");
            var y = "Phone => 48-421-674-8974, Name => Anastasia, Address => Via Quirinal Roma";
            Assert.AreEqual(x, y);

        }
        [TestMethod]
        public void TestMethod2()
        {
            Assert.AreEqual(phone(_phoneBook, "1-921-512-2222"), "Phone => 1-921-512-2222, Name => Wilfrid Stevens, Address => Wild Street AA-67209");
        }
        [TestMethod]
        public void TestMethod3()
        {
            Assert.AreEqual(phone(_phoneBook, "1-908-512-2222"), "Phone => 1-908-512-2222, Name => Peter O'Brien, Address => High Street CC-47209");
        }
        [TestMethod]
        public void TestMethod4()
        {
            Assert.AreEqual(phone(_phoneBook, "1-541-754-3010"), "Phone => 1-541-754-3010, Name => J Steeve, Address => 156 Alphand_St.");
        }
        [TestMethod]
        public void TestMethod5()
        {
            Assert.AreEqual(phone(_phoneBook, "1-121-504-8974"), "Phone => 1-121-504-8974, Name => Arthur Clarke, Address => San Antonio TT-45120");
        }
        [TestMethod]
        public void TestMethod6()
        {
            Assert.AreEqual(phone(_phoneBook, "1-498-512-2222"), "Phone => 1-498-512-2222, Name => Bernard Deltheil, Address => Mount Av.Eldorado");
        }
        [TestMethod]
        public void TestMethod7()
        {
            Assert.AreEqual(phone(_phoneBook, "1-098-512-2222"), "Error => Too many people: 1-098-512-2222");
        }
        [TestMethod]
        public void TestMethod8()
        {
            Assert.AreEqual(phone(_phoneBook, "5-555-555-5555"), "Error => Not found: 5-555-555-5555");
        }
        private string phone(string phoneBook, string phoneNumber)
        {
            string result = "";

            string[] lines = phoneBook.Split('\n');
            // Linq match data
            var matchQuery = from word in lines
                             where word.Contains(phoneNumber)
                             select word;

            //count phone number is matched 
            int phone_match = matchQuery.Count();

            //validate phone number
            if (phone_match > 1)
            {
                result = "Error => Too many people: " + phoneNumber;
            }
            else if (phone_match == 0)
            {
                result = "Error => Not found: " + phoneNumber;
            }
            else
            {
                //split line
               // string[] lines = phoneBook.Split('\n');
                string name, address, cleanStr;
                foreach (string phoneBookLine in lines)
                {

                    //find line of phone 
                    if (phoneBookLine.Contains(phoneNumber))
                    {
                        string line = phoneBookLine;
                        line = line.Replace("+" + phoneNumber, ""); //remove phone number

                        //find name from <...>
                        int idx_name_first = line.IndexOf('<');
                        int idx_name_last = line.IndexOf('>');
                        name = line.Substring(idx_name_first, idx_name_last - idx_name_first + 1);

                        line = line.Replace(name, "");// remove name
                        name = name.Trim('<', '>'); //remove < >

                        //find address
                        List<string> cleanStrList = new List<string>();
                        foreach (string s in line.Split(' ')) //San Antonio $ TT-45120
                        {
                            cleanStr = Regex.Replace(s, "^[^a-zA-Z0-9]", string.Empty); //clean string
                            if(!string.IsNullOrEmpty(cleanStr)) cleanStrList.Add(cleanStr);// add to list

                        }
                        address = string.Join(" ", cleanStrList);

                        result = $"Phone => {phoneNumber}, Name => {name}, Address => {address}";
                        return result;
                    }
                }
            }

            return result;
        }
    }
}