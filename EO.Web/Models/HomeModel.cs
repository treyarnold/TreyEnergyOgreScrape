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
        public List<Rate> Rates = new List<Rate>();
        public bool empty = true;

        public class Rate
        {
            public string Product { get; set; }
            public string Term { get; set; }
            public string RatePerKWH { get; set; }
            public List<string> PromotionDesc = new List<string>();
            public string EnrollURL { get; set; }
            public string FactsURL { get; set; }
        }

        public void GetRates()
        {
            
            ChromeOptions option = new ChromeOptions();
            option.AddArgument("--headless");

            using (var driver = new ChromeDriver(option))
            {
                driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(20);

                driver.Navigate().GoToUrl("https://www.firstchoicepower.com/");

                var zipCodeField = driver.FindElementByClassName("zipcode");
                var searchButton = driver.FindElementByClassName("submitOrder");

                zipCodeField.SendKeys(Zipcode);

                searchButton.Click();


                for (var i = 1; i <= 3; i++)
                {
                    for (var j = 1; j <= 3; j++)
                    {
                        Rate currentRate = new Rate();
                        currentRate.RatePerKWH = driver.FindElementByXPath($"//*[@id=\"tab_11\"]/div[{i}]/div[{j}]/div/div/div[2]/h4/span").GetAttribute("innerText");
                        currentRate.Product = driver.FindElementByXPath($"//*[@id=\"tab_11\"]/div[{i}]/div[{j}]/div/div/h3/span").GetAttribute("innerText");

                        for (var x = 1; x <= 2; x++)
                        {
                            string description = driver.FindElementByXPath($"//*[@id=\"tab_11\"]/div[{i}]/div[{j}]/div/div/ul/li[{x}]/div").GetAttribute("innerText");
                            currentRate.PromotionDesc.Add(description);
                        }
                                                
                        if (currentRate.Product[0] == 'Y')
                        {
                            currentRate.Term = currentRate.Product.Substring(currentRate.Product.Length - 2);
                        }
                        else if (currentRate.Product[0] == 'M')
                        {
                            currentRate.Term = "Month to Month";
                        }
                        else
                        {
                            currentRate.Term = "Prepaid";
                        }
                        
                        //currentRate.Term = "0";
                        currentRate.EnrollURL = "#";
                        currentRate.FactsURL = driver.FindElementByXPath($"//*[@id=\"tab_11\"]/div[{i}]/div[{j}]/div/div/div[3]/a[3]").GetAttribute("href");
                        Rates.Add(currentRate);
                    }
                }
                empty = false;                
            }
        }

        public HomeModel ParseRates(HomeModel model)
        {
            return model;
        }
    }
}
