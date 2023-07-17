using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace webapi.Helpers
{
	public enum UnitEnum
	{
		[Display(Name = "pump(s)")]
		Pump = 1,
		[Display(Name = "tsp(s)")]
		Tsp = 2,
		[Display(Name = "shot(s)")]
		Shot = 3,
		[Display(Name = "Other")]
		Other = 4,
		[Display(Name = "Undefined")]
		Undefined = 5,
		[Display(Name = "Base")]
		Base = 6
	}
}
