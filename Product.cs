using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    public class Product
    {
		private bool displayWaste;

		public bool DisplayWaste
		{
			get { return displayWaste; }
			set { displayWaste = value; }
		}


		private int displayOrder;

		public int DisplayOrder
		{
			get { return displayOrder; }
			set { displayOrder = value; }
		}


		private bool csoHasLimitedTimeDiscount;

		public bool CSOHasLimitedTimeDiscount
		{
			get { return csoHasLimitedTimeDiscount; }
			set { csoHasLimitedTimeDiscount = value; }
		}


		private string productUnit;

		public string ProductUnit
		{
			get { return productUnit; }
			set { productUnit = value; }
		}


		private bool containerVM;

		public bool ContainerVM
		{
			get { return containerVM; }
			set { containerVM = value; }
		}


		private bool dummyProduct;

		public bool DummyProduct
		{
			get { return dummyProduct; }
			set { dummyProduct = value; }
		}


		private float priceOther;
        public float PriceOther
        {
            get { return priceOther; }
            set { priceOther = value; }
        }

        private float priceTakeout;
        public float PriceTakeout
        {
            get { return priceTakeout; }
            set { priceTakeout = value; }
        }

        private float priceEatIn;
		public float PriceEatIn
		{
			get { return priceEatIn; }
			set { priceEatIn = value; }
		}

		private List<string> distribution;
		public List<string> Distribution
		{
			get { return distribution; }
			set { distribution = value; }
		}


		private SalesType salesType;
		public SalesType SalesType
		{
			get { return salesType; }
			set { salesType = value; }
		}

		private bool upsizable;
		public bool Upsizable
		{
			get { return upsizable; }
			set { upsizable = value; }
		}


		private string dayPartCode;
		public string DayPartCode
		{
			get { return dayPartCode; }
			set { dayPartCode = value; }
		}

		private string familyGroup;
		public string FamilyGroup
		{
			get { return familyGroup; }
			set { familyGroup = value; }
		}

		private bool secondary;
		public bool Secondary
		{
			get { return secondary; }
			set { secondary = value; }
		}

		private List<int> categoryID;
		public List<int> CategoryID
		{
			get { return categoryID; }
			set { categoryID = value; }
		}

		private int productCode;
		public int ProductCode
		{
			get { return productCode; }
			set { productCode = value; }
		}

		private bool modified;
		public bool Modified
		{
			get { return modified; }
			set { modified = value; }
		}

		private bool salable;
		public bool Salable
		{
			get { return salable; }
			set { salable = value; }
		}

		private string productCategory;
		public string ProductCategory
		{
			get { return productCategory; }
			set { productCategory = value; }
		}

		private string productClass;
		public string ProductClass
		{
			get { return productClass; }
			set { productClass = value; }
		}

		private string statusCode;
		public string StatusCode
		{
			get { return statusCode; }
			set { statusCode = value; }
		}

	}
}
