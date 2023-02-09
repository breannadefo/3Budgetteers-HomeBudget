using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml;

// ============================================================================
// (c) Sandy Bultena 2018
// * Released under the GNU General Public License
// ============================================================================

namespace Budget
{
    // ====================================================================
    // CLASS: categories
    //        - A collection of category items,
    //        - Read / write to file
    //        - etc
    // ====================================================================

    /// <summary>
    /// Used to hold collections of Category objects and to read/write from files. It contains a default file name, the list
    /// that holds all the category items, a file name, and a directory name. It has methods to manage the list of categories
    /// and some to read/write to files that work specifically with getting or saving the information about the categories. 
    /// Only the default file name is static, so to access anything else in the class, an instance must be created first.
    /// </summary>
    public class Categories
    {
        private static String DefaultFileName = "budgetCategories.txt";
        private List<Category> _Cats = new List<Category>();
        private string _FileName;
        private string _DirName;

        // ====================================================================
        // Properties
        // ====================================================================

        /// <value>
        /// Returns the value of the file name backing field.
        /// </value>
        public String FileName { get { return _FileName; } }

        /// <value>
        /// Returns the value of the directory name backing field.
        /// </value>
        public String DirName { get { return _DirName; } }

        // ====================================================================
        // Constructor
        // ====================================================================

        /// <summary>
        /// Creates a new instance of the object. Calls a method to set everything to default values.
        /// </summary>
        public Categories()
        {
            SetCategoriesToDefaults();
        }

        // ====================================================================
        // get a specific category from the list where the id is the one specified
        // ====================================================================

        /// <summary>
        /// Returns the Category object, from the categories list, whose id matches the id that is passed in as a parameter. 
        /// 
        /// <example>
        /// Here is an example of how this is used:
        /// 
        /// When a new categories object is created, it comes with 16 default categories. This means that if the following commands
        /// are run:
        /// <code>
        /// 
        /// Categories c = new Categories();
        /// Console.Write(c.GetCategoryFromId(1));
        /// 
        /// </code>
        /// 
        /// The expected outcome should be:
        /// 
        /// <code>
        /// Utilities
        /// </code>
        /// </example>
        /// 
        /// </summary>
        /// <param name="i">Represents the category id of the category that should be retrieved.</param>
        /// <returns>The Category object that has the same id as the parameter.</returns>
        /// <exception cref="Exception">Thrown when the parameter value does not match with any of the existing categories.</exception>
        public Category GetCategoryFromId(int i)
        {
            Category c = _Cats.Find(x => x.Id == i);
            if (c == null)
            {
                throw new Exception("Cannot find category with id " + i.ToString());
            }
            return c;
        }

        // ====================================================================
        // populate categories from a file
        // if filepath is not specified, read/save in AppData file
        // Throws System.IO.FileNotFoundException if file does not exist
        // Throws System.Exception if cannot read the file correctly (parsing XML)
        // ====================================================================

        /// <summary>
        /// Prepares to get category information from a file. It clears the current categories and file/directory names, gets a
        /// verified file path using a method from the BudgetFiles class, then calls a different method to actually read and 
        /// get the new category informaiton. It takes a file name as a parameter, but if none are provided, it defaults it 
        /// to null. This means that if no file path is provided, it will use a default file path and file name.
        /// 
        /// <example>
        /// Here is an example of how to use this method:
        /// 
        /// <code>
        /// 
        /// Categories cats = new Categories();
        /// 
        /// </code>
        /// 
        /// A basic Categories object is created, but there is a file that is full of categories that should be used in the program.
        /// Here is how to get those categories into the object above:
        /// 
        /// <code>
        /// 
        /// string categoriesFileName = "./categoriesToUse.cats";
        /// 
        /// cats.ReadFromFile(categoriesFileName);
        /// 
        /// </code>
        /// 
        /// After this has run, all the categories will be the ones from the file instead of the default ones that come with the 
        /// default constructor.
        /// 
        /// </example>
        /// </summary>
        /// <param name="filepath">The file path of the file that will be read.</param>
        /// <exception cref="FileNotFoundException">Thrown when a valid file path cannot be found or created.</exception>
        /// <exception cref="Exception">Thrown when the file cannot be read.</exception>
        public void ReadFromFile(String filepath = null)
        {

            // ---------------------------------------------------------------
            // reading from file resets all the current categories,
            // ---------------------------------------------------------------
            _Cats.Clear();

            // ---------------------------------------------------------------
            // reset default dir/filename to null 
            // ... filepath may not be valid, 
            // ---------------------------------------------------------------
            _DirName = null;
            _FileName = null;

            // ---------------------------------------------------------------
            // get filepath name (throws exception if it doesn't exist)
            // ---------------------------------------------------------------
            filepath = BudgetFiles.VerifyReadFromFileName(filepath, DefaultFileName);

            // ---------------------------------------------------------------
            // If file exists, read it
            // ---------------------------------------------------------------
            _ReadXMLFile(filepath);
            _DirName = Path.GetDirectoryName(filepath);
            _FileName = Path.GetFileName(filepath);
        }

