using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MojoIndia.Models
{
    public class StaticWebsiteData
    {
        static StaticWebsiteData()
        {
            dayData = new List<SelectListItem>();
            dayData.Add(new SelectListItem() { Text = "Day", Value = "" });
            for (int i = 1; i <= 31; i++)
            {
                dayData.Add(new SelectListItem() { Text = i.ToString(), Value = i.ToString() });
            }

            monthData = new List<SelectListItem>();
            monthData.Add(new SelectListItem() { Text = "Month", Value = "" });

            monthData.Add(new SelectListItem() { Text = "January", Value = "1" });
            monthData.Add(new SelectListItem() { Text = "February", Value = "2" });
            monthData.Add(new SelectListItem() { Text = "March", Value = "3" });
            monthData.Add(new SelectListItem() { Text = "April", Value = "4" });
            monthData.Add(new SelectListItem() { Text = "May", Value = "5" });
            monthData.Add(new SelectListItem() { Text = "June", Value = "6" });
            monthData.Add(new SelectListItem() { Text = "July", Value = "7" });
            monthData.Add(new SelectListItem() { Text = "August", Value = "8" });
            monthData.Add(new SelectListItem() { Text = "September", Value = "9" });
            monthData.Add(new SelectListItem() { Text = "October", Value = "10" });
            monthData.Add(new SelectListItem() { Text = "November", Value = "11" });
            monthData.Add(new SelectListItem() { Text = "December", Value = "12" });

        }
        public static List<SelectListItem> dayData { get; set; }
        public static List<SelectListItem> monthData { get; set; }
        public static List<SelectListItem> getDay(string id)
        {
            id = string.IsNullOrEmpty(id) ? "" : id;
            List<SelectListItem> day = new List<SelectListItem>();
            foreach (var item in dayData)
            {
                if (item.Value.ToLower() == id.ToLower())
                {
                    item.Selected = true;
                }
                day.Add(item);
            }
            return day;
        }
        public static List<SelectListItem> getMonth(string id)
        {
            id = string.IsNullOrEmpty(id) ? "" : id;
            List<SelectListItem> month = new List<SelectListItem>();
            foreach (var item in monthData)
            {
                if (item.Value.ToLower() == id.ToLower())
                {
                    item.Selected = true;
                }
                month.Add(item);
            }
            return month;
        }
        public static List<SelectListItem> getYear(string id,Core.PassengerType paxType)
        {
            id = string.IsNullOrEmpty(id) ? "" : id;
            List<SelectListItem> year = new List<SelectListItem>();
            int minDate = DateTime.Today.AddYears(-100).Year;
            int maxDate = DateTime.Today.AddYears(-10).Year;
            if (paxType == Core.PassengerType.Child)
            {
                minDate = DateTime.Today.AddYears(-13).Year;
                maxDate = DateTime.Today.AddYears(-1).Year;
            }
            else if (paxType == Core.PassengerType.Infant || paxType == Core.PassengerType.InfantWs)
            {
                minDate = DateTime.Today.AddYears(-3).Year;
                maxDate = DateTime.Today.Year;
            }
            for (int i = maxDate; i >= minDate; i--)
            {
                 year.Add(new SelectListItem() { Text = i.ToString(), Value = i.ToString() });
            }
            List<SelectListItem> yearData = new List<SelectListItem>();
            foreach (var item in year)
            {
                if (item.Value.ToLower() == id.ToLower())
                {
                    item.Selected = true;
                }
                yearData.Add(item);
            }
            return yearData;
        }
        public static List<SelectListItem> getYear(string id, Core.PassengerType paxType, DateTime travelDate)
        {
            id = string.IsNullOrEmpty(id) ? "" : id;
            List<SelectListItem> year = new List<SelectListItem>();
            int minDate = travelDate.AddYears(-100).Year;
            int maxDate = travelDate.AddYears(-11).Year;
            if (paxType == Core.PassengerType.Child)
            {
                minDate = travelDate.AddYears(-12).Year;
                maxDate = travelDate.AddYears(-1).Year;
            }
            else if (paxType == Core.PassengerType.Infant || paxType == Core.PassengerType.InfantWs)
            {
                minDate = travelDate.AddYears(-3).Year;
                maxDate = travelDate.Year;
            }
            for (int i = maxDate; i >= minDate; i--)
            {
                year.Add(new SelectListItem() { Text = i.ToString(), Value = i.ToString() });
            }
            List<SelectListItem> yearData = new List<SelectListItem>();
            foreach (var item in year)
            {
                if (item.Value.ToLower() == id.ToLower())
                {
                    item.Selected = true;
                }
                yearData.Add(item);
            }
            return yearData;
        }
        public static List<SelectListItem> getExpPassYear(string id)
        {
            List<SelectListItem> yearData = new List<SelectListItem>();
            int exYear = DateTime.Today.Year;
            for (int i = 0; i < 30; i++)
            {
                yearData.Add(new SelectListItem() { Text = (i + exYear).ToString(), Value = (i + exYear).ToString() });
            }
            return yearData;


            //List<SelectListItem> yearData = new List<SelectListItem>();
            ////   int exYear = DateTime.Today.Year;travelDate.AddYears(-11).Year;
            //int exYear = DateTime.Today.AddYears(-12).Year;
            //for (int i = 0; i <30; i++)
            //{
            //    yearData.Add(new SelectListItem() { Text = (i + exYear).ToString(), Value = (i + exYear).ToString() });
            //}
            //return yearData;

        }
        public static List<SelectListItem> getCountry(string id)
        {
            Core.CountryList cl = new Core.CountryList();
            id = string.IsNullOrEmpty(id) ? "" : id;
            List<SelectListItem> month = new List<SelectListItem>();
            foreach (var country in cl.country)
            {
                SelectListItem sli = new SelectListItem() { Value = country.code, Text = country.name };
                if (sli.Value.ToLower() == id.ToLower() && sli.Value!="")
                {
                    sli.Selected = true;
                }
                if (sli.Text == "----------")
                {
                    sli.Disabled = true;
                }
                month.Add(sli);
            }
            return month;
        }


        public static List<SelectListItem> getIssuePassYear(string id, DateTime travelDate)
        {
            List<SelectListItem> yearData = new List<SelectListItem>();
            // int IssueYear = DateTime.Today.AddYears(-1).Year;
            int IssueYear = travelDate.AddYears(-20).Year;
            for (int i = 0; i < 20; i++)
            {
                yearData.Add(new SelectListItem() { Text = (i + IssueYear).ToString(), Value = (i + IssueYear).ToString() });
            }
            return yearData;


            //List<SelectListItem> yearData = new List<SelectListItem>();
            ////   int exYear = DateTime.Today.Year;travelDate.AddYears(-11).Year;
            //int exYear = DateTime.Today.AddYears(-12).Year;
            //for (int i = 0; i <30; i++)
            //{
            //    yearData.Add(new SelectListItem() { Text = (i + exYear).ToString(), Value = (i + exYear).ToString() });
            //}
            //return yearData;

        }
    }
}