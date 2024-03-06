using ExpanceTracker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Transactions;
using Transaction = ExpanceTracker.Models.Transaction;

namespace ExpanceTracker.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ActionResult> Index()
        {
            // last 7 days 

            DateTime StartDate = DateTime.Today.AddDays(-6);
            DateTime EndDate = DateTime.Today;

            List<Transaction> SelectedTransacations = await _context.Transactions.Include(x => x.Category).Where(y => y.Date >= StartDate && y.Date <= EndDate).ToListAsync();

            // total income

            int TotalIncome = SelectedTransacations.Where(i => i.Category.Type =="Income").Sum(j => j.Amount);
            ViewBag.TotalIncome = TotalIncome.ToString("C0", System.Globalization.CultureInfo.GetCultureInfo("en-US"));

            // total Expance

            int TotalExpence = SelectedTransacations.Where(i => i.Category.Type == "Expence").Sum(j => j.Amount);
            ViewBag.TotalExpence = TotalExpence.ToString("C0", System.Globalization.CultureInfo.GetCultureInfo("en-US"));

            //balance

            int Balance = TotalIncome - TotalExpence;
            ViewBag.Balance = Balance.ToString("C0", System.Globalization.CultureInfo.GetCultureInfo("en-US"));

            //dunatchart - expence by category
            ViewBag.DunatchartData = SelectedTransacations.Where(i => i.Category.Type == "Expence").GroupBy(j => j.Category.CategoryId).Select(k => new
            {
                categoryTitleWithIcon = k.First().Category.Icon + "" + k.First().Category.Title,
                amount = k.Sum(j => j.Amount),
                formattedAmount = k.Sum(j => j.Amount).ToString("C0"),
            }).OrderByDescending(m=>m.amount)
                .ToList();


            // spline chart mate - income vs expense
            //income
            List<SplineChartData> IncomeSummery = SelectedTransacations.Where(m => m.Category.Type == "Income").GroupBy(p => p.Date).Select(s => new SplineChartData()
            {
                day = s.First().Date.ToString("dd-MMM"),
                income = s.Sum(l => l.Amount),
            }).ToList();

            //expence
            List<SplineChartData> ExpenceSummery = SelectedTransacations.Where(m => m.Category.Type == "Expence").GroupBy(p => p.Date).Select(s => new SplineChartData()
            {
                day = s.First().Date.ToString("dd-MMM"),
                expence = s.Sum(l => l.Amount),
            }).ToList();

            // combine income and expence

            string[] Last7Days = Enumerable.Range(0, 7).Select(i=> StartDate.AddDays(i).ToString("dd-MMM")).ToArray();

            // have upar ni 3 list ne combine karva mate viewbag ma 

            ViewBag.SplineChartData = from day in Last7Days
                                      join income in IncomeSummery on day equals income.day into dayIncomeJoined
                                      from
                                      income in dayIncomeJoined.DefaultIfEmpty()
                                      join expence in ExpenceSummery on day equals expence.day into expenceJoined
                                      from expence in expenceJoined.DefaultIfEmpty()
                                      select new
                                      {
                                          day = day,
                                          income = income == null ? 0 : income.income,
                                          expence = expence == null ? 0 : expence.expence,
                                      };

            // now recent transactions

            ViewBag.RecentTransactions = await _context.Transactions.Include(i => i.Category).OrderByDescending(j => j.Date).Take(7).ToListAsync();

            return View();
        }
    }

    public class SplineChartData
    {
        public string day;
        public int income;
        public int expence;
    }
}
