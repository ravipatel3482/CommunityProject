using System.Numerics;

namespace UsindianCommunity.Models
{
    public class Business
    {
        public BigInteger Id { get; set; }
        public BusinessType BusinessType { get; set; }

        // Expense Related Property
        public int NoOfEmployeeRequired { get; set; }
        public double MonthlyPayout { get; set; }
        public double Electricity { get; set; }
        public double WaterBill { get; set; }
        public double AccountantExpense { get; set; }
        public double insurance { get; set; }
        // Income Relatred Property 
        public double LotteryCommision { get; set; }
        public double AtmCommision { get; set; }
        public double GasolinCommision { get; set; }
        public double InsideSale { get; set; }
        public double AirMachineCommision { get; set; }
        public double NewsPaperCommision { get; set; }
        // selling realated property
        public double PropertyValue { get; set; }
        public SellType SellType { get; set; }

        //Prize 
        public double PropertyPrize { get; set; }
        public double Rent { get; set; }
        public double GoodWill { get; set; }


    }
}
