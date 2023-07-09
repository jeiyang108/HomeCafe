using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace webapi.Helpers
{
	public enum StatusEnum
	{
		/* Ingredient Status */
		Active = 5, // Default / Available
		Inactive = 6, // Unavailable at the moment
		Deleted = 7 // Hidden from the list
	}
}
