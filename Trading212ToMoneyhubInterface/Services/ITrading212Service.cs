
namespace Trading212ToMoneyhubInterface.Services
{
    public interface ITrading212Service
    {
        Task<decimal> GetPortfolioBalance();
    }
}