        // ====================================================================
        // save to a file
        // if filepath is not specified, read/save in AppData file
        // ====================================================================

        /// <summary>
        /// Prepares to write category information to a file. It prepares the file path if one was not passed in as a 
        /// parameter, then checks to make sure that file path is a valid one using a method from the BudgetFiles class
        /// before calling a method to actually write the information to a file.
        /// 
        /// <example>
        /// Here is an example of how to use this method:
        /// 
        /// <code>
        /// 
        /// string categoriesFileName = "./savingCategories.cats";
        /// 
        /// Categories cats = new Categories();
        /// cats.Add("Reffing", Category.CategoryType.Income);
        /// cats.Add("Bursary", Category.CategoryType.Income);
        /// 
        /// cats.SaveToFile(categoriesFileName);
        /// 
        /// </code>
        /// </example>
        /// </summary>
        /// <param name="filepath">The file path of the file that will be written to.</param>
        /// <exception cref="Exception">Thrown when file doesn't exist or is read only.</exception>
        public void SaveToFile(String filepath = null)
        {
            // ---------------------------------------------------------------
            // if file path not specified, set to last read file
            // ---------------------------------------------------------------
            if (filepath == null && DirName != null && FileName != null)
            {
                filepath = DirName + "\\" + FileName;
            }

            // ---------------------------------------------------------------
            // just in case filepath doesn't exist, reset path info
            // ---------------------------------------------------------------
            _DirName = null;
            _FileName = null;

            // ---------------------------------------------------------------
            // get filepath name (throws exception if it doesn't exist)
            // ---------------------------------------------------------------
            filepath = BudgetFiles.VerifyWriteToFileName(filepath, DefaultFileName);

            // ---------------------------------------------------------------
            // save as XML
            // ---------------------------------------------------------------
            _WriteXMLFile(filepath);

            // ----------------------------------------------------------------
            // save filename info for later use
            // ----------------------------------------------------------------
            _DirName = Path.GetDirectoryName(filepath);
            _FileName = Path.GetFileName(filepath);
        }

        // ====================================================================
        // set categories to default
        // ====================================================================

        /// <summary>
        /// Clears the current categories list before populating the list with default categories. These values are hardcoded
        /// into the class.
        /// 
        /// <example>
        /// Here is an example of how to use this method:
        /// 
        /// <code>
        /// 
        /// Categories cats = new Categories();
        /// 
        /// cats.Add("Sports Equipment", Category.CategoryType.Expense);
        /// cats.Add("Sports Registration", Category.CategoryType.Credit);
        /// cats.Delete(2);
        /// cats.Delete(5);
        /// 
        /// </code>
        /// 
        /// After making all these modifications, it turns out the program needed the default categories to work. This method 
        /// allows an easy way to get to that.
        /// 
        /// <code>
        /// 
        /// cats.SetCategoriesToDefualt();
        /// 
        /// </code>
        /// </example>
        /// </summary>
        public void SetCategoriesToDefaults()
        {
            // ---------------------------------------------------------------
            // reset any current categories,
            // ---------------------------------------------------------------
            _Cats.Clear();

            // ---------------------------------------------------------------
            // Add Defaults
            // ---------------------------------------------------------------
            Add("Utilities", Category.CategoryType.Expense);
            Add("Rent", Category.CategoryType.Expense);
            Add("Food", Category.CategoryType.Expense);
            Add("Entertainment", Category.CategoryType.Expense);
            Add("Education", Category.CategoryType.Expense);
            Add("Miscellaneous", Category.CategoryType.Expense);
            Add("Medical Expenses", Category.CategoryType.Expense);
            Add("Vacation", Category.CategoryType.Expense);
            Add("Credit Card", Category.CategoryType.Credit);
            Add("Clothes", Category.CategoryType.Expense);
            Add("Gifts", Category.CategoryType.Expense);
            Add("Insurance", Category.CategoryType.Expense);
            Add("Transportation", Category.CategoryType.Expense);
            Add("Eating Out", Category.CategoryType.Expense);
            Add("Savings", Category.CategoryType.Savings);
            Add("Income", Category.CategoryType.Income);

        }

        // ====================================================================
        // Add category
        // ====================================================================
        private void Add(Category cat)
        {
            _Cats.Add(cat);
        }

