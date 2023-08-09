using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace webapi.Helpers
{
	public enum StatusEnum
	{
		/* Drink Status */
		[Display(Name = "New Order")] // In cart
		NewOrder = 1,
		[Display(Name = "In Progress")] // Order submitted
		InProgress = 2,
		[Display(Name = "Completed")]
		Completed = 3,
		[Display(Name = "Cancelled")]
		Cancelled = 4,
		/* Ingredient Status */
		[Display(Name = "Active")]
		Active = 5, // Default / Available
		[Display(Name = "Inactive")]
		Inactive = 6, // Unavailable at the moment
		[Display(Name = "Deleted")]
		Deleted = 7 // Hidden from the list
	}
}
