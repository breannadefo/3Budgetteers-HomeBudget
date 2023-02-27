﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml;
using System.Data.SQLite;
using System.Reflection.PortableExecutable;

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
        // ====================================================================
        // Constructor
        // ====================================================================

        /// <summary>
        /// Creates a new instance of the object. Calls a method to set everything to default values.
        /// </summary>
        public Categories(System.Data.SQLite.SQLiteConnection conn, bool resetDatabase)
        {
            if(resetDatabase == true)
            {
                ResetCategoryTypes();
                SetCategoriesToDefaults();
            }
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
            //Setting up the command and executing it
            SQLiteCommand sqlite_cmd;
            SQLiteDataReader sqlite_datareader;
            sqlite_cmd = Database.dbConnection.CreateCommand();
            sqlite_cmd.CommandText = "SELECT * FROM categories WHERE Id = @id";
            sqlite_cmd.Parameters.Add(new SQLiteParameter("@id", i));
            sqlite_datareader = sqlite_cmd.ExecuteReader();

            //Reads the row returned
            sqlite_datareader.Read();

            //Creates the category so that it can be returned
            Category category = new Category(sqlite_datareader.GetInt32(0), sqlite_datareader.GetString(1), (Category.CategoryType)sqlite_datareader.GetInt32(2));
            
            sqlite_datareader.Close();
            return category;
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
            //Getting rid of the old categories

            // ---------------------------------------------------------------
            // reset any current categories,
            // ---------------------------------------------------------------
            RemoveAllCategories();

            // ---------------------------------------------------------------
            // Add Defaults
            // ---------------------------------------------------------------
            AddAllCategories();
        }

        private void AddAllCategories()
        {
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

        /// <summary>
        /// Gets all the categories and removes them from the table.
        /// </summary>
        private void RemoveAllCategories()
        {
            List<Category> categoriesList = List();
            
            foreach(Category categoryItem in categoriesList)
            {
                Delete(categoryItem.Id);
            }
        }

        /// <summary>
        /// Removes all existing items in the categoryTypes table before adding default values to that table.
        /// </summary>
        /// <exception cref="SQLiteException">Thrown if there is a problem deleting any of the values from the table.</exception>
        public void ResetCategoryTypes()
        {
            try
            {
                SQLiteCommand cmd = new SQLiteCommand(Database.dbConnection);
                cmd = Database.dbConnection.CreateCommand();

                //deleting all existing category types
                cmd.CommandText = "DELETE FROM categoryTypes;";
                cmd.ExecuteNonQuery();

                //adding the category types into the table
                cmd.CommandText = "INSERT INTO categoryTypes (Id, Description) VALUES (1, 'Income');";
                cmd.ExecuteNonQuery();

                cmd.CommandText = "INSERT INTO categoryTypes (Id, Description) VALUES (2, 'Expense');";
                cmd.ExecuteNonQuery();

                cmd.CommandText = "INSERT INTO categoryTypes (Id, Description) VALUES (3, 'Credit');";
                cmd.ExecuteNonQuery();

                cmd.CommandText = "INSERT INTO categoryTypes (Id, Description) VALUES (4, 'Savings');";
                cmd.ExecuteNonQuery();

                cmd.Dispose();

            }
            catch (Exception e)
            {
                throw new SQLiteException();
            }
        }

        // ====================================================================
        // Add category
        // ====================================================================
        private void Add(Category cat)
        {
            //Connecting to the database
            SQLiteCommand sqlite_cmd;
            sqlite_cmd = Database.dbConnection.CreateCommand();

            //Writing the insert command for ID
            sqlite_cmd.CommandText = "INSERT INTO categories (Description, TypeId) VALUES (@Description, @Type);";
            sqlite_cmd.Parameters.Add(new SQLiteParameter("@Description", cat.Description));
            sqlite_cmd.Parameters.Add(new SQLiteParameter("@Type", (int)cat.Type));
            sqlite_cmd.ExecuteNonQuery();
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
            //Connecting to the database
            SQLiteCommand sqlite_cmd;
            sqlite_cmd = Database.dbConnection.CreateCommand();

            //Writing the insert command
            sqlite_cmd.CommandText = "INSERT INTO categories (Description, TypeId) VALUES (@Description, @Type);";
            sqlite_cmd.Parameters.Add(new SQLiteParameter("@Description", desc));
            sqlite_cmd.Parameters.Add(new SQLiteParameter("@Type", (int)type));
            sqlite_cmd.ExecuteNonQuery();
        }

        // ====================================================================
        // Delete category
        // ====================================================================

        /// <summary>
        /// Removes a category from the categories list. To do this, the category's id must be provided. Using the id, it
        /// searches for that Id in the databse and then deletes the row with that category.
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
            try
            {
                //Connecting to the database
                SQLiteCommand sqlite_cmd;
                sqlite_cmd = Database.dbConnection.CreateCommand();

                //Writing the Delete command
                sqlite_cmd.CommandText = "DELETE FROM categories WHERE Id=@Id";
                sqlite_cmd.Parameters.Add(new SQLiteParameter("@Id", Id));
                sqlite_cmd.ExecuteNonQuery();
            }
            catch(Exception ex)
            {
                if(ex is SQLiteException)
                {
                    throw new SQLiteException(ex.Message);
                }
            }
        }

        public void UpdateProperties(int idToUpdate, string newDescription, Category.CategoryType newType)
        {
            try
            {
                SQLiteConnection conn = Database.dbConnection;

                SQLiteCommand cmd = conn.CreateCommand();

                cmd.CommandText = "SELECT count(*) FROM categories WHERE Id = @id;";
                cmd.Parameters.Add(new SQLiteParameter("@id", idToUpdate));

                int count = Convert.ToInt32(cmd.ExecuteScalar());
                if (count == 0)
                {
                    throw new SQLiteException("Record to update could not be found with id: " + idToUpdate);
                }

                cmd.CommandText = "UPDATE categories SET Description = @newDescription, TypeId = @newTypeId WHERE Id = @id;";
                cmd.Parameters.Add(new SQLiteParameter("@newDescription", newDescription));
                cmd.Parameters.Add(new SQLiteParameter("@newTypeId", (int)newType));

                int updatedRows = cmd.ExecuteNonQuery();
                if(updatedRows == 0)
                {
                    throw new SQLiteException("No rows were updated.");
                }
                else if(updatedRows > 1)
                {
                    throw new SQLiteException("More than 1 rows were updated.");
                }
            }
            catch (Exception e)
            {
                if(e is SQLiteException)
                {
                    throw new SQLiteException(e.Message);
                }
                throw new Exception(e.Message);
            }
            //run it, read it, check if exists, bla bla bla
        }

        // ====================================================================
        // Return list of categories
        // Note:  make new copy of list, so user cannot modify what is part of
        //        this instance
        // ====================================================================

        /// <summary>
        /// Creates a list of all the categories in the database to return to the user. It creates a new list and gets
        /// everything from the categories table in the database. For each row in the database, the data is parsed and a new
        /// Category object is created and added to the list.
        /// 
        /// <example>
        /// Here's an example of how to use this method:
        /// 
        /// <code>
        /// 
        /// Categories cats = new Categories();
        /// <![CDATA[
        /// List<Category> categoriesList = cats.List();
        /// int firstNumberOfCategories = categoriesList.Count();
        /// 
        /// cats.Delete(3);
        /// cats.Delete(4);
        /// cats.Delete(7);
        /// 
        /// List<Category> secondCategoryList = cats.List();
        /// int secondNumberOfCategories = secondCategoriesList.Count();
        /// 
        /// if (firstNumberOfCategories != secondNumberOfCategories) {
        ///     Console.WriteLine("Categories have been removed from the database."); 
        /// }
        /// 
        /// ]]>
        /// </code>
        /// 
        /// Since categories were deleted from the database and the list of categories was retrieved after those deletions,
        /// the number of categories in the list will be smaller than before the deletions.
        /// 
        /// </example>
        /// 
        /// </summary>
        /// <returns>A list of all the categories from the categories table.</returns>
        public List<Category> List()
        {
            int idColumn = 0, descriptionColumn = 1, typeIdColumn = 2;
            List<Category> list = new List<Category>();

            SQLiteDataReader reader;
            SQLiteCommand cmd;
            cmd = Database.dbConnection.CreateCommand();

            cmd.CommandText = "SELECT Id, Description, TypeId FROM categories ORDER BY Id ASC;";

            reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                Category.CategoryType type;
                while (reader.Read())
                {
                    object tempId = reader.GetValue(idColumn);
                    string description = reader.GetString(descriptionColumn);
                    object tempEnum = reader.GetValue(typeIdColumn);

                    int convertedId = Convert.ToInt32(tempId);
                    int convertedEnum = Convert.ToInt32(tempEnum);
                    
                    if(convertedEnum == 1)
                    {
                        type = Category.CategoryType.Income;
                    }
                    else if(convertedEnum == 2)
                    {
                        type = Category.CategoryType.Expense;
                    }
                    else if(convertedEnum == 3)
                    {
                        type = Category.CategoryType.Credit;
                    }
                    else
                    {
                        type = Category.CategoryType.Savings;
                    }
                    
                    list.Add(new Category(convertedId, description, type)); 
                    
                }
            }

            reader.Close();

            return list;
        }


    }
}

