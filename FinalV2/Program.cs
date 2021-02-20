using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ISDSFinalProject
{
    class Program
    {
        static void Main()
        {

            #region Variables
            // Reading Variables


            string inputRow;
            int rowCount, colCount;
            const char DELIM = ',';

            string[,] users;

            string[] rowData;

            //Sign In Variables

            int userRow;

            string fileFN;
            string fileLN;
            string fileEmail;

            bool userMatch;
            

            string fileUser, filePW;

            string userNameInput;
            string passwordInput;

            string userName;
            string password;
            string memberAnswer;

            string customerFN;
            string customerLN;
            string customerEmail;

            //Vending Variables
            double totalPrice = 0;
            int numberLabel = 1;
            int priceCounter = 0;

            int userChoice;
            bool loopQuestion = false;
            string loopAnswer;

            int userQuantity;

            //Ending Var
            bool programEnd = false;

            var currentUser = "";
            var currentName = "";
           
            var currentEM = "";

            string timeStamp = DateTime.Now.ToString().Replace("/", "").Replace(":", "").Replace(" ", "");

            //Lists. 
            var productNames = new List<string>();
            var productPrices = new List<double>();

            var productsPurchased = new List<string>();
            var quantityPurchased = new List<int>();
            var totalPricePerItem = new List<double>();
            var timeStampsPerItem = new List<DateTime>();
            #endregion

            #region R from Membership and Info Files
            string productsFile = "products.txt";

            using (StreamReader sr = File.OpenText(productsFile))
            {
                string product = " ";

                while ((product = sr.ReadLine()) != null)
                {
                    productNames.Add(product);
                }
            }
            string pricesFile = "prices.txt";

            using (StreamReader sr = File.OpenText(pricesFile))
            {
                string price = " ";

                while ((price = sr.ReadLine()) != null)
                {
                    productPrices.Add(Double.Parse(price));
                }

            }

            const string membershipFile = "membership.txt";

            FileStream inDataFile =
               new FileStream(membershipFile, FileMode.Open, FileAccess.Read);

            StreamReader dataReader = new StreamReader(inDataFile);

            inputRow = dataReader.ReadLine();

            rowCount = 0;
            colCount = 0;

            while (inputRow != null)
            {

                if (rowCount == 0)
                {
                    rowData = inputRow.Split(DELIM);
                    colCount = rowData.Length;
                }
                rowCount++;

                inputRow = dataReader.ReadLine();
            }

            // set the array to the correct size
            users = new string[rowCount, colCount];

            // reset the cursor to the beginning of the file.
            inDataFile.Seek(0, SeekOrigin.Begin);

            // read the file data populate the 2D array 
            inputRow = dataReader.ReadLine();

            rowCount = 0;

            while (inputRow != null)
            {
                // split each row read from file, into a single one-dimentional array (rowData)

                rowData = inputRow.Split(DELIM);

                for (int j = 0; j < rowData.Length; ++j)
                {
                    users[rowCount, j] = rowData[j];
                }

                rowCount++;

                inputRow = dataReader.ReadLine();
            }

            dataReader.Close();
            inDataFile.Close();
            #endregion

            #region LOGIN LOGIC

            Console.WriteLine("\n{0, 27}", "The V.E.N.D Machine!");
            Console.WriteLine("\nAre you a member? --- Y/Yes or N/No");
            memberAnswer = Console.ReadLine().ToUpper();
            if (memberAnswer == "Y" || memberAnswer == "YES")
            {
                userMatch = false;
                
                filePW = "";
                fileUser = "";
                rowCount = 0;

                fileFN = "";
                fileLN = "";
                fileEmail = "";

                Console.WriteLine("Please enter username");
                userNameInput = Console.ReadLine();

                while (!userMatch && rowCount < users.GetLength(0))
                {
                    if (userNameInput.ToUpper() == users[rowCount, 0].ToUpper())
                    {
                        userMatch = true;
                        fileUser = users[rowCount, 0];
                        filePW = users[rowCount, 1];
                        currentUser = users[rowCount, 0];
                        customerFN = users[rowCount, 2] + users[rowCount, 3];
                        currentEM = users[rowCount, 4];
                    }
                    rowCount++;

                }
                userRow = rowCount;

                if (!userMatch)
                {
                    Console.WriteLine("Username '{0}' does not exist", userNameInput);
                }

                if (userMatch)
                {
                    Console.WriteLine("Please enter password");
                    passwordInput = Console.ReadLine();

                    while (passwordInput != filePW)
                    {
                        Console.WriteLine("ERROR: Incorrect Password, try again. \"Q\" to QUIT.");
                        passwordInput = Console.ReadLine();
                        if (passwordInput.ToUpper() == "Q")
                        {
                            break;
                        }
                    }
                    // if password matches, don't ask anymore.
                    if (passwordInput == filePW)
                    {
                        
                        fileUser = fileUser;
                        customerFN = fileFN;
                        customerLN = fileLN;
                        customerEmail = fileEmail;
                        loopQuestion = true;
                    }
                }


            }
            //If no then help become a member. 
            else if (memberAnswer == "N" || memberAnswer == "NO")
            {
                Console.Write("Enter a username: ");
                currentUser = Console.ReadLine();
                Console.Write("\nEnter a password: ");
                password = Console.ReadLine();
                Console.Write("\nEnter your first name: ");
                string userFN = Console.ReadLine();
                Console.Write("\nEnter your last name: ");
                string userLN = Console.ReadLine();
                Console.Write("\nEnter your email: ");
                currentEM = Console.ReadLine();

                currentName = userFN + " " + userLN;

                string dataLine = String.Format("{0},{1},{2},{3},{4}",
                   currentUser, password, userFN, userLN, currentEM);


                using (StreamWriter newUser = File.AppendText(membershipFile))
                {
                    newUser.WriteLine(dataLine);
                }

                loopQuestion = true;
            }

            #endregion

            #region VENDING LOGIC
            while (loopQuestion == true)
            {
                Console.Clear();
                //Resets Counter and Label for the next iteration after Console.Clear()
                priceCounter = 0;
                numberLabel = 1;

                Console.WriteLine("{0, 25}", "The Vending Machine\n");

                for (int i = 0; i < productNames.Count; i++)
                {
                    Console.WriteLine("{0, 2}. {1, -15} -- {2, -15:C2}", numberLabel, productNames[i], productPrices[priceCounter]);
                    priceCounter++;
                    numberLabel++;
                }
                //Ask for user input and assign it to a variable.
                Console.WriteLine("\nWhat item would you like to order? (Enter a number 1-15)");
                userChoice = Int32.Parse(Console.ReadLine());

                switch (userChoice)
                {
                    case 1:
                    case 2:
                    case 3:
                    case 4:
                    case 5:
                    case 6:
                    case 7:
                    case 8:
                    case 9:
                    case 10:
                    case 11:
                    case 12:
                    case 13:
                    case 14:
                    case 15:
                        //Adds to purchased and increments price and quantity. 
                        productsPurchased.Add(productNames[userChoice - 1]);
                        Console.WriteLine("How many {0} would you like?", productNames[userChoice - 1]);
                        userQuantity = Int32.Parse(Console.ReadLine());
                        quantityPurchased.Add(userQuantity);
                        totalPricePerItem.Add(productPrices[userChoice - 1] * userQuantity);
                        timeStampsPerItem.Add(DateTime.Now);
                        break;
                    default:
                        Console.WriteLine("Please enter a valid item number.");
                        break;
                }

                Console.WriteLine("Would you like to order another item? -- Y/Yes or N/No");
                loopAnswer = Console.ReadLine().ToUpper();
                if (loopAnswer == "Y")
                {
                    loopQuestion = true;
                }
                else if (loopAnswer == "N")
                {
                    loopQuestion = false;
                }
                programEnd = true;
            }

            #endregion

            #region END LOGIC

            if (programEnd == true)
            {

                //After Program Finishes it should read and write all data to the files. 

                //Clear console and show ending screen. Mention receipts will be printed. 
                Console.Clear();
                Console.WriteLine("{0, 5} {1, 32}\n\n", " ", "Thanks for shopping with us!");

                Console.WriteLine("{0, 5} {1, 20}", " ", "*********************************");
                Console.WriteLine("\n{0, 20} {1}", "Date:", DateTime.Now);
                Console.WriteLine("{0, 20} {1}", "Username:", currentUser);
                Console.WriteLine("{0, 20} {1, 15}", "Name:", currentName);
                Console.WriteLine("{0, 20} {1}", "Email:", currentEM);

                foreach (double pricePerItem in totalPricePerItem)
                {
                    totalPrice += pricePerItem;
                }
                for (int j = 0; j < productsPurchased.Count; j++)
                {

                    Console.WriteLine("{0, -2} {1, -15} {2, -8:C2} {3, -15}", quantityPurchased[j], productsPurchased[j], totalPricePerItem[j], timeStampsPerItem[j]);
                }

                Console.WriteLine("\nTOTAL PRICE: {0, 12:C2}", totalPrice);

                Console.WriteLine("\n{0, 5} {1, 20}", " ", "*********************************");
                #endregion

                #region Receipt / Log R&W
                //Write Info to daily log and receipt

                using (StreamWriter receiptText = new StreamWriter("Reciept_" + currentUser + "_" + timeStamp + ".txt"))
                {
                    receiptText.WriteLine("***********");
                    receiptText.WriteLine("Receipt: {0}", DateTime.Now);
                    receiptText.WriteLine("User: {0}", currentUser);
                    receiptText.WriteLine("Name: {0, 15}", currentName);
                    receiptText.WriteLine("Email: {0}", currentEM);
                    for (int k = 0; k < productsPurchased.Count; k++)
                    {
                        receiptText.WriteLine("{0, -2} {1, -15} {2, -8:C2} {3, -15}", quantityPurchased[k], productsPurchased[k], totalPricePerItem[k], timeStampsPerItem[k]);

                    }
                    receiptText.WriteLine("\nTotal Price: {0, 12:C2}", totalPrice);
                    receiptText.WriteLine("***********");

                }

                string dailyLog = "dailylog.txt";

                using (StreamWriter dailyLogText = File.AppendText(dailyLog))
                {
                    dailyLogText.WriteLine("***********");
                    dailyLogText.WriteLine("Receipt: {0}", DateTime.Now);
                    dailyLogText.WriteLine("User: {0}", currentUser);
                    dailyLogText.WriteLine("Name: {0, 15}", currentName);
                    dailyLogText.WriteLine("Email: {0}", currentEM);
                    for (int l = 0; l < productsPurchased.Count; l++)
                    {
                        dailyLogText.WriteLine("{0, -2} {1, -15} {2, -8:C2} {3, -15}", quantityPurchased[l], productsPurchased[l], totalPricePerItem[l], timeStampsPerItem[l]);
                    }
                    dailyLogText.WriteLine("Total Price: {0, 12:C2}", totalPrice);
                    dailyLogText.WriteLine("***********");
                }



            }
            #endregion
        }

    }
    
}


