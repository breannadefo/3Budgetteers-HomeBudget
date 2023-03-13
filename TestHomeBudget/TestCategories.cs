﻿using System;
using Xunit;
using System.IO;
using System.Collections.Generic;
using Budget;
using System.Data.SQLite;

namespace BudgetCodeTests
{
    [Collection("Sequential")]
    public class TestCategories
    {
        public int numberOfCategoriesInFile = TestConstants.numberOfCategoriesInFile;
        public String testInputFile = TestConstants.testDBInputFile;
        public int maxIDInCategoryInFile = TestConstants.maxIDInCategoryInFile;
        Category firstCategoryInFile = TestConstants.firstCategoryInFile;
        int IDWithSaveType = TestConstants.CategoryIDWithSaveType;

        // ========================================================================

        [Fact]
        public void CategoriesObject_New()
        {
            // Arrange
            String folder = TestConstants.GetSolutionDir();
            String newDB = $"{folder}\\newDB.db";
            Database.newDatabase(newDB);
            SQLiteConnection conn = Database.dbConnection;

            // Act
            Categories categories = new Categories(conn, true);

            // Assert 
            Assert.IsType<Categories>(categories);
        }

        // ========================================================================

        [Fact]
        public void CategoriesObject_New_CreatesDefaultCategories()
        {
            // Arrange
            String folder = TestConstants.GetSolutionDir();
            String newDB = $"{folder}\\newDB.db";
            Database.newDatabase(newDB);
            SQLiteConnection conn = Database.dbConnection;

            // Act
            Categories categories = new Categories(conn, true);

            // Assert 
            Assert.False(categories.List().Count == 0, "Non zero categories");

        }

        // ========================================================================

        //Modify to use new DB

        [Fact]
        public void CategoriesMethod_ReadFromDatabase_ValidateCorrectDataWasRead()
        {
            // Arrange
            String folder = TestConstants.GetSolutionDir();
            String newDB = $"{folder}\\newDB.db";
            Database.newDatabase(newDB);
            SQLiteConnection conn = Database.dbConnection;
            int numberOfDefaultCategories = 16;

            // Act
            Categories categories = new Categories(conn, true);
            List<Category> list = categories.List();
            Category firstCategory = list[0];

            // Assert
            Assert.Equal(numberOfDefaultCategories, list.Count);
            Assert.Equal(firstCategoryInFile.Id, firstCategory.Id);
            Assert.Equal(firstCategoryInFile.Description, firstCategory.Description);

        }

        // ========================================================================

        //Modify to use new DB

        [Fact]
        public void CategoriesMethod_List_ReturnsListOfCategories()
        {
            // Arrange
            String folder = TestConstants.GetSolutionDir();
            String newDB = $"{folder}\\newDB.db";
            Database.newDatabase(newDB);
            SQLiteConnection conn = Database.dbConnection;
            Categories categories = new Categories(conn, true);
            int numberOfDefaultCategories = 16;

            // Act
            List<Category> list = categories.List();

            // Assert
            Assert.Equal(numberOfDefaultCategories, list.Count);

        }

        // ========================================================================

        [Fact]
        public void CategoriesMethod_Add()
        {
            // Arrange
            String folder = TestConstants.GetSolutionDir();
            String goodDB = $"{folder}\\{TestConstants.testDBInputFile}";
            String messyDB = $"{folder}\\messy.db";
            System.IO.File.Copy(goodDB, messyDB, true);
            Database.existingDatabase(messyDB);
            SQLiteConnection conn = Database.dbConnection;
            Categories categories = new Categories(conn, false);
            string descr = "New Category";
            Category.CategoryType type = Category.CategoryType.Income;

            // Act
            List<Category> beforeAddList = categories.List();
            categories.Add(descr,type);
            List<Category> afterAddList = categories.List();
            int sizeOfList = afterAddList.Count;

            // Assert
            Assert.Equal(beforeAddList.Count + 1, sizeOfList);
            Assert.Equal(descr, afterAddList[sizeOfList - 1].Description);

        }

        // ========================================================================

        [Fact]
        public void CategoriesMethod_Delete()
        {
            // Arrange
            String folder = TestConstants.GetSolutionDir();
            String goodDB = $"{folder}\\{TestConstants.testDBInputFile}";
            String messyDB = $"{folder}\\messy.db";
            System.IO.File.Copy(goodDB, messyDB, true);
            Database.existingDatabase(messyDB);
            SQLiteConnection conn = Database.dbConnection;
            Categories categories = new Categories(conn, false);
            int IdToDelete = 3;

            // Act
            List<Category> beforeDeleteList = categories.List();
            categories.Delete(IdToDelete);
            List<Category> afterDeleteList = categories.List();
            int sizeOfList = afterDeleteList.Count;

            // Assert
            Assert.Equal(beforeDeleteList.Count - 1, sizeOfList);
            Assert.False(afterDeleteList.Exists(e => e.Id == IdToDelete), "correct Category item deleted");

        }

