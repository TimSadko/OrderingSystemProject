using System.ComponentModel.DataAnnotations;

namespace OrderingSystemProject.Models;

public enum CardFilterType
{
    [Display(Name = "Lunch")] LUNCH = 0,

    [Display(Name = "Dinner")] DINNER = 1,

    [Display(Name = "Drinks")] DRINKS = 2,

    [Display(Name = "Alcoholic Drinks")] ALCOHOLIC_DRINKS = 3,

    [Display(Name = "All")] ALL = -1
}