        /// <summary>
        /// Creates and adds a new category to the categories list. To create a new category, it needs an id, a description,
        /// and a category type. The description and type are passed in as parameters, while the id is calculated based on 
        /// the list's current highest id. After the id is calculated, the category is created and immediately added to the
        /// categories list.
        /// 
        /// <example>
        /// Here is an example of how to use the method:
        /// <code>
        /// Categories c = new Categories();
        /// c.Add("Test Grading", Category.CategoryType.Income);
        /// </code>
        /// This creates a new Categories object which gets 16 default categories. After the Add method is run, c would contain
        /// a list of 17 categories, with the last one being the "test" one that was just created.
        /// </example>
        /// </summary>
        /// <param name="desc">Represents the description of the new category.</param>
        /// <param name="type">Represents the category type of the new category.</param>
        public void Add(String desc, Category.CategoryType type)
        {
            int new_num = 1;
            if (_Cats.Count > 0)
            {
                new_num = (from c in _Cats select c.Id).Max();
                new_num++;
            }
            _Cats.Add(new Category(new_num, desc, type));
        }

        // ====================================================================
        // Delete category
        // ====================================================================

        /// <summary>
        /// Removes a category from the categories list. To do this, the category's id must be provided. Using the id, it
        /// finds the index of that category and removes the category at that index from the list.
        /// 
        /// <example>
        /// Here is an example of how to use this method:
        /// <code>
        /// Categories c = new Categories();
        /// c.Delete(1);
        /// </code>
        /// The Categories constructor creates a new object with 16 default categories. After calling the Delete method, only
        /// 15 will remain since the category with the id of 1 would have been removed from the list.
        /// </example>
        /// </summary>
        /// <param name="Id">Represents the category id of the category that will be removed.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when provided id does not match with an existing 
        /// category's id.</exception>
        public void Delete(int Id)
        {
            int i = _Cats.FindIndex(x => x.Id == Id);
            _Cats.RemoveAt(i);
        }

        // ====================================================================
        // Return list of categories
        // Note:  make new copy of list, so user cannot modify what is part of
        //        this instance
        // ====================================================================

        /// <summary>
        /// Creates a copy of the categories list to return to the user. It creates a new list and copies each category into
        /// the new list. Since lists are passed by reference, this is done so that the user cannot modify the categories 
        /// list without using the Add and Delete methods.
        /// 
        /// <example>
        /// Here's an example of how to use this method:
        /// 
        /// <code>
        /// 
        /// Categories cats = new Categories();
        /// <![CDATA[
        /// List<Category> copyOfList = cats.List();
        /// 
        /// copyOfList.RemoveAt(3);
        /// copyOfList.RemoveAt(4);
        /// copyOfList.RemoveAt(7);
        /// ]]>
        /// </code>
        /// 
        /// Since its a copy, none of those RemoveAt methods will change any of the instance categories.
        /// 
        /// </example>
        /// 
        /// </summary>
        /// <returns>A copy of the categories list.</returns>
        public List<Category> List()
        {
            List<Category> newList = new List<Category>();
            foreach (Category category in _Cats)
            {
                newList.Add(new Category(category));
            }
            return newList;
        }

        // ====================================================================
        // read from an XML file and add categories to our categories list
        // ====================================================================
        private void _ReadXMLFile(String filepath)
        {

            // ---------------------------------------------------------------
            // read the categories from the xml file, and add to this instance
            // ---------------------------------------------------------------
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(filepath);

                foreach (XmlNode category in doc.DocumentElement.ChildNodes)
                {
                    String id = (((XmlElement)category).GetAttributeNode("ID")).InnerText;
                    String typestring = (((XmlElement)category).GetAttributeNode("type")).InnerText;
                    String desc = ((XmlElement)category).InnerText;

                    Category.CategoryType type;
                    switch (typestring.ToLower())
                    {
                        case "income":
                            type = Category.CategoryType.Income;
                            break;
                        case "expense":
                            type = Category.CategoryType.Expense;
                            break;
                        case "credit":
                            type = Category.CategoryType.Credit;
                            break;
                        default:
                            type = Category.CategoryType.Expense;
                            break;
                    }
                    this.Add(new Category(int.Parse(id), desc, type));
                }

            }
            catch (Exception e)
            {
                throw new Exception("ReadXMLFile: Reading XML " + e.Message);
            }

        }


        // ====================================================================
        // write all categories in our list to XML file
        // ====================================================================
        private void _WriteXMLFile(String filepath)
        {
            try
            {
                // create top level element of categories
                XmlDocument doc = new XmlDocument();
                doc.LoadXml("<Categories></Categories>");

                // foreach Category, create an new xml element
                foreach (Category cat in _Cats)
                {
                    XmlElement ele = doc.CreateElement("Category");
                    XmlAttribute attr = doc.CreateAttribute("ID");
                    attr.Value = cat.Id.ToString();
                    ele.SetAttributeNode(attr);
                    XmlAttribute type = doc.CreateAttribute("type");
                    type.Value = cat.Type.ToString();
                    ele.SetAttributeNode(type);

                    XmlText text = doc.CreateTextNode(cat.Description);
                    doc.DocumentElement.AppendChild(ele);
                    doc.DocumentElement.LastChild.AppendChild(text);

                }

                // write the xml to FilePath
                doc.Save(filepath);

            }
            catch (Exception e)
            {
                throw new Exception("_WriteXMLFile: Reading XML " + e.Message);
            }

        }

    }
}

