// ============================================================================
// (c) Sandy Bultena 2018
// * Released under the GNU General Public License
// ============================================================================

using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;
using System.Globalization;
using System.Reflection.PortableExecutable;

namespace Budget
{
    // ====================================================================
    // CLASS: HomeBudget
    //        - Combines categories Class and expenses Class
    //        - One File defines Category and Budget File
    //        - etc
    // ====================================================================

    /// <summary>
    /// Holds the starting information for a budget. It acts as a hub for getting the different types of information including getting an
    /// already existing budget from a file, getting a list of all the BudgetItems that fit a designated filter, getting a list of BudgetItems
    /// that have been sorted based on the month they occured in or the category they belong to, and getting a dictionary that holds the 
    /// information for when the BudgetItems are sorted for by month and category at the same time. The class contains a file name, a directory
    /// name a <see cref="Categories">Categories</see> object, and an <see cref="Expenses">Expenses</see> object. It also contains properties
    /// for a path name. 
    /// 
    /// <example>
    /// Here is how to initialize an instance of this class:
    /// 
    /// <code>
    /// 
    /// string budgetFilePath = "../../../tests/test.budget";
    /// HomeBudget budget = new HomeBudget(budgetFilePath);
    /// 
    /// </code>
    /// 
    /// Now that an instance of the class has been created, methods can be called for this instance.
    /// As an example, let's say that changes have been made to the budget so the updated information needs to be save to a file again. Here
    /// is how it would be done:
    /// 
    /// <code>
    /// 
    /// budget.SaveToFile(budgetFilePath);
    /// 
    /// </code>
    /// 
    /// After running this, the file from the variable should be updated with the new additions or edits to the budget.
    /// 
    /// </example>
    /// </summary>
    public class HomeBudget
    {
        private string _FileName;
        private string _DirName;
        private Categories _categories;
        private Expenses _expenses;

        // ====================================================================
        // Properties
        // ===================================================================

        // Properties (location of files etc)

        /// <value>
        /// Returns the budget file name.
        /// </value>
        public String FileName { get { return _FileName; } }

        /// <value>
        /// Returns the directory name of the file.
        /// </value>
        public String DirName { get { return _DirName; } }

        /// <summary>
        /// Gets the full file path. If both the file and directory names are not null, the file path is a combination of the
        /// two, otherwise the file path is null.
        /// </summary>
        /// <value>
        /// Returns the file path.
        /// </value>
        public String PathName
        {
            get
            {
                if (_FileName != null && _DirName != null)
                {
                    return Path.GetFullPath(_DirName + "\\" + _FileName);
                }
                else
                {
                    return null;
                }
            }
        }

        // Properties (categories and expenses object)

        /// <value>
        /// Returns the Categories object that contains the list of all the categories for the budget.
        /// </value>
        public Categories categories { get { return _categories; } }

        /// <value>
        /// Returns the Expenses object that contains the list of all the expenses for the budget.
        /// </value>
        public Expenses expenses { get { return _expenses; } }

        // -------------------------------------------------------------------
        // Constructor (existing budget ... must specify file)
        // -------------------------------------------------------------------

        /// <summary>
        /// Creates a new HomeBudget object based on an already existing budget. It initializes the categories and expenses,
        /// which sets up all the default values, then calls another method to read from the file that contains the already 
        /// existing budget to add that informaiton into the proper places.
        /// 
        /// <example>
        /// Here is an example of how to use this:
        /// 
        /// <code>
        /// 
        /// string fileName = "./myBudget.txt";
        /// 
        /// HomeBudget myBudget = new HomeBudget(fileName);
        /// 
        /// </code>
        /// 
        /// Now, the myBudget object will have all the budget information that was in the myBudget.txt file.
        /// 
        /// </example>
        /// </summary>
        /// <param name="budgetFileName">Represents the file that contains the already existing budget.</param>
        /// <exception cref="Exception">Thrown when there is a problem reading from the file.</exception>
        public HomeBudget(String databaseFile, String expensesXMLFile, bool newDB=false)
        {
            //If the user does not want to reset the databse and it exists we read from existing database
            if(!newDB && File.Exists(databaseFile))
            {
                Database.existingDatabase(databaseFile);
            }
            else
            {//If the database does not exist or the user wants to reset it we create a new one
                Database.newDatabase(databaseFile);
                newDB = true;
            }

            //If the newDB flag is set to true the categories are reset
            _categories = new Categories(Database.dbConnection, newDB);

            //Intializes Expenses
            _expenses = new Expenses(Database.dbConnection);

            //read the expenese from the xml
            _expenses.ReadFromFile(expensesXMLFile);
        }

        #region OpenNewAndSave
        // ---------------------------------------------------------------
        // Read
        // Throws Exception if any problem reading this file
        // ---------------------------------------------------------------