        // ========================================================================

        [Fact]
        public void CategoriesMethod_Update()
        {
            //Arrange
            String folder = TestConstants.GetSolutionDir();
            String goodDB = $"{folder}\\{TestConstants.testDBInputFile}";
            String messyDB = $"{folder}\\messy.db";
            System.IO.File.Copy(goodDB, messyDB, true);
            Database.existingDatabase(messyDB);
            SQLiteConnection conn = Database.dbConnection;
            Categories categories = new Categories(conn, false);
            int IdToUpdate = 3;
            string newDescription = "Test description";
            Category.CategoryType newType = Category.CategoryType.Credit;
            
            categories.UpdateProperties(IdToUpdate, newDescription, newType);

            List<Category> categoriesList = categories.List();
            //minus one since index is starting at 0 and ID starts at 1
            Category catToUpdate = categoriesList[IdToUpdate - 1];

            Assert.Equal(newDescription, catToUpdate.Description);
            Assert.Equal(newType, catToUpdate.Type);
        }

        // ========================================================================

        [Fact]
        public void CategoriesMethod_Delete_InvalidIDDoesntCrash()
        {
            // Arrange
            // Arrange
            String folder = TestConstants.GetSolutionDir();
            String goodDB = $"{folder}\\{TestConstants.testDBInputFile}";
            String messyDB = $"{folder}\\messyDB";
            System.IO.File.Copy(goodDB, messyDB, true);
            Database.existingDatabase(messyDB);
            SQLiteConnection conn = Database.dbConnection;
            Categories categories = new Categories(conn, false);
            int IdToDelete = 9999;
            int sizeOfList = categories.List().Count;

            // Act
            try
            {
                categories.Delete(IdToDelete);
                Assert.Equal(sizeOfList, categories.List().Count);
            }

            // Assert
            catch
            {
                Assert.True(false, "Invalid ID causes Delete to break");
            }
        }

        // ========================================================================

        [Fact]
        public void CategoriesMethod_GetCategoryFromId()
        {
            // Arrange
            String folder = TestConstants.GetSolutionDir();
            String newDB = $"{folder}\\{TestConstants.testDBInputFile}";
            Database.existingDatabase(newDB);
            SQLiteConnection conn = Database.dbConnection;
            Categories categories = new Categories(conn, false);
            int catID = 15;

            // Act
            Category category = categories.GetCategoryFromId(catID);

            // Assert
            Assert.Equal(catID,category.Id);

        }

        // ========================================================================

        [Fact]
        public void CategoriesMethod_SetCategoriesToDefaults()
        {

            // Arrange
            String folder = TestConstants.GetSolutionDir();
            String newDB = $"{folder}\\newDB.db";
            Database.newDatabase(newDB);
            SQLiteConnection conn = Database.dbConnection;

            // Act
            Categories categories = new Categories(conn, true);
            List<Category> originalList = categories.List();

            // modify list of categories
            categories.Delete(1);
            categories.Delete(2);
            categories.Delete(3);
            categories.Add("Another one ", Category.CategoryType.Credit);

            //"just double check that initial conditions are correct");
            Assert.NotEqual(originalList.Count, categories.List().Count);

            // Act
            categories.SetCategoriesToDefaults();

            // Assert
            Assert.Equal(originalList.Count, categories.List().Count);
            foreach (Category defaultCat in originalList)
            {
                Assert.True(categories.List().Exists(c => c.Description == defaultCat.Description && c.Type == defaultCat.Type));
            }

        }

        // ========================================================================
        
        [Fact]
        public void CategoriesMethod_UpdateCategory()
        {
            // Arrange
            String folder = TestConstants.GetSolutionDir();
            String newDB = $"{folder}\\newDB.db";
            Database.newDatabase(newDB);
            SQLiteConnection conn = Database.dbConnection;
            Categories categories = new Categories(conn, true);
            String newDescr = "Presents";
            int id = 11;

            // Act
            categories.UpdateProperties(id,newDescr, Category.CategoryType.Income);
            Category category = categories.GetCategoryFromId(id);

            // Assert 
            Assert.Equal(newDescr, category.Description);
            Assert.Equal(Category.CategoryType.Income, category.Type);

        }

        // ========================================================================

        [Fact]
        public void CategoriesMethod_ResetCategoryTypes_ErrorIfCategoriesStillExist()
        {
            // Arrange
            String folder = TestConstants.GetSolutionDir();
            String newDB = $"{folder}\\newDB.db";
            Database.newDatabase(newDB);
            SQLiteConnection conn = Database.dbConnection;
            Categories categories = new Categories(conn, true);
            bool resetThrows = false;

            // Act
            categories.SetCategoriesToDefaults();
            
            try
            {
                categories.ResetCategoryTypes();
            }
            catch (Exception ex)
            {
                resetThrows = true;
            }
            
            // Assert
            Assert.True(resetThrows);
        }
    }
}

