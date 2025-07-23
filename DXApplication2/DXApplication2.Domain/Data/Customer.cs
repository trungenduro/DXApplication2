using System.ComponentModel.DataAnnotations;

namespace DXApplication2.Domain.Data
{
    public class DHFOrder1
    {
		public DHFOrder1()
		{
			    
		}
		public int Id { get; set; }
        public override int GetHashCode()
        {
            return Id;
        }
     
        public string OrderNo { get; set; } = string.Empty;
        public string Custormer { get; set; } = string.Empty;
    
        public int Total { get; set; } = 0;
        public string SiteName
        {
            get; set;
        } = "";

    }
		public class Customer
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Company { get; set; }
        public string? Email { get; set; }

        public string FullName => $"{FirstName} {LastName}";
		public string OrderNo { get; set; } = string.Empty;
		public string Custormer { get; set; } = string.Empty;

		public int Total { get; set; } = 0;
		public string SiteName
		{
			get; set;
		} = "";
		public Customer() { }
        public Customer(int id)
        {
            Id = id;
			OrderNo = $"OrderNo-00{id}";
			Custormer = $"Custormer-00{id}";
            Company = $"Company{id}";
            Email = $"{FirstName}.{LastName}@{Company}.com";
        }

        public override int GetHashCode()
        {
            return Id;
        }
        public override bool Equals(object? obj)
        {
            if (obj is not Customer customer)
                return false;
            return Id == customer.Id;
        }
        public static void Copy(Customer source, Customer target)
        {
            target.Id = source.Id;
            target.FirstName = source.FirstName;
            target.LastName = source.LastName;
            target.Company = source.Company;
            target.Email = source.Email;
        }
    }
}