        /// <summary>
        /// Reads and processes the information from the budget file. It calls a method from the BudgetFiles class to verify
        /// the budget file that is passed in as a parameter. If the file is valid, all the file's content is saved in an 
        /// array, and file and directory name of the file are saved to different variables. Then it calls methods to read
        /// the files that correlate to the categories and expenses, and that data is saved to its respective backing field.
        /// 
        /// <example>
        /// Here is an example of how to use this:
        /// 
        /// A new HomeBudget is cerated. It has been left empty.
        /// <code>
        /// 
        /// HomeBudget budget = new HomeBudget();
        /// 
        /// </code>
        /// 
        /// After it was created, a file containing information about an already existing budget was found, and now needs to be implemented
        /// into the program. Instead of creating a new HomeBudget object, the following method can be used to continue using the budget
        /// HomeBudget object.
        /// 
        /// <code>
        /// 
        /// string existingBudget = "../foundANewBudget.txt";
        /// 
        /// budget.ReadFromFile(existingBudget);
        /// 
        /// </code>
        /// 
        /// Now the contents of the foundANewBudget.txt file will be added to the budget object.
        /// 
        /// </example>
        /// 
        /// </summary>
        /// <param name="budgetFileName">Represents the file that holds the already existing budget.</param>
        /// <exception cref="Exception">Thrown if something goes wrong while trying to read from the files.</exception>
        public void ReadFromFile(String budgetFileName)
        {
            // ---------------------------------------------------------------
            // read the budget file and process
            // ---------------------------------------------------------------
            try
            {
                // get filepath name (throws exception if it doesn't exist)
                budgetFileName = BudgetFiles.VerifyReadFromFileName(budgetFileName, "");

                // If file exists, read it
                string[] filenames = System.IO.File.ReadAllLines(budgetFileName);

                // ----------------------------------------------------------------
                // Save information about budget file
                // ----------------------------------------------------------------
                string folder = Path.GetDirectoryName(budgetFileName);
                _FileName = Path.GetFileName(budgetFileName);

                // read the expenses and categories from their respective files
                _expenses.ReadFromFile(folder + "\\" + filenames[1]);

                // Save information about budget file
                _DirName = Path.GetDirectoryName(budgetFileName);
                _FileName = Path.GetFileName(budgetFileName);
            }

            // ----------------------------------------------------------------
            // throw new exception if we cannot get the info that we need
            // ----------------------------------------------------------------
            catch (Exception e)
            {
                throw new Exception("Could not read budget info: \n" + e.Message);
            }

        }

        // ====================================================================
        // save to a file
        // saves the following files:
        //  filepath_expenses.exps  # expenses file
        //  filepath_categories.cats # categories files
        //  filepath # a file containing the names of the expenses and categories files.
        //  Throws exception if we cannot write to that file (ex: invalid dir, wrong permissions)
        // ====================================================================

        /// <summary>
        /// Prepares to write the budget information to files. It starts off by clearing the directory and file name backing 
        /// fields to make sure the right information is used. Then, it calls a method from the BudgetFiles class to get a
        /// valid file path and saves information about the file path. It then creates file paths for the files that will hold
        /// the categories and expenses information, and then runs the respective methods that will actually save the 
        /// information to those files. It then writes the names of those two files to the budget information file and saves
        /// that file's directory and file name to the backing fields.
        /// 
        /// <example>
        /// Here is an example of how to use this:
        /// 
        /// A new budget is created.
        /// <code>
        /// 
        /// string fileName = "../myBudget.txt";
        /// 
        /// HomeBudget budget = new HomeBudget(filename);
        /// 
        /// </code>
        /// 
        /// The user continues to use this budget for a few weeks. They add dozens upon dozens of expenses to it, and now they don't want to 
        /// lose this information. To save it, they would just have to do the following:
        /// 
        /// <code>
        /// 
        /// budget.SaveToFile(fileName);
        /// 
        /// </code>
        /// 
        /// Now all their progress has been saved to the file that their original budget came from.
        /// 
        /// </example>
        /// 
        /// </summary>
        /// <param name="filepath">Represents the file path of the file that will be written to.</param>
        /// <exception cref="Exception">Thrown when file doesn't exist or its read only.</exception>
        /// <exception cref="DirectoryNotFoundException">Thrown when the directory specified in a file path does not exist.</exception>
        /// <exception cref="PathTooLongException">Thrown when the names of the directories and files in a file path is longer 
        /// than the maximum length a file path is given by the operating system.</exception>
        public void SaveToFile(String filepath)
        {

            // ---------------------------------------------------------------
            // just in case filepath doesn't exist, reset path info
            // ---------------------------------------------------------------
            _DirName = null;
            _FileName = null;

            // ---------------------------------------------------------------
            // get filepath name (throws exception if we can't write to the file)
            // ---------------------------------------------------------------
            filepath = BudgetFiles.VerifyWriteToFileName(filepath, "");

            String path = Path.GetDirectoryName(Path.GetFullPath(filepath));
            String file = Path.GetFileNameWithoutExtension(filepath);
            String ext = Path.GetExtension(filepath);

            // ---------------------------------------------------------------
            // construct file names for expenses and categories
            // ---------------------------------------------------------------
            String expensepath = path + "\\" + file + "_expenses" + ".exps";
            String categorypath = path + "\\" + file + "_categories" + ".cats";

            // ---------------------------------------------------------------
            // save the expenses and categories into their own files
            // ---------------------------------------------------------------
            _expenses.SaveToFile(expensepath);

            // ---------------------------------------------------------------
            // save filenames of expenses and categories to budget file
            // ---------------------------------------------------------------
            string[] files = { Path.GetFileName(categorypath), Path.GetFileName(expensepath) };
            System.IO.File.WriteAllLines(filepath, files);

            // ----------------------------------------------------------------
            // save filename info for later use
            // ----------------------------------------------------------------
            _DirName = path;
            _FileName = Path.GetFileName(filepath);
        }
        #endregion OpenNewAndSave

        #region GetList

        // ============================================================================
        // Get all expenses list
        // NOTE: VERY IMPORTANT... budget amount is the negative of the expense amount
        // Reasoning: an expense of $15 is -$15 from your bank account.
        // ============================================================================

