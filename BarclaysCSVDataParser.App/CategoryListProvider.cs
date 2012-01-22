using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BarclaysCSVDataParser.App
{
    public static class CategoryListProvider
    {
        static List<string> _categories = new List<string>();

        static CategoryListProvider()
        {
            _categories = new List<string>()
            {
                "Other",
                "Food shopping",
                "Takeout/Eating out", 
                "Car - Fuel",
                "Home Maintenance",
                "Clothing",
                "Birthdays/Christmas",

                "Income - Investment",
                "Income - Child Support",
                "Income - Salary",
                "Income - Reimbursement,/Refunds",
                "Income - Other",

                "Bills - Mortgage",
                "Bills - Cable TV",
                "Bills - Electricity and Gas",
                "Bills - Electricity",
                "Bills - Gas",
                "Bills - Water",
                "Bills - Mobile Phone",
                "Bills - Debt/Credit Cards",
                "Bills - TV Licence",
                "Bills - Council Tax",
                "Car - Maintenance",
                "Dental",
                "Eyecare",
                "Prescriptions",
                "Furnishing",
                "Insurance - Car",
                "Insurance - Health",
                "Insurance - Home",
                "Insurance - Life",
                "Insurance - Phone",
                "Cinemas",
                "DVD Rental",
                "Savings",
                "Jess Savings",
                "CASH",
                "Payment to Abbi"
                
            };
        }

        public static List<string> GetCategoryList()
        {
            return _categories;
        }
    }
}
