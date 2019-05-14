using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace EO.Web.Models
{
    public class HomeModel
    {
        public string Zipcode { get; set; }
        public string results;
        public List<Rate> Rates;

        public class Rate
        {
            public string Product { get; set; }
            public int Term { get; set; }
            public string RatePerKWH { get; set; }
            public List<string> PromotionDesc { get; set; }
            public string EnrollURL { get; set; }
            public string FactsURL { get; set; }
        }

        public void GetRates()
        {
            ChromeOptions option = new ChromeOptions();
            option.AddArgument("--headless");

            using (var driver = new ChromeDriver(option))
            {
                driver.Navigate().GoToUrl("https://www.firstchoicepower.com/");

                var zipCodeField = driver.FindElementByClassName("zipcode");
                var searchButton = driver.FindElementByClassName("submitOrder");

                zipCodeField.SendKeys(Zipcode);

                searchButton.Click();

                var allPlans = driver.FindElementByXPath("//*[@id=\"tab_11\"]");
                var flippers = allPlans.FindElements(By.ClassName("flipper")).ToList();

                foreach (var card in flippers)
                {
                    Rate currentRate = new Rate();
                    currentRate.RatePerKWH = card.FindElement(By.ClassName("price-container")).FindElement(By.TagName("span")).Text;
                    currentRate.Product = card.FindElement(By.ClassName("gridDescription")).Text;
                    card.FindElements(By.ClassName("bullet-hover")).ToList().ForEach(item =>
                    {
                        string description = item.Text;
                        currentRate.PromotionDesc.Add(description);
                    });
                    currentRate.EnrollURL = "#";
                    currentRate.Term = 0;
                    currentRate.FactsURL = card.FindElement(By.ClassName("efl_label")).GetAttribute("href");
                    Rates.Add(currentRate);                    
                }
            }
        }

        public HomeModel ParseRates(HomeModel model)
        {
            return model;
        }
    }
}