        /// <summary>
        /// Gets all the budget items into a list and returns that list. Can filter the results by start and end date, which
        /// only gets the budget items that took place between whatever time frame is created. If no dates are selected, 
        /// default ones are in place to select all items. There is also the option to filter the results to only see items
        /// from one category. This can happen by setting the FilterFlag parameter to true and inserting the category id of
        /// the wanted category as the last parameter. These filters are not necessary for the method to function. 
        /// 
        /// The method starts off by saving the times, then gets a collection of the categories and the expenses that fall 
        /// within the time limits that have been set. A BudgetItem list is created and a loop goes through all the elements
        /// in the collection of categories and expenses. A total is calculated based on the expenses amounts. If the 
        /// FilterFlag is true, it filters out all the items that are not in the desired category, otherwise, all items are 
        /// considered. The remaining elements get created into BudgetItem objects, using the information from both the 
        /// categories and expenses data. These new objects are added to the BudgetItem list, which is then returned once
        /// the loop is over.
        /// 
        /// <example>
        /// Here is an example of how to use this method:
        /// 
        /// It starts off with this pre-existing budget that contains the content shown below.
        /// <code>
        /// 
        /// string budgetFilePath = "../../../tests/test.budget";
        /// 
        /// ExpenseID   Description         Date        Category        CategoryID  Amount  Balance
        /// 1           hat (on credit)     01-10-2018  Clothes         10          -10     -10
        /// 2           hat                 01-11-2018  Credit Card     9           10      0
        /// 3           scarf (on credit)   01-10-2019  Clothes         10          -15     -15
        /// 4           scarf               01-10-2020  Credit Card     9           15      0
        /// 5           McDonalds           01-11-2020  Eating Out      14          -45     -45
        /// 7           Wendys              01-12-2020  Eating Out      14          -25     -70
        /// 10          Pizza               02-01-2020  Eating Out      14          -33.33  -103.33
        /// 13          mittens             02-10-2020  Credit Card     9           15      -88.33
        /// 12          Hat                 02-25-2020  Credit Card     9           25      -63.33
        /// 11          Pizza               02-27-2020  Eating Out      14          -33.33  -96.66
        /// 9           Cafeteria           07-11-2020  Eating Out      14          -11.11  -107.77
        /// 
        /// </code>
        /// 
        /// By creating a new HomeBudget object using this file, all this data with be added to the budget. At the same time, 
        /// the variables to call this method will be created.
        /// 
        /// <code>
        /// 
        /// HomeBudget budget = new HomeBudget(budgetFilePath);
        /// 
        /// DateTime? start = null, end = null;
        /// bool filter = false;
        /// int categoryId = 2;
        /// <![CDATA[
        /// List<BudgetItem> budgetItems = budget.GetBudgetItems(start, end, filter, categoryId);
        /// 
        /// ]]>
        /// </code>
        /// 
        /// Since the start/end values were null and the filter was set to false, all the budget items from above will be in the list.
        /// This can be tested by doing a quick loop through the list to get print some of the information to the console.
        /// 
        /// <code>
        /// 
        /// foreach (BudgetItem item in budgetItems){
        /// 
        ///     Console.WriteLine(item.ExpenseID);
        /// 
        /// }
        /// 
        /// </code>
        /// 
        /// The above code would give the following output:
        /// 
        /// <code>
        /// 
        /// 1       
        /// 2        
        /// 3    
        /// 4          
        /// 5       
        /// 7     
        /// 10  
        /// 13       
        /// 12        
        /// 11        
        /// 9 
        /// 
        /// </code>
        /// </example>
        /// 
        /// </summary>
        /// <param name="Start">Represents the earliest date the budget item could have happened on.</param>
        /// <param name="End">Represents the latest date/futuristic date the budget item could have happened on.</param>
        /// <param name="FilterFlag">Represents whether the BudgetItem list should include all categories or not.</param>
        /// <param name="CategoryID">Represents the category the items can be a part of if the FilterFlag is on.</param>
        /// <returns>A list of BudgetItems that respect the filters that were passed to the method.</returns>
        public List<BudgetItem> GetBudgetItems(DateTime? Start, DateTime? End, bool FilterFlag, int CategoryID)
        {
            // ------------------------------------------------------------------------
            // return joined list within time frame
            // ------------------------------------------------------------------------
            DateTime StartDate = Start ?? new DateTime(1900, 1, 1);
            DateTime EndDate = End ?? new DateTime(2500, 1, 1);

            //Sets the start time and end time to what was specified OR makes it 1900 and 2500 if the specified parameters are null
            if (Start == null){ StartDate = new DateTime(1900, 1, 1); }
            if(End == null) { EndDate = new DateTime(2500, 1, 1); }

            //Creating the Query
            SQLiteDataReader sqliteReader;
            SQLiteCommand cmd = Database.dbConnection.CreateCommand();

            //Creating the Query
            cmd.CommandText = "SELECT categories.Id AS CategoryId, expenses.Id AS ExpenseId, expenses.Date AS Date, categories.Description AS Category, expenses.Description AS ShortDescription, expenses.Amount AS Amount \n" +
                "FROM expenses \n" +
                "JOIN categories ON expenses.CategoryId=categories.Id \n" +
                "WHERE expenses.Date > @startDate AND expenses.Date < @endDate;";
            cmd.Parameters.Add(new SQLiteParameter("@startDate", StartDate.ToString("yyyy-MM-dd")));
            cmd.Parameters.Add(new SQLiteParameter("@endDate", EndDate.ToString("yyyy-MM-dd")));

            sqliteReader = cmd.ExecuteReader();

            // ------------------------------------------------------------------------
            // create a BudgetItem list with totals,
            // ------------------------------------------------------------------------
            List<BudgetItem> items = new List<BudgetItem>();
            
            if (sqliteReader.HasRows)
            {
                while (sqliteReader.Read())
                {
                    // filter out unwanted categories if filter flag is on
                    if (FilterFlag == true && CategoryID != sqliteReader.GetInt32(0))
                    {
                        continue;
                    }
                    else
                    {
                        BudgetItem budgetItem = new BudgetItem();
                        budgetItem.CategoryID = sqliteReader.GetInt32(0);
                        budgetItem.ExpenseID = sqliteReader.GetInt32(1);
                        budgetItem.Date = DateTime.ParseExact(sqliteReader.GetString(2), "yyyy-MM-dd", CultureInfo.InvariantCulture);
                        budgetItem.Category = sqliteReader.GetString(3);
                        budgetItem.ShortDescription = sqliteReader.GetString(4);
                        budgetItem.Amount = sqliteReader.GetDouble(5);

                        items.Add(budgetItem);
                    }

                }
            }

            //Sorting the list by date
            items.Sort((x, y) => x.Date.CompareTo(y.Date));

            Double total = 0;
            foreach (BudgetItem item in items)
            {
                total += item.Amount;
                item.Balance = total;
            }

            sqliteReader.Close();

            return items;
        }

