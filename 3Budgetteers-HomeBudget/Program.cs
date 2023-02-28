using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Dynamic;
using static System.Net.WebRequestMethods;
using System.Collections;

namespace Budget
{
    internal class Program
    {
        const string FORMAT = "| {0,-9}| {1,-20}| {2,-15}| {3,-15}|{4,10} |{5,10} |";
        //id, description, date, category, amount, balance
    
        static void Main(string[] args)
        {
            /*
            
            string budgetFilePath = "../../../tests/test.budget";

            //Expense e = new Expense(1, DateTime.Now, 10, 12, null);
            ////Category cat = new Category(55, "test");
            Categories c = new Categories();

            c.Add("test", Category.CategoryType.Income);
            Console.Write(c.GetCategoryFromId(1));
            //c.Delete(-1);


            //HomeBudget budget = new HomeBudget(budgetFilePath);
            //DateTime? start = null, end = null;
            //bool filter = false;
            //int categoryId = 0;

            //GetUserInput(budget, out start, out end, out filter, out categoryId);

            //DisplayFirstReport(budget, start, end, filter, categoryId);

            //DisplaySecondReport(budget, start, end, filter, categoryId);

            //DisplayThirdReport(budget, start, end, filter, categoryId);

            //DisplayFourthReport(budget, start, end, filter, categoryId);

            //List<Category> cats = budget.categories.List();
            //foreach (Category category in cats)
            //{
            //    Console.WriteLine(category);
            //}

            Console.ReadLine();

        }

        public static void GetUserInput(HomeBudget budget, out DateTime? start, out DateTime? end, out bool filterFlag, out int categoryId)
        {
            string userInput;
            DateTime? startDateTime, endDateTime;
            bool filter;
            int catId;
            
            Console.WriteLine("Would you like to get the budget items within a smaller time window? (y or n)");
            if ((userInput = Console.ReadLine()) == "y") 
            {
                startDateTime = GetDate("start");
                endDateTime = GetDate("end");
                
            }
            else
            {
                startDateTime = null;
                endDateTime= null;
            }

            Console.WriteLine("Would you like to filter the budget items? (y or n)");
            if ((userInput = Console.ReadLine()) == "y")
            {
                filter = true;
            }
            else
            {
                filter = false;
            }

            Console.WriteLine("Please enter a Category id.");
            catId = int.Parse(Console.ReadLine());

            start = startDateTime;
            end = endDateTime;
            filterFlag = filter;
            categoryId = catId;

        }

        public static DateTime GetDate(string status)
        {
            bool isValid = false;
            DateTime date = DateTime.Now;
            while(!isValid)
            {
                Console.WriteLine($"What would you like the {status} date to be?");
                try
                {
                    date = DateTime.Parse(Console.ReadLine());

                    isValid = true;
                }
                catch(Exception e)
                {
                    Console.WriteLine("Sorry, there was an error parsing that date. Please try again: (ex: mm/dd/yyyy).");
                }
            }

            return date;
        
        }

        public static void DisplayFirstReport(HomeBudget budget, DateTime? start, DateTime? end, bool filter, int catId)
        {
            Console.Clear();
            Console.WriteLine("Report for GetBudgetItems: \n");

            List<BudgetItem> budgetItemList = budget.GetBudgetItems(start, end, filter, catId);

            Console.WriteLine("+----------+---------------------+----------------+----------------+-----------+-----------+");
            Console.WriteLine(FORMAT,"ExpenseId", "Description", "Date", "Category", "Amount", "Balance");
            Console.WriteLine("+----------+---------------------+----------------+----------------+-----------+-----------+");
            foreach (BudgetItem item in budgetItemList)
            {
                Console.WriteLine(FORMAT, item.ExpenseID, item.ShortDescription, item.Date.ToString("MM/dd/yyyy"), item.Category, item.Amount, item.Balance);
                Console.WriteLine("+----------+---------------------+----------------+----------------+-----------+-----------+");
            }

            Console.WriteLine("\nPress enter to go see the next report:\n");
            Console.ReadLine();
        }

        static void DisplaySecondReport(HomeBudget budget, DateTime? start, DateTime? end, bool filter, int catId)
        {
            Console.WriteLine("\n\nReport for GetBudgetItemsByMonth:\n");

            List<BudgetItemsByMonth> budgetItemsByMonths = budget.GetBudgetItemsByMonth(start, end, filter, catId);

            foreach (BudgetItemsByMonth monthGroup in budgetItemsByMonths)
            {
                Console.WriteLine($"\n    Month: {monthGroup.Month}\n    Total for the month: {monthGroup.Total.ToString("C")}");

                Console.WriteLine("+----------+---------------------+----------------+----------------+-----------+-----------+");
                Console.WriteLine(FORMAT, "ExpenseId", "Description", "Date", "Category", "Amount", "Balance");
                Console.WriteLine("+----------+---------------------+----------------+----------------+-----------+-----------+");
                foreach (BudgetItem item in monthGroup.Details)
                {
                    Console.WriteLine(FORMAT, item.ExpenseID, item.ShortDescription, item.Date.ToString("MM/dd/yyyy"), item.Category, item.Amount, item.Balance);
                    Console.WriteLine("+----------+---------------------+----------------+----------------+-----------+-----------+");
                }
            }

            Console.WriteLine("\nPress enter to go see the next report:\n");
            Console.ReadLine();
        }

        static void DisplayThirdReport(HomeBudget budget, DateTime? start, DateTime? end, bool filter, int catId)
        {
            Console.WriteLine("\n\nReport for GeBudgetItemsByCategory:\n");

            List<BudgetItemsByCategory> budgetItemsByCategories = budget.GeBudgetItemsByCategory(start, end, filter, catId);

            foreach (BudgetItemsByCategory categoryGroup in budgetItemsByCategories)
            {
                Console.WriteLine($"\n    Category: {categoryGroup.Category}\n    Total of this category: {categoryGroup.Total.ToString("C")}");

                Console.WriteLine("+----------+---------------------+----------------+----------------+-----------+-----------+");
                Console.WriteLine(FORMAT, "ExpenseId", "Description", "Date", "Category", "Amount", "Balance");
                Console.WriteLine("+----------+---------------------+----------------+----------------+-----------+-----------+");
                foreach (BudgetItem item in categoryGroup.Details)
                {
                    Console.WriteLine(FORMAT, item.ExpenseID, item.ShortDescription, item.Date.ToString("MM/dd/yyyy"), item.Category, item.Amount, item.Balance);
                    Console.WriteLine("+----------+---------------------+----------------+----------------+-----------+-----------+");
                }
            }

            Console.WriteLine("\nPress enter to go see the next report:\n");
            Console.ReadLine();
        }

        static void DisplayFourthReport(HomeBudget budget, DateTime? start, DateTime? end, bool filter, int catId)
        {
            Console.WriteLine("\n\nReport for GetBudgetDictionaryByCategoryAndMonth:\n");

            List<Dictionary<string,object>> budgetBoth = budget.GetBudgetDictionaryByCategoryAndMonth(start, end, filter, catId);

            foreach (Dictionary<string,object> dictionary in budgetBoth)
            {
                string[] keys = dictionary.Keys.ToArray();
                int indexAfterMonthTotal = 2;

                if (dictionary.ContainsKey("Month") && dictionary.ContainsKey("Total"))
                {
                    Console.WriteLine($"\n\n    Month: {dictionary["Month"]}\n    Total for the month: {String.Format("{0:C}", dictionary["Total"])}");

                    for (int i = indexAfterMonthTotal; i < keys.Length; i = i + 2)
                    {
                        Console.WriteLine($"\nCategory: {keys[i + 1]}\nTotal for the category: {String.Format("{0:C}", dictionary[keys[i + 1]])}");

                        Console.WriteLine("+----------+---------------------+----------------+----------------+-----------+-----------+");
                        Console.WriteLine(FORMAT, "ExpenseId", "Description", "Date", "Category", "Amount", "Balance");
                        Console.WriteLine("+----------+---------------------+----------------+----------------+-----------+-----------+");

                        List<BudgetItem> budgetItems = dictionary[keys[i]] as List<BudgetItem>;

                        foreach (BudgetItem item in budgetItems)
                        {
                            Console.WriteLine(FORMAT, item.ExpenseID, item.ShortDescription, item.Date.ToString("MM/dd/yyyy"), item.Category, item.Amount, item.Balance);
                            Console.WriteLine("+----------+---------------------+----------------+----------------+-----------+-----------+");
                        }
                    }

                }
                else
                {
                    Console.WriteLine("\n\n" + dictionary["Month"] + ":");


                    for (int i = 1; i < dictionary.Count; i++)
                    {
                        Console.WriteLine(keys[i] + ": " + String.Format("{0:C}", dictionary[keys[i]]));
                    }
                }

            }
            */
        }
    }
}