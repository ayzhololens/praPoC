using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity;

public class violationsLib : Singleton<violationsLib> {

    //h
    public Dictionary<int, string> violationsCategory = new Dictionary<int, string>();
    public Dictionary<int, string> violationsSubCategory4 = new Dictionary<int, string>();
    public Dictionary<int, string> violationsSpecific41 = new Dictionary<int, string>();
    public Dictionary<int, string> violationsSeverity = new Dictionary<int, string>();
    public Dictionary<int, string> violationsStatus =new Dictionary<int, string>();

    public CategoryContainer categoryLib = new CategoryContainer();

    [System.Serializable]
    public class CategoryContainer
    {
        public Dictionary<int, Category> categoryList = new Dictionary<int, Category>();
    }

    [System.Serializable]
    public class Category
    {
        public string name;
        public Dictionary<int, SubCategory> subCategoryList = new Dictionary<int, SubCategory>();
    }

    [System.Serializable]
    public class SubCategory
    {
        public string name;
        public Dictionary<int, Specific> specificList = new Dictionary<int, Specific>();
    }

    [System.Serializable]
    public class Specific
    {
        public string name;
    }

    private void Start()
    {
        defineViolationsDicts();
    }

    void defineViolationsDicts()
    {
        //severity
        violationsSeverity.Add(0, "minor");
        violationsSeverity.Add(1, "moderate");
        violationsSeverity.Add(2, "severe");

        //status
        violationsStatus.Add(0, "resolved");
        violationsStatus.Add(1, "not resolved");
        violationsStatus.Add(2, "other");

        //category
        violationsCategory.Add(1, "Boiler Controls");
        violationsCategory.Add(2, "Boiler Piping");
        violationsCategory.Add(3, "Boiler Mdr");
        violationsCategory.Add(4, "Boiler Components");
        violationsCategory.Add(5, "Boiler Press Relief Device");
        violationsCategory.Add(6, "P. Vessels");
        violationsCategory.Add(7, "R/A");

        foreach (int cat in violationsCategory.Keys)
        {
            Category tempCategory = new Category();
            tempCategory.name = violationsCategory[cat];
            categoryLib.categoryList.Add(cat, tempCategory);
        }

        //subCategory4
        violationsSubCategory4.Add(1, "Water Leaks");
        violationsSubCategory4.Add(2, "Baffles/refactory");
        violationsSubCategory4.Add(3, "Furnace");
        violationsSubCategory4.Add(4, "Waterside");
        violationsSubCategory4.Add(5, "Superheaters");
        violationsSubCategory4.Add(6, "Economizer");
        violationsSubCategory4.Add(7, "Installation");
        violationsSubCategory4.Add(8, "Undefined");

        foreach (int cat in violationsSubCategory4.Keys)
        {
            SubCategory tempSubCategory4 = new SubCategory();
            tempSubCategory4.name = violationsSubCategory4[cat];
            categoryLib.categoryList[4].subCategoryList.Add(cat, tempSubCategory4);
        }

        //specific
        violationsSpecific41.Add(1, "Handhole or gasket or gasket seat installation is not satisfactory");
        violationsSpecific41.Add(2, "Head is not installed or installed incorrectly or inoperable or damaged or leaking.");
        violationsSpecific41.Add(3, "Shell is not installed or installed incorrectly or inoperable or damaged or leaking.");
        violationsSpecific41.Add(4, "Tubesheet is not installed or installed incorrectly or inoperable or damaged or leaking");
        violationsSpecific41.Add(5, "Tube/tube ends is not installed or installed incorrectly or inoperable or damaged or leaking");
        violationsSpecific41.Add(6, "Manway gasket is not installed or installed incorrectly or inoperable or damaged or leaking");

        foreach (int cat in violationsSpecific41.Keys)
        {
            Specific tempSpecific41 = new Specific();
            tempSpecific41.name = violationsSpecific41[cat];
            categoryLib.categoryList[4].subCategoryList[1].specificList.Add(cat, tempSpecific41);
        }
    }

    private void Update()
    {

    }
}