        // ============================================================================
        // Group all expenses month by month (sorted by year/month)
        // returns a list of BudgetItemsByMonth which is 
        // "year/month", list of budget items, and total for that month
        // ============================================================================

        /// <summary>
        /// Sorts all the BudgetItems into lists, which are stored in BudgetItemsByMonth objects, which are stored and returned 
        /// in a list. As the class name suggests, the budget items are sorted by the month they occur in.
        /// 
        /// The method takes in the start date, end date, a FilterFlag which determines if the budget items will be filtered by
        /// category, and a category id. These parameters are used near the beginning of the method, where the GetBudgetItems 
        /// method is called. This returns a list of budget items that are filtered based on the parameters that were passed in.
        /// 
        /// All these budget items get grouped into a collection of elements, where each element holds all the budget items that
        /// happened in the same month. These groups are sorted in a year/month order. To save this data in the actual class lists
        /// that have been created, there is a foreach loop that goes through each element in the collection. Each iteration of the
        /// loop keeps track of the total amount of the budget items and creates a new list of budget items. It also includes a 
        /// nested foreach loop that goes through each budget item from the group and increases the total, then adds the budget item 
        /// to the list. Once the inner loop is done, it creates a BudgetItemsByMonth object using the group's key, the list of 
        /// budget items, and the total from the inner loop. This object is added to the list of BudgetItemsByMonth, and the cycle 
        /// continues until the outer loop is done.
        /// 
        /// When the outer loop has finished, it means that all the budget items have been sorted into BudgetItemsByMonth objects, 
        /// which have all been added to the list, so that list is returned.
        /// 
        /// <example>
        /// Here is an example of how to use this method:
        /// 
        /// It starts off with this pre-existing budget that contains the content shown below.
        /// <code>
        /// 
        /// string budgetFilePath = "../tests/test.budget";
        /// 
        /// ExpenseID   Description         Date        Category        CategoryID  Amount  Balance
        /// 1           hat (on credit)     01-10-2018  Clothes         10          -10     -10
        /// 2           hat                 01-11-2018  Credit Card     9           10      0
        /// 3           scarf (on credit)   01-10-2019  Clothes         10          -15     -15
        /// 4           scarf               01-10-2020  Credit Card     9           15      0
        /// 5           McDonalds           01-11-2020  Eating Out      14          -45     -45
        /// 7           Wendys              01-12-2020  Eating Out      14          -25     -70
        /// 10          Pizza               02-01-2020  Eating Out      14          -33.33  -103.33
        /// 13          mittens             02-10-2020  Credit Card     9           15      -88.33
        /// 12          Hat                 02-25-2020  Credit Card     9           25      -63.33
        /// 11          Pizza               02-27-2020  Eating Out      14          -33.33  -96.66
        /// 9           Cafeteria           07-11-2020  Eating Out      14          -11.11  -107.77
        /// 
        /// </code>
        /// 
        /// By creating a new HomeBudget object using this file, all this data with be added to the budget. At the same time, 
        /// the variables to call this method will be created.
        /// 
        /// <code>
        /// 
        /// HomeBudget budget = new HomeBudget(budgetFilePath);
        /// 
        /// DateTime? start = null, end = null;
        /// bool filter = false;
        /// int categoryId = 2;
        /// <![CDATA[
        /// List<BudgetItemsByMonth> budgetItemsByMonth = budget.GetBudgetItemsByMonth(start, end, filter, categoryId);
        /// 
        /// ]]>
        /// </code>
        /// 
        /// Since the start/end values were null and the filter was set to false, all the budget items can be found somewhere in the
        /// list. The question is just which month is it in.
        /// This can be tested by doing a couple loops through the list to get print some of the information to the console.
        /// 
        /// <code>
        /// 
        /// foreach (BudgetItemsByMonth monthGroup in budgetItemsByMonths)
        /// {
        ///        Console.WriteLine($"\nMonth: {monthGroup.Month}");
        ///        
        ///        foreach (BudgetItem item in monthGroup.Details)
        ///        {
        ///            Console.WriteLine(item.ShortDescription, item.Date.ToString("MM/dd/yyyy"));
        ///        }
        /// }
        /// 
        /// </code>
        /// 
        /// The above code would give the following output:
        /// 
        /// <code>
        /// 
        /// Month: 2018/01
        /// hat (on credit)     01-10-2018  
        /// hat                 01-11-2018  
        /// 
        /// Month: 2019/01
        /// scarf (on credit)   01-10-2019  
        /// 
        /// Month: 2020/01
        /// scarf               01-10-2020  
        /// McDonalds           01-11-2020
        /// Wendys              01-12-2020
        /// 
        /// Month: 2020/02
        /// Pizza               02-01-2020 
        /// mittens             02-10-2020 
        /// Hat                 02-25-2020 
        /// Pizza               02-27-2020 
        /// 
        /// Month: 2020/07
        /// Cafeteria           07-11-2020
        /// 
        /// </code>
        /// </example>
        /// </summary>
        /// <param name="Start">Represents the earliest date the budget item could have happened on.</param>
        /// <param name="End">Represents the latest date/futuristic date the budget item could have happened on.</param>
        /// <param name="FilterFlag">Represents whether the BudgetItem list should include all categories or not.</param>
        /// <param name="CategoryID">Represents the category the items can be a part of if the FilterFlag is on.</param>
        /// <returns>A list of BudgetItemsByMonth, in which the Details of all the objects is a list of all the BudgetItems that
        /// occured in the same month.</returns>
        public List<BudgetItemsByMonth> GetBudgetItemsByMonth(DateTime? Start, DateTime? End, bool FilterFlag, int CategoryID)
        {
            // -----------------------------------------------------------------------
            // get all items first
            // -----------------------------------------------------------------------
            List<BudgetItem> items = GetBudgetItems(Start, End, FilterFlag, CategoryID);

            //get all budget items grouped by month
            //once that is done, requery to get all bugdte items for specific month
            //add those into the details of budget items by month
            //seems useless to query twice
            //wtv lets do it that way then see
           
            //Get all expenses grouped by month
            string expenseQuery = "SELECT SUM(Amount), substr(Date ,1 ,7) FROM expenses WHERE Date >= startDate AND Date =< endDate GROUP BY substr(Date ,1 ,7);"

                //write query for categories and then inner join
            string categoryQuery = "";
            // -----------------------------------------------------------------------
            // Group by year/month
            // -----------------------------------------------------------------------
            var GroupedByMonth = items.GroupBy(c => c.Date.Year.ToString("D4") + "/" + c.Date.Month.ToString("D2"));

            // -----------------------------------------------------------------------
            // create new list
            // -----------------------------------------------------------------------
            var summary = new List<BudgetItemsByMonth>();
            foreach (var MonthGroup in GroupedByMonth)
            {
                // calculate total for this month, and create list of details
                double total = 0;
                var details = new List<BudgetItem>();
                foreach (var item in MonthGroup)
                {
                    total = total + item.Amount;
                    details.Add(item);
                }

                // Add new BudgetItemsByMonth to our list
                summary.Add(new BudgetItemsByMonth
                {
                    Month = MonthGroup.Key,
                    Details = details,
                    Total = total
                });
            }

            return summary;
        }

