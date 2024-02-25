using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace WindowsFormsApp1
{
    internal class ProductFactory
    {
        private string xmlPath;
        List<Product> products;

        public ProductFactory(string pathToProductDb)
        {
            xmlPath = pathToProductDb;
            Products = new List<Product>();
            BuildProducts();
        }

        public List<Product> Products { get => products; set => products = value; }

        private void BuildProducts()
        {
            XmlTextReader reader = new XmlTextReader(xmlPath);
            while (reader.Read())
            {
                switch(reader.NodeType)
                {
                    case XmlNodeType.Element:
                        if(reader.Name == "Product")
                        {
                            Product prod = new Product
                            {
                                ProductCategory = reader.GetAttribute("productCategory"),
                                StatusCode = reader.GetAttribute("statusCode"),
                                ProductClass = reader.GetAttribute("productClass"),
                                Salable = bool.Parse(reader.GetAttribute("salable")),
                                Modified = bool.Parse(reader.GetAttribute("modified"))
                            };
                            XmlReader subReader = reader.ReadSubtree();
                            while(subReader.Read())
                            {
                                if(subReader.NodeType == XmlNodeType.Element)
                                {
                                    switch(subReader.Name)
                                    {
                                        case "ProductCode":
                                            prod.ProductCode = int.Parse(subReader.ReadElementContentAsString());
                                            break;
                                        case "CategoryMenus":
                                            XmlReader categorySubReader = subReader.ReadSubtree();
                                            List<int> catIDs = new List<int>();
                                            while(categorySubReader.Read())
                                            {
                                                if(categorySubReader.NodeType == XmlNodeType.Element && categorySubReader.Name == "CategoryMenu")
                                                {
                                                    // Move to the attribute "categoryID" and read its value
                                                    string categoryID = categorySubReader.GetAttribute("categoryID");

                                                    if (!string.IsNullOrEmpty(categoryID))
                                                    {
                                                        catIDs.Add(int.Parse(categoryID));
                                                    }
                                                }
                                            }
                                            prod.CategoryID = catIDs;
                                            break;
                                        case "Secondary":
                                            prod.Secondary = bool.Parse(subReader.ReadElementContentAsString());
                                            break;
                                        case "FamilyGroup":
                                            prod.FamilyGroup = subReader.ReadElementContentAsString();
                                            break;
                                        case "DayPartCode":
                                            prod.DayPartCode = subReader.ReadElementContentAsString();
                                            break;
                                        case "DummyProduct":
                                            prod.DummyProduct = bool.Parse(subReader.ReadElementContentAsString());
                                            break;
                                        case "ContainerVM":
                                            prod.ContainerVM = bool.Parse(subReader.ReadElementContentAsString());
                                            break;
                                        case "ProductUnit":
                                            prod.ProductUnit = subReader.ReadElementContentAsString();
                                            break;
                                        case "CSOHasLimitedTimeDiscount":
                                            prod.CSOHasLimitedTimeDiscount = bool.Parse(subReader.ReadElementContentAsString());
                                            break;
                                        case "DisplayOrder":
                                            prod.DisplayOrder = int.Parse(subReader.ReadElementContentAsString());
                                            break;
                                        case "SalesType":
                                            SalesType salesType = new SalesType();
                                            salesType.eatIn = bool.Parse(subReader.GetAttribute("eatin"));
                                            salesType.takeout = bool.Parse(subReader.GetAttribute("takeout"));
                                            salesType.other = bool.Parse(subReader.GetAttribute("other"));
                                            prod.SalesType = salesType;
                                            break;
                                        case "DisplayWaste":
                                            prod.DisplayWaste = bool.Parse(subReader.ReadElementContentAsString());
                                            break;
                                        case "Upsizable":
                                            prod.Upsizable = bool.Parse(subReader.ReadElementContentAsString());
                                            break;
                                        case "Distribution":
                                            XmlReader distSubReader = subReader.ReadSubtree();
                                            List<string> dist = new List<string>();
                                            while(distSubReader.Read())
                                            {
                                                if(distSubReader.NodeType == XmlNodeType.Element && distSubReader.Name == "Point")
                                                {
                                                    dist.Add(distSubReader.ReadElementContentAsString());
                                                }
                                            }
                                            prod.Distribution = dist;
                                            break;
                                        case "PriceList":
                                            XmlReader priceSubReader = subReader.ReadSubtree();
                                            while(priceSubReader.Read())
                                            {
                                                if(priceSubReader.NodeType == XmlNodeType.Element)
                                                {
                                                    string priceCode = reader.GetAttribute("priceCode");
                                                    reader.ReadToFollowing("Price");
                                                    // Check if the Price element is present
                                                    if (priceSubReader.NodeType == XmlNodeType.Element)
                                                    {
                                                        // Read the Price element content as a string
                                                        string priceString = priceSubReader.ReadElementContentAsString();

                                                        // Attempt to parse the string as a float
                                                        if (float.TryParse(priceString, out float price))
                                                        {
                                                            // Assign the parsed price based on priceCode
                                                            switch (priceCode)
                                                            {
                                                                case "EATIN":
                                                                    prod.PriceEatIn = price;
                                                                    break;
                                                                case "TAKEOUT":
                                                                    prod.PriceTakeout = price;
                                                                    break;
                                                                case "OTHER":
                                                                    prod.PriceOther = price;
                                                                    break;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            // Handle the case where parsing fails
                                                            //Console.WriteLine($"Failed to parse price for {priceCode}");
                                                        }
                                                    }
                                                }
                                            }
                                            break;
                                    }
                                }
                            }
                            
                            products.Add(prod);
                        }
                        break;
                }
            }
            
            reader.Close();
        }
    }
}
