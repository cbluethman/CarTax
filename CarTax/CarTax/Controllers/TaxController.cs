using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarTax.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class TaxController : ControllerBase
	{
		private readonly ILogger<TaxController> _logger;
		public TaxController(ILogger<TaxController> logger)
		{
			_logger = logger;
		}

		[HttpGet]
		public IEnumerable<TaxResult> Get(string state, int income)
		{
			TaxResult tr = new TaxResult();

			state = state.ToUpper();

			string[] states = new string[] { "NY", "CA", "VA" };

			if (String.IsNullOrEmpty(state) || state == "UNDEFINED" )
			{
				tr.formula = string.Empty;
				tr.tax = 0;
			}
			else if (states.Contains(state))
			{
				var taxTable = new[]
				{
					new { Lower = 0m, Upper = 40000m, Rate = 0m },
					new { Lower = 40001m, Upper = 86375m, Rate = .12m },
					new { Lower = 86376m, Upper = 164925m, Rate = .22m },
					new { Lower = 164926m, Upper = 209425m, Rate = .24m },
					new { Lower = 209426m, Upper = 523600m, Rate = .35m },
					new { Lower = 523601m, Upper = decimal.MaxValue, Rate = .37m }
				};

				var row = taxTable.Where(t => (income >= t.Lower &&
					income <= t.Upper));

				decimal rate = row.ToList()[0].Rate;

				if ( rate == 0)
				{
					tr.formula = "flat";
					tr.tax = 6000;
				}
				else
				{
					tr.formula = "progressive";
					tr.tax = (1 + rate) * income;
				}
			}
			else
			{
				tr.formula = "fixed";
				tr.tax = 6000;
			}

			var result = new List<TaxResult>();

			result.Add(tr);

			return (result);
		}
	}
}