        // ============================================================================
        // Group all expenses by category (ordered by category name)
        // ============================================================================

        /// <summary>
        /// Sorts all the BudgetItems into lists, which are stored in BudgetItemsByCategory objects, which are stored and returned 
        /// in a list. As the class name suggests, the budget items are sorted by the category they belong to.
        /// 
        /// The method takes in the start date, end date, a FilterFlag which determines if the budget items will be filtered by
        /// category, and a category id. These parameters are used near the beginning of the method, where the GetBudgetItems 
        /// method is called. This returns a list of budget items that are filtered based on the parameters that were passed in.
        /// 
        /// All these budget items get grouped into a collection of elements, where each element holds all the budget items that
        /// all belong to the same category. These groups are sorted in alphabetical order based on the category name. To save 
        /// this data in the actual class lists that have been created, there is a foreach loop that goes through each element in 
        /// the collection. Each iteration of the loop keeps track of the total amount of the budget items and creates a new list 
        /// of budget items. It also includes a nested foreach loop that goes through each budget item from the group and increases
        /// the total, then adds the budget item to the list. Once the inner loop is done, it creates a BudgetItemsByCategory object 
        /// using the group's key, the list of budget items, and the total from the inner loop. This object is added to the list of 
        /// BudgetItemsByCategory, and the cycle continues until the outer loop is done.
        /// 
        /// When the outer loop has finished, it means that all the budget items have been sorted into BudgetItemsByCategory objects, 
        /// which have all been added to the list, so that list is returned.
        /// 
        /// <example>
        /// Here is an example of how to use this method:
        /// 
        /// It starts off with this pre-existing budget that contains the content shown below.
        /// <code>
        /// 
        /// string budgetFilePath = "./tests/test.budget";
        /// 
        /// ExpenseID   Description         Date        Category        CategoryID  Amount  Balance
        /// 1           hat (on credit)     01-10-2018  Clothes         10          -10     -10
        /// 2           hat                 01-11-2018  Credit Card     9           10      0
        /// 3           scarf (on credit)   01-10-2019  Clothes         10          -15     -15
        /// 4           scarf               01-10-2020  Credit Card     9           15      0
        /// 5           McDonalds           01-11-2020  Eating Out      14          -45     -45
        /// 7           Wendys              01-12-2020  Eating Out      14          -25     -70
        /// 10          Pizza               02-01-2020  Eating Out      14          -33.33  -103.33
        /// 13          mittens             02-10-2020  Credit Card     9           15      -88.33
        /// 12          Hat                 02-25-2020  Credit Card     9           25      -63.33
        /// 11          Pizza               02-27-2020  Eating Out      14          -33.33  -96.66
        /// 9           Cafeteria           07-11-2020  Eating Out      14          -11.11  -107.77
        /// 
        /// </code>
        /// 
        /// By creating a new HomeBudget object using this file, all this data with be added to the budget. At the same time, 
        /// the variables to call this method will be created.
        /// 
        /// <code>
        /// 
        /// HomeBudget budget = new HomeBudget(budgetFilePath);
        /// 
        /// DateTime? start = null, end = null;
        /// bool filter = false;
        /// int categoryId = 2;
        /// <![CDATA[
        /// List<BudgetItemsByCategory> budgetItemsByCategory = budget.GetBudgetItemsByCategory(start, end, filter, categoryId);
        /// 
        /// ]]>
        /// </code>
        /// 
        /// Since the start/end values were null and the filter was set to false, all the budget items can be found somewhere in the
        /// list. The question is just which category is it in.
        /// This can be tested by doing a couple loops through the list to get print some of the information to the console.
        /// 
        /// <code>
        /// 
        /// foreach (BudgetItemsByCategory categoryGroup in budgetItemsByCategory)
        /// {
        ///        Console.WriteLine($"\nCategory: {categoryGroup.Category}");
        ///        
        ///        foreach (BudgetItem item in categoryGroup.Details)
        ///        {
        ///            Console.WriteLine(item.ShortDescription, item.Category, item.CategoryID);
        ///        }
        /// }
        /// 
        /// </code>
        /// 
        /// The above code would give the following output:
        /// 
        /// <code>
        /// 
        /// Category: Clothes
        /// hat (on credit)     Clothes         10
        /// scarf (on credit)   Clothes         10
        /// 
        /// Category: Credit Card
        /// hat                 Credit Card     9
        /// scarf               Credit Card     9 
        /// mittens             Credit Card     9 
        /// Hat                 Credit Card     9 
        /// 
        /// Category: Eating Out
        /// McDonalds           Eating Out      14 
        /// Wendys              Eating Out      14  
        /// Pizza               Eating Out      14    
        /// Pizza               Eating Out      14   
        /// Cafeteria           Eating Out      14  
        /// 
        /// </code>
        /// </example>
        /// </summary>
        /// <param name="Start">Represents the earliest date the budget item could have happened on.</param>
        /// <param name="End">Represents the latest date/futuristic date the budget item could have happened on.</param>
        /// <param name="FilterFlag">Represents whether the BudgetItem list should include all categories or not.</param>
        /// <param name="CategoryID">Represents the category the items can be a part of if the FilterFlag is on.</param>
        /// <returns>A list of BudgetItemsByCategory, in which the Details of all the objects is a list of all the BudgetItems that
        /// are a part of the same category.</returns>
        public List<BudgetItemsByCategory> GeBudgetItemsByCategory(DateTime? Start, DateTime? End, bool FilterFlag, int CategoryID)
        {
            // -----------------------------------------------------------------------
            // get all items first
            // -----------------------------------------------------------------------
            List<BudgetItem> items = GetBudgetItems(Start, End, FilterFlag, CategoryID);

            // -----------------------------------------------------------------------
            // Group by Category
            // -----------------------------------------------------------------------
            var GroupedByCategory = items.GroupBy(c => c.Category);

            // -----------------------------------------------------------------------
            // create new list
            // -----------------------------------------------------------------------
            List<BudgetItemsByCategory> summary = new List<BudgetItemsByCategory>();
            foreach (var CategoryGroup in GroupedByCategory.OrderBy(g => g.Key))
            {
                // calculate total for this category, and create list of details
                double total = 0;
                List<BudgetItem> details = new List<BudgetItem>();
                foreach (var item in CategoryGroup)
                {
                    total = total + item.Amount;
                    details.Add(item);
                }

                // Add new BudgetItemsByCategory to our list
                summary.Add(new BudgetItemsByCategory
                {
                    Category = CategoryGroup.Key,
                    Details = details,
                    Total = total
                });
            }

            return summary;
        }


