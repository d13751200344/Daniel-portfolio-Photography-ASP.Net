using Microsoft.AspNetCore.Mvc;
using Photography.Models;

namespace Photography.Components.ViewComponents
{
    public class NavigationMenuViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            var menuItems = new List<MenuItem>
            {
                new MenuItem { Controller = "Home", Action = "Index", Label = "Home"},
                new MenuItem { Controller = "Gallery", Action = "Index", Label = "Gallery",
                        DropdownItems = new List<MenuItem>
                            { 
                                new MenuItem { Controller = "Gallery", Action = "Product", Label = "Product"},
                                new MenuItem { Controller = "Gallery", Action = "Service", Label = "Service"},
                                new MenuItem { Controller = "Gallery", Action = "Food", Label = "Food"},
                                new MenuItem { Controller = "Gallery", Action = "Wedding", Label = "Wedding"},
                                new MenuItem { Controller = "Gallery", Action = "Property", Label = "Property"},

                            } },
                new MenuItem { Controller = "Courses", Action = "Index", Label = "Course"},
                new MenuItem { Controller = "Home", Action = "About", Label = "About"},
                new MenuItem { Controller = "Home", Action = "Privacy", Label = "Privacy"},
             };

            return View(menuItems);
        }
    }
}