        // ============================================================================
        // Group all expenses by category and Month
        // creates a list of Dictionary objects (which are objects that contain key value pairs).
        // The list of Dictionary objects includes:
        //          one dictionary object per month with expenses,
        //          and one dictionary object for the category totals
        // 
        // Each per month dictionary object has the following key value pairs:
        //           "Month", <the year/month for that month as a string>
        //           "Total", <the total amount for that month as a double>
        //            and for each category for which there is an expense in the month:
        //             "items:category", a List<BudgetItem> of all items in that category for the month
        //             "category", the total amount for that category for this month
        //
        // The one dictionary for the category totals has the following key value pairs:
        //             "Month", the string "TOTALS"
        //             for each category for which there is an expense in ANY month:
        //             "category", the total for that category for all the months
        // ============================================================================

        /// <summary>
        /// Gets a list of dictionaries that hold information about the expenses for every month, also broken down into how those expenses were 
        /// divided category wise. The method takes fours parameters and they are all passed to the GetBudgetItemsByMonth method so that the budget items can be filtered
        /// and sorted by month to start everything off. This is stored in a list.
        /// 
        /// Next there is the creation of a list of dictionaries, summary, which is the dictionary that will gain all the other information and will
        /// be returned at the end of the method, and the dictionary totalsPerCategory, which keeps track of the total monetary amount linked to all 
        /// the expenses for each category.
        /// 
        /// It then has a foreach loop where it goes through each month from the list of budget items, creates a new dictionary to hold the month's
        /// information, including the month itself, the total, each category that has an expense during the month, and the total amount for each
        /// of those categories. To get the latter information, the budget items are grouped into their categories and fed through another loop.
        /// In this loop, a new budget item list is created so all the items can be added to it, and the total is tracked. Once all the items are
        /// added to the the list, this information needs to be added to the dictionary that represents this month. With the information stored,
        /// the totalsPerCategory dictionary needs to be updated, so it either updates the key, or initializes a new one. Once looping through all 
        /// the categories in a month is done, it adds the dictionary that was created to the summary list, and the process continues for the 
        /// next month.
        /// 
        /// Once all that has finished, a last dictionary is created to hold information about the total amounts for every category. This information
        /// is gained from the totalsPerCategory dictionary. It essentially goes through each category from that dictionary and adds the category and
        /// the total to the last dictionary. When it has gone through all the categories, it adds this new dictionary to the summary list and
        /// returns the summary list.
        /// 
        /// <example>
        /// Here is an example of how to use this method:
        /// 
        /// It starts off with this pre-existing budget that contains the content shown below.
        /// <code>
        /// 
        /// string budgetFilePath = "./tests/test.budget";
        /// 
        /// ExpenseID   Description         Date        Category        CategoryID  Amount  Balance
        /// 1           hat (on credit)     01-10-2018  Clothes         10          -10     -10
        /// 2           hat                 01-11-2018  Credit Card     9           10      0
        /// 3           scarf (on credit)   01-10-2019  Clothes         10          -15     -15
        /// 4           scarf               01-10-2020  Credit Card     9           15      0
        /// 5           McDonalds           01-11-2020  Eating Out      14          -45     -45
        /// 7           Wendys              01-12-2020  Eating Out      14          -25     -70
        /// 10          Pizza               02-01-2020  Eating Out      14          -33.33  -103.33
        /// 13          mittens             02-10-2020  Credit Card     9           15      -88.33
        /// 12          Hat                 02-25-2020  Credit Card     9           25      -63.33
        /// 11          Pizza               02-27-2020  Eating Out      14          -33.33  -96.66
        /// 9           Cafeteria           07-11-2020  Eating Out      14          -11.11  -107.77
        /// 
        /// </code>
        /// 
        /// By creating a new HomeBudget object using this file, all this data with be added to the budget. At the same time, 
        /// the variables to call this method will be created.
        /// 
        /// <code>
        /// 
        /// HomeBudget budget = new HomeBudget(budgetFilePath);
        /// 
        /// DateTime? start = null, end = null;
        /// bool filter = false;
        /// int categoryId = 2;
        /// <![CDATA[
        /// List<Dictionary<string,object>> budgetBoth = budget.GetBudgetDictionaryByCategoryAndMonth(start, end, filter, categoryId);
        /// 
        /// ]]>
        /// </code>
        /// 
        /// Since the start/end values were null and the filter was set to false, all the budget items can be found somewhere in the
        /// list. They are just spread out throughout different dictionaries based on the month they occured and what category they
        /// are a part of.
        /// This can be tested by doing a couple loops through the list to get print some of the information to the console.
        /// 
        /// <code>
        /// <![CDATA[
        /// foreach (Dictionary<string,object> dictionary in budgetBoth)
        /// {
        ///     string[] keys = dictionary.Keys.ToArray();
        ///     int indexAfterMonthTotal = 2;
        ///
        ///     //checks if it a regular dictionary or if it is the totals dictionary
        ///     if (dictionary.ContainsKey("Month") && dictionary.ContainsKey("Total"))
        ///     {
        ///         Console.WriteLine($"\n    Month: {dictionary["Month"]}\n    Total for the month: {String.Format("{0:C}", dictionary["Total"])}");
        ///         for (int i = indexAfterMonthTotal; i<keys.Length; i = i + 2)
        ///         {
        ///               Console.WriteLine($"\nCategory: {keys[i + 1]}\nTotal for the category: {String.Format("{0:C}", dictionary[keys[i + 1]])}");
        ///
        ///               List<BudgetItem> budgetItems = dictionary[keys[i]] as List<BudgetItem>;
        ///
        ///               foreach (BudgetItem item in budgetItems)
        ///               {
        ///                   Console.WriteLine(item.ShortDescription, item.Date.ToString("MM/dd/yyyy"), item.Category, item.Amount);
        ///               }
        ///          }
        ///      }
        ///      else
        ///      {
        ///          Console.WriteLine("\n\n" + dictionary["Month"] + ":");
        ///
        ///          for (int i = 1; i < dictionary.Count; i++)
        ///          {
        ///               Console.WriteLine(keys[i] + ": " + String.Format("{0:C}", dictionary[keys[i]]));
        ///          }
        ///      }
        /// }
        /// ]]>
        /// </code>
        /// 
        /// The above code would give the following output:
        /// 
        /// <code>
        /// 
        ///     Month: 2018/01
        ///     Total for the month: $0.00
        /// 
        /// Category: Clothes
        /// Total for the category: -$10.00
        /// hat (on credit)     01-10-2018  Clothes         -10    
        /// 
        /// Category: Credit Card
        /// Total for the category: $10.00
        /// hat                 01-11-2018  Credit Card     10      
        /// 
        /// 
        ///     Month: 2019/01
        ///     Total for the month -$15.00
        ///     
        /// Category: Clothes
        /// Total for the category: -$15.00
        /// scarf (on credit)   01-10-2019  Clothes         -15     
        /// 
        /// 
        ///     Month: 2020/01
        ///     Total for the month: -$55.00
        ///     
        /// Category: Credit Card
        /// Total for the category: $15.00
        /// scarf               01-10-2020  Credit Card     15      
        /// 
        /// Category: Eating Out
        /// Total for the category: -$70.00
        /// McDonalds           01-11-2020  Eating Out      -45     
        /// Wendys              01-12-2020  Eating Out      -25  
        /// 
        /// 
        ///     Month: 2020/02
        ///     Total for the month: -$26.66
        ///     
        /// Category: Credit Card
        /// Total for the category: $40.00
        /// mittens             02-10-2020  Credit Card     15    
        /// Hat                 02-25-2020  Credit Card     25    
        /// 
        /// Category: Eating Out
        /// Total for the category: -$66.66
        /// Pizza               02-01-2020  Eating Out      -33.33
        /// Pizza               02-27-2020  Eating Out      -33.33
        /// 
        /// 
        ///     Month: 2020/07
        ///     Total for the month: -$11.11
        ///     
        /// Category: Eating Out
        /// Total for the category: -$11.11
        /// Cafeteria           07-11-2020  Eating Out      -11.11
        /// 
        /// </code>
        /// </example>
        /// 
        /// </summary>
        /// <param name="Start">Represents the earliest date the budget item could have happened on.</param>
        /// <param name="End">Represents the latest date/futuristic date the budget item could have happened on.</param>
        /// <param name="FilterFlag">Represents whether the BudgetItem list should include all categories or not.</param>
        /// <param name="CategoryID">Represents the category the items can be a part of if the FilterFlag is on.</param>
        /// <returns>A list of dictionaries that include information about budget items and expenses separated by month and category.</returns>
        public List<Dictionary<string, object>> GetBudgetDictionaryByCategoryAndMonth(DateTime? Start, DateTime? End, bool FilterFlag, int CategoryID)
        {
            // -----------------------------------------------------------------------
            // get all items by month 
            // -----------------------------------------------------------------------
            List<BudgetItemsByMonth> GroupedByMonth = GetBudgetItemsByMonth(Start, End, FilterFlag, CategoryID);
            //info per month


            // -----------------------------------------------------------------------
            // loop over each month
            // -----------------------------------------------------------------------
            //List<Dictionary<string, object>>
            var summary = new List<Dictionary<string, object>>();
            //Dictionary<String, Double>
            var totalsPerCategory = new Dictionary<String, Double>();


            //BudgetItemsByMonth
            foreach (var MonthGroup in GroupedByMonth)      //separating everything my months
            {
                // create record object for this month
                Dictionary<string, object> record = new Dictionary<string, object>();

                //populating the month and total for the dictionary
                //budgetitemsbymonth has month, details, and total properties
                record["Month"] = MonthGroup.Month;
                record["Total"] = MonthGroup.Total;

                // break up the month details into categories
                //ienumerable grouping/dictionary <string, budgetItem> ==> category name, budgetItem
                var GroupedByCategory = MonthGroup.Details.GroupBy(c => c.Category);

                // -----------------------------------------------------------------------
                // loop over each category
                // -----------------------------------------------------------------------
                //ienumerable<grouping<category,budgetItem>>
                //dictionary <category/string,budgetItem>

                //it goes through each key/category in the dictionary separated by the months?
                //categoryGroup is still a dictionary/grouping
                foreach (var CategoryGroup in GroupedByCategory.OrderBy(g => g.Key))
                {

                    // calculate totals for the cat/month, and create list of details
                    double total = 0;

                    //list<budgetItem>
                    var details = new List<BudgetItem>();

                    //category group = dictionary/grouping<category,budget item>
                    //budget item
                    foreach (var item in CategoryGroup)
                    {
                        total = total + item.Amount;    //budget item amount
                        details.Add(item);
                    }

                    // add new properties and values to our record object
                    //key is the category, since it is the first element of the <category,budget item> dictionary
                    record["details:" + CategoryGroup.Key] = details;
                    record[CategoryGroup.Key] = total;

                    //these two things are added for each category
                    //on top of the "Month" and "Total" from before

                    // keep track of totals for each category
                    if (totalsPerCategory.TryGetValue(CategoryGroup.Key, out Double CurrentCatTotal))
                    {
                        //dictionary <string,double>
                        totalsPerCategory[CategoryGroup.Key] = CurrentCatTotal + total;
                    }
                    else
                    {
                        //dictionary <string,double>
                        totalsPerCategory[CategoryGroup.Key] = total;
                    }
                }

                // add record to collection
                summary.Add(record);
            }
            // ---------------------------------------------------------------------------
            // add final record which is the totals for each category
            // ---------------------------------------------------------------------------
            Dictionary<string, object> totalsRecord = new Dictionary<string, object>();
            totalsRecord["Month"] = "TOTALS";

            foreach (var cat in categories.List())
            {
                try
                {
                    totalsRecord.Add(cat.Description, totalsPerCategory[cat.Description]);
                }
                catch { }
            }
            summary.Add(totalsRecord);


            return summary;
        }




        #endregion GetList

    }